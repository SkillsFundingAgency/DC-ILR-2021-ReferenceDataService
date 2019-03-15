using ESFA.DC.ILR.ReferenceDataService.Stateless.ServiceFabric.Config.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.ServiceFabric.Config
{
    public class StatelessServiceConfiguration : IStatelessServiceConfiguration
    {
        public string ServiceBusConnectionString { get; set; }

        public string TopicName { get; set; }

        public string SubscriptionName { get; set; }

        public string TopicMaxConcurrentCalls { get; set; }

        public string TopicMaxCallbackTimeSpanMinutes { get; set; }

        public string JobStatusQueueName { get; set; }

        public string JobStatusMaxConcurrentCalls { get; set; }

        public string AuditQueueName { get; set; }

        public string AuditMaxConcurrentCalls { get; set; }

        public string LoggerConnectionString { get; set; }
    }
}
