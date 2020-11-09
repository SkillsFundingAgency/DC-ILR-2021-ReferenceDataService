using System;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.JobContextManager.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Context
{
    public class DesktopReferenceDataJobContext : IDesktopReferenceDataContext
    {
        private readonly JobContextMessage _jobContextMessage;

        public DesktopReferenceDataJobContext(JobContextMessage jobContextMessage)
        {
            _jobContextMessage = jobContextMessage;
        }

        public long JobId => _jobContextMessage.JobId;

        public string CollectionName => _jobContextMessage.KeyValuePairs[JobContextMessageKey.CollectionName].ToString();

        public string Container => _jobContextMessage.KeyValuePairs[JobContextMessageKey.Container].ToString();

        public DateTime SubmissionDateTimeUTC => _jobContextMessage.SubmissionDateTimeUtc;
    }
}
