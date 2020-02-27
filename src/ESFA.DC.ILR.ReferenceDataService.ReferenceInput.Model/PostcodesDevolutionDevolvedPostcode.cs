using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class PostcodesDevolutionDevolvedPostcode
    {
        public PostcodesDevolutionDevolvedPostcode()
        {
            PostcodesDevolutionMcaGlaSofLookups = new HashSet<PostcodesDevolutionMcaGlaSofLookup>();
            PostcodesDevolutionPostcodes = new HashSet<PostcodesDevolutionPostcode>();
        }

        public int Id { get; set; }

        public virtual ICollection<PostcodesDevolutionMcaGlaSofLookup> PostcodesDevolutionMcaGlaSofLookups { get; set; }
        public virtual ICollection<PostcodesDevolutionPostcode> PostcodesDevolutionPostcodes { get; set; }
    }
}
