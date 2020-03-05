using Autofac;
using ESFA.DC.Logging.Desktop;
using ESFA.DC.Logging.Desktop.Config.Interfaces;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Console.modules
{
    public class LoggingModule : Module
    {
        private readonly IDesktopLoggerSettings _loggerSettings;

        public LoggingModule(IDesktopLoggerSettings loggerSettings)
        {
            _loggerSettings = loggerSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var loggerFactory = new DesktopLoggerFactory();

            builder.Register(c => loggerFactory.Build(_loggerSettings)).As<ILogger>().InstancePerLifetimeScope();
        }
    }
}
