using System;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population
{
    public class AcademicYearDataService : IAcademicYearDataService
    {
        private readonly int academicYearStartYear = 2020;
        private readonly int academicYearEndYear = 2021;

        public DateTime CurrentYearStart => new DateTime(academicYearStartYear, 08, 01);

        public DateTime CurrentYearEnd => new DateTime(academicYearEndYear, 07, 31);
    }
}
