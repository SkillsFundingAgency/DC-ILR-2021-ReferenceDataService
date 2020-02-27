using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class MetaDataValidationError
    {
        public int Id { get; set; }
        public string RuleName { get; set; }
        public int Severity { get; set; }
        public string Message { get; set; }
    }
}
