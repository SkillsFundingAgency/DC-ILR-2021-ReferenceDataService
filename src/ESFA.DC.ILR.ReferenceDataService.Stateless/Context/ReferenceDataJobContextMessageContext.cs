using ESFA.DC.ILR.Constants;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
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
            get => _jobContextMessage.KeyValuePairs[ILRJobContextMessageKeys.Filename].ToString();
            set => _jobContextMessage.KeyValuePairs[ILRJobContextMessageKeys.Filename] = value;
        }

        public string OriginalFileReference
        {
            get => _jobContextMessage.KeyValuePairs[ILRJobContextMessageKeys.OriginalFilename].ToString();
            set => _jobContextMessage.KeyValuePairs[ILRJobContextMessageKeys.OriginalFilename] = value;
        }

        public string Container => _jobContextMessage.KeyValuePairs[ILRJobContextMessageKeys.Container].ToString();

        public string OutputReferenceDataFileKey => _jobContextMessage.KeyValuePairs[ILRJobContextMessageKeys.OutputReferenceDataFileKey].ToString();
    }
}
