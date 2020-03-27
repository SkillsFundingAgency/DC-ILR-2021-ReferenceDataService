using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using CommandLine;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Context;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Modules;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Console.modules;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Interface;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Mapping.Message;
using ESFA.DC.Logging.Desktop.Config;
using ESFA.DC.Logging.Desktop.Config.Interfaces;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;
using Microsoft.Extensions.Configuration;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Console
{
    public class ImportClass
    {
        public void ImportFile(string[] args)
        {
            using (var container = BuildContainerBuilder().Build())
            {
                var messengerService = container.Resolve<IMessengerService>();

                Parser.Default.ParseArguments<CommandLineArguments>(args)
                    .WithParsed(cla =>
                    {
                        if (!File.Exists((cla.SourceFile)))
                        {
                            System.Console.WriteLine($"Source file does not exist ({cla.SourceFile})");
                            return;
                        }

                        try
                        {
                            var referenceInputDataPopulationService = container.Resolve<IReferenceInputDataPopulationService>();
                            var config = container.Resolve<IConfiguration>();

                            var cancellationToken = new CancellationToken();

                            var inputReferenceDataContext = new InputReferenceDataContext
                            {
                                InputReferenceDataFileKey = Path.GetFileName(cla.SourceFile),
                                Container = Path.GetDirectoryName(cla.SourceFile),
                                ConnectionString = config.GetConnectionString("targetServer")
                            };

                            messengerService.Register<TaskProgressMessage>(this, HandleTaskProgressMessage);

                            System.Console.WriteLine("Reference Data Import Console" + Environment.NewLine + Environment.NewLine);

                            var task = Task.Run(async () => await referenceInputDataPopulationService.PopulateAsyncByType(inputReferenceDataContext, cancellationToken), cancellationToken);
                            var result = task.Result;

                            System.Console.WriteLine("Reference data import completed.");
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine(ex);

                            throw;
                        }
                    });
            }
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = System.Console.CursorTop;
            System.Console.SetCursorPosition(0, System.Console.CursorTop);
            System.Console.Write(new string(' ', System.Console.WindowWidth));
            System.Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void OverwriteConsoleText(string newMessage)
        {
            System.Console.SetCursorPosition(0, System.Console.CursorTop - 1);
            ClearCurrentConsoleLine();
            System.Console.WriteLine(newMessage);
        }

        public static void HandleTaskProgressMessage(TaskProgressMessage taskProgressMessage)
        {
            OverwriteConsoleText(taskProgressMessage.TaskName);
        }

        private static ContainerBuilder BuildContainerBuilder()
        {
            var containerBuilder = new ContainerBuilder();
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appSettings.json");
            IConfiguration config = configBuilder.Build();

            // bind logger settings
            DesktopLoggerSettings settings = new DesktopLoggerSettings();
            config.GetSection("Logging").Bind(settings);
            containerBuilder.RegisterInstance<IConfiguration>(config);
            containerBuilder.RegisterInstance<IDesktopLoggerSettings>(settings);


            containerBuilder.RegisterModule<ReferenceDataInputModule>();
            containerBuilder.RegisterModule(new LoggingModule(settings));

            containerBuilder.RegisterType<JsonSerializationService>().As<IJsonSerializationService>();

            return containerBuilder;
        }
    }
}
