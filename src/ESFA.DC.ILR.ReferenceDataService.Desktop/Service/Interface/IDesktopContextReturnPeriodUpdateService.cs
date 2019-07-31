using System;
using System.Collections.Generic;
using ESFA.DC.ILR.ReferenceDataService.Interfaces;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.CollectionDates;

namespace ESFA.DC.ILR.ReferenceDataService.Desktop.Service
{
    public interface IDesktopContextReturnPeriodUpdateService
    {
        void UpdateCollectionPeriod(IReferenceDataContext referenceDataContext, DateTime filePrepDate, IReadOnlyCollection<ReturnPeriod> returnPeriods);
    }
}
