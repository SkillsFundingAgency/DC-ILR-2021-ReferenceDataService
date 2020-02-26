using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class PostcodesDevolutionDevolvedPostcodes
    {
        public PostcodesDevolutionDevolvedPostcodes()
        {
            PostcodesDevolutionDevolvedPostcode = new HashSet<PostcodesDevolutionDevolvedPostcode>();
            PostcodesDevolutionMcaGlaSofLookup = new HashSet<PostcodesDevolutionMcaGlaSofLookup>();
        }

        public int Id { get; set; }

        public virtual ICollection<PostcodesDevolutionDevolvedPostcode> PostcodesDevolutionDevolvedPostcode { get; set; }
        public virtual ICollection<PostcodesDevolutionMcaGlaSofLookup> PostcodesDevolutionMcaGlaSofLookup { get; set; }
    }
}
