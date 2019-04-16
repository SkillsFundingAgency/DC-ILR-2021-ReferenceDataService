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

        public string OutputReferenceDataFileKey => _jobContextMessage.KeyValuePairs[JobContextMessageKey.IlrReferenceData].ToString();
    }
}
