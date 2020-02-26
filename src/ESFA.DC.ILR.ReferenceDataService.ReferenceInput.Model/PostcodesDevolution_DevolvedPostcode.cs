using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class PostcodesDevolution_DevolvedPostcode
    {
        public PostcodesDevolution_DevolvedPostcode()
        {
            PostcodesDevolution_DevolvedPostcode1s = new HashSet<PostcodesDevolution_DevolvedPostcode1>();
            PostcodesDevolution_McaGlaSofLookups = new HashSet<PostcodesDevolution_McaGlaSofLookup>();
        }

        public int Id { get; set; }

        public virtual ICollection<PostcodesDevolution_DevolvedPostcode1> PostcodesDevolution_DevolvedPostcode1s { get; set; }
        public virtual ICollection<PostcodesDevolution_McaGlaSofLookup> PostcodesDevolution_McaGlaSofLookups { get; set; }
    }
}
