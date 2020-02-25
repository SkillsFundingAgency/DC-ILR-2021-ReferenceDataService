using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData.CollectionDates
{
    public class IlrCollectionDates
    {
        public IReadOnlyCollection<ReturnPeriod> ReturnPeriods { get; set; }

        public IReadOnlyCollection<CensusDate> CensusDates { get; set; }
    }
}
