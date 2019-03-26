using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Model;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population
{
    public class MessageMapperService : IMessageMapperService
    {
        private readonly IEmpIdMapper _empIdMapper;
        private readonly IEpaOrgIdMapper _epaOrgIdMapper;
        private readonly IFM36UlnMapper _fM36UlnMapper;
        private readonly ILearnAimRefMapper _learnAimRefMapper;
        private readonly ILearningProviderUkprnMapper _learningProviderUkprnMapper;
        private readonly IPostcodesMapper _postcodesMapper;
        private readonly IStandardCodesMapper _standardCodesMapper;
        private readonly IUkprnsMapper _ukprnsMapper;
        private readonly IUlnMapper _ulnMapper;

        public MessageMapperService(
            IEmpIdMapper empIdMapper,
            IEpaOrgIdMapper epaOrgIdMapper,
            IFM36UlnMapper fM36UlnMapper,
            ILearnAimRefMapper learnAimRefMapper,
            ILearningProviderUkprnMapper learningProviderUkprnMapper,
            IPostcodesMapper postcodesMapper,
            IStandardCodesMapper standardCodesMapper,
            IUkprnsMapper ukprnsMapper,
            IUlnMapper ulnMapper)
        {
            _empIdMapper = empIdMapper;
            _epaOrgIdMapper = epaOrgIdMapper;
            _fM36UlnMapper = fM36UlnMapper;
            _learnAimRefMapper = learnAimRefMapper;
            _learningProviderUkprnMapper = learningProviderUkprnMapper;
            _postcodesMapper = postcodesMapper;
            _standardCodesMapper = standardCodesMapper;
            _ukprnsMapper = ukprnsMapper;
            _ulnMapper = ulnMapper;
        }

        public MapperData MapFromMessage(IMessage message)
        {
            return new MapperData
            {
                EmployerIds = _empIdMapper.MapEmpIdsFromMessage(message),
                EpaOrgIds = _epaOrgIdMapper.MapEpaOrgIdsFromMessage(message),
                FM36Ulns = _fM36UlnMapper.MapFM36UlnsFromMessage(message),
                LearnAimRefs = _learnAimRefMapper.MapLearnAimRefsFromMessage(message),
                LearningProviderUKPRN = _learningProviderUkprnMapper.MapLearningProviderUKPRNFromMessage(message),
                Postcodes = _postcodesMapper.MapPostcodesFromMessage(message),
                StandardCodes = _standardCodesMapper.MapStandardCodesFromMessage(message),
                UKPRNs = _ukprnsMapper.MapUKPRNsFromMessage(message),
                ULNs = _ulnMapper.MapUlnsFromMessage(message),
            };
        }
    }
}
