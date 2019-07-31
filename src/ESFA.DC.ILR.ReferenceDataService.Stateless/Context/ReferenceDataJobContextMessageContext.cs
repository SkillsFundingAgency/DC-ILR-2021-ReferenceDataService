using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.JobContextManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Context
{
    public class ReferenceDataJobContextMessageContext : IReferenceDataContext
    {
        private readonly JobContextMessage _jobContextMessage;

        public ReferenceDataJobContextMessageContext(JobContextMessage jobContextMessage)
        {
            _jobContextMessage = jobContextMessage;
        }

        public string FileReference
        {
            get => _jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename].ToString();
            set => _jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename] = value;
        }

        public string OriginalFileReference
        {
            get => _jobContextMessage.KeyValuePairs[JobContextMessageKey.OriginalFilename].ToString();
            set => _jobContextMessage.KeyValuePairs[JobContextMessageKey.OriginalFilename] = value;
        }

        public string Container => _jobContextMessage.KeyValuePairs[JobContextMessageKey.Container].ToString();

        public string OutputReferenceDataFileKey => _jobContextMessage.KeyValuePairs[JobContextMessageKey.IlrReferenceData].ToString();

        public string Task => _jobContextMessage.Topics[_jobContextMessage.TopicPointer].Tasks.SelectMany(x => x.Tasks).First();

        public int ReturnPeriod
        {
            get => int.Parse(_jobContextMessage.KeyValuePairs[JobContextMessageKey.ReturnPeriod].ToString());
            set => _jobContextMessage.KeyValuePairs[JobContextMessageKey.ReturnPeriod] = value;
        }
    }
}
