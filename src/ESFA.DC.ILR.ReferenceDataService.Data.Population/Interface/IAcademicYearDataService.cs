using System;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface
{
    public interface IAcademicYearDataService
    {
        DateTime CurrentYearStart { get; }

        DateTime CurrentYearEnd { get; }
    }
}
