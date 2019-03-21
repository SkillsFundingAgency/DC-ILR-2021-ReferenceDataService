using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Model
{
    public class MapperData
    {
        public IReadOnlyCollection<int> EmployerIds { get; set; }

        public IReadOnlyCollection<string> EpaOrgIds { get; set; }

        public IReadOnlyCollection<string> LearnAimRefs { get; set; }

        public int LearningProviderUKPRN { get; set; }

        public IReadOnlyCollection<string> Postcodes { get; set; }

        public IReadOnlyCollection<int> StandardCodes { get; set; }

        public IReadOnlyCollection<int> UKPRNs { get; set; }

        public IReadOnlyCollection<long> ULNs { get; set; }
    }
}
