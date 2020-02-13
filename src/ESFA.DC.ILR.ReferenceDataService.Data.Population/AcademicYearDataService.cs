using System;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population
{
    public class AcademicYearDataService : IAcademicYearDataService
    {
        private readonly int academicYearStartYear = 2019;

        public DateTime CurrentYearStart => new DateTime(academicYearStartYear, 08, 01);
    }
}
