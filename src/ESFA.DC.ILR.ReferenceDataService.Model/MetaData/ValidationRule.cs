using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.ILR.ReferenceDataService.Model.MetaData
{
    public class ValidationRule
    {
        public string RuleName { get; set; }

        public bool Desktop { get; set; }

        public bool Online { get; set; }
    }
}
