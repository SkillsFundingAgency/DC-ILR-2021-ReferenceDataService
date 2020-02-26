using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaData_CensusDate
    {
        public int Id { get; set; }
        public int Period { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
