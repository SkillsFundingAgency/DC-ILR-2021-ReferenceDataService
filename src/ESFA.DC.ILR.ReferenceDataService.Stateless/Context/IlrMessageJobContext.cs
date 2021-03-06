﻿using System;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Constants;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.JobContextManager.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Stateless.Context
{
    public class IlrMessageJobContext : IReferenceDataContext
    {
        private readonly JobContextMessage _jobContextMessage;

        public IlrMessageJobContext(JobContextMessage jobContextMessage)
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

        public long JobId => _jobContextMessage.JobId;

        public string CollectionName => _jobContextMessage.KeyValuePairs[JobContextMessageKey.CollectionName].ToString();

        public string Container => _jobContextMessage.KeyValuePairs[JobContextMessageKey.Container].ToString();

        public string DesktopInputReferenceDataFileKey => _jobContextMessage.KeyValuePairs[ReferenceDataContextKeys.DesktopInputReferenceDataKey].ToString();

        public string OutputIlrReferenceDataFileKey => _jobContextMessage.KeyValuePairs[JobContextMessageKey.IlrReferenceData].ToString();

        public string FrmReferenceDataFileKey => _jobContextMessage.KeyValuePairs[ReferenceDataContextKeys.FrmReferenceDataFileKey].ToString();

        public string LearnerReferenceDataFileKey => _jobContextMessage.KeyValuePairs[ReferenceDataContextKeys.LearnerReferenceDataFileKey].ToString();

        public string Task => _jobContextMessage.Topics[_jobContextMessage.TopicPointer].Tasks.SelectMany(x => x.Tasks).First();

        public int ReturnPeriod
        {
            get => int.Parse(_jobContextMessage.KeyValuePairs[JobContextMessageKey.ReturnPeriod].ToString());
            set => _jobContextMessage.KeyValuePairs[JobContextMessageKey.ReturnPeriod] = value;
        }

        public string ValidationMessagesFileReference => _jobContextMessage.KeyValuePairs[ReferenceDataContextKeys.ValidationMessagesFileReferenceKey].ToString();

        public int Ukprn
        {
            get => int.Parse(_jobContextMessage.KeyValuePairs[JobContextMessageKey.UkPrn].ToString());
            set => throw new System.NotImplementedException();
        }

        public DateTime SubmissionDateTimeUTC => _jobContextMessage.SubmissionDateTimeUtc;
    }
}
