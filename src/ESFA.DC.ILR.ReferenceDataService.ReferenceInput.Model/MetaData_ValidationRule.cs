using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaData_ValidationRule
    {
        public int Id { get; set; }
        public string RuleName { get; set; }
        public bool Desktop { get; set; }
        public bool Online { get; set; }
    }
}
