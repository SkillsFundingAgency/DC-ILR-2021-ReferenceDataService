using System;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaData_ReturnPeriod
    {
        public int Id { get; set; }
        public string Period { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
