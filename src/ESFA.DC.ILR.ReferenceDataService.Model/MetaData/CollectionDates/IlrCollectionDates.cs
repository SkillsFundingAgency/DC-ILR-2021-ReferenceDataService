using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData.CollectionDates
{
    public class IlrCollectionDates
    {
        public IReadOnlyCollection<ReturnPeriod> ReturnPeriods { get; set; }

        public IReadOnlyCollection<CensusDate> CensusDates { get; set; }
    }
}
