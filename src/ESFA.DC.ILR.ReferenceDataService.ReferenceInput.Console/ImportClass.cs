using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using CommandLine;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Context;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Modules;
using ESFA.DC.ILR.ReferenceDataService.Desktop.Service.Interface;
using ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Console.modules;
using ESFA.DC.Logging.Desktop.Config;
using ESFA.DC.Logging.Desktop.Config.Interfaces;
using ESFA.DC.Logging.Interfaces;
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
                Parser.Default.ParseArguments<CommandLineArguments>(args)
                    .WithParsed(async cla =>
                    {
                        var logger = container.Resolve<ILogger>();

                        if (!File.Exists((cla.SourceFile)))
                        {
                            System.Console.WriteLine($"Source file does not exist ({cla.SourceFile})");
                        }

                        try
                        {
                            var referenceInputDataPopulationService =
                                container.Resolve<IReferenceInputDataPopulationService>();
                            var config = container.Resolve<IConfiguration>();

                            var cancellationToken = new CancellationToken();

                            var inputReferenceDataContext = new InputReferenceDataContext
                            {
                                InputReferenceDataFileKey = Path.GetFileName(cla.SourceFile),
                                Container = Path.GetDirectoryName(cla.SourceFile),
                                ConnectionString = config.GetConnectionString("targetServer")
                            };


                            await referenceInputDataPopulationService.PopulateAsync(inputReferenceDataContext,
                                cancellationToken);

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
