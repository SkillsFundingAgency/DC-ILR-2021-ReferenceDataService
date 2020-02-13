using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.JobContextManager.Model;

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

        public string InputReferenceDataFileKey => _jobContextMessage.KeyValuePairs["FISReferenceData"].ToString();

        public string OutputReferenceDataFileKey => _jobContextMessage.KeyValuePairs[JobContextMessageKey.IlrReferenceData].ToString();

        public string FrmReferenceDataFileKey => _jobContextMessage.KeyValuePairs["FrmReferenceData"].ToString();

        public string Task => _jobContextMessage.Topics[_jobContextMessage.TopicPointer].Tasks.SelectMany(x => x.Tasks).First();

        public int ReturnPeriod
        {
            get => int.Parse(_jobContextMessage.KeyValuePairs[JobContextMessageKey.ReturnPeriod].ToString());
            set => _jobContextMessage.KeyValuePairs[JobContextMessageKey.ReturnPeriod] = value;
        }

        public string ValidationMessagesFileReference => _jobContextMessage.KeyValuePairs["ValidationMessagesFileReference"].ToString();

        public int Ukprn
        {
            get => int.Parse(_jobContextMessage.KeyValuePairs[JobContextMessageKey.UkPrn].ToString());
            set => throw new System.NotImplementedException();
        }
    }
}
