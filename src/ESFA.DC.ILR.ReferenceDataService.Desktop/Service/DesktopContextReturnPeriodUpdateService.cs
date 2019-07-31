using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.CollectionDates;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service
{
    public class DesktopContextReturnPeriodUpdateService : IDesktopContextReturnPeriodUpdateService
    {
        public DesktopContextReturnPeriodUpdateService()
        {
        }

        public void UpdateCollectionPeriod(IReferenceDataContext referenceDataContext, DateTime filePrepDate, IReadOnlyCollection<ReturnPeriod> returnPeriods)
        {
            var r01 = returnPeriods.First(rp => rp.Period == 1).Start;
            var r14 = returnPeriods.First(rp => rp.Period == 14).End;
            var submissionDate =
                filePrepDate < r01 ? r01 :
                filePrepDate > r14 ? r14 :
                filePrepDate;

            referenceDataContext.ReturnPeriod = returnPeriods.First(rp => rp.Start <= submissionDate && rp.End >= submissionDate).Period;
        }
    }
}
