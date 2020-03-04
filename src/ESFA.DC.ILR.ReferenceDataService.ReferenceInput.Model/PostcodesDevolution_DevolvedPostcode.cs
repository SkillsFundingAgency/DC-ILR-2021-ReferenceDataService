using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class PostcodesDevolution_DevolvedPostcode
    {
        public PostcodesDevolution_DevolvedPostcode()
        {
            PostcodesDevolution_McaGlaSofLookups = new HashSet<PostcodesDevolution_McaGlaSofLookup>();
            PostcodesDevolution_Postcodes = new HashSet<PostcodesDevolution_Postcode>();
        }

        public int Id { get; set; }

        public virtual ICollection<PostcodesDevolution_McaGlaSofLookup> PostcodesDevolution_McaGlaSofLookups { get; set; }
        public virtual ICollection<PostcodesDevolution_Postcode> PostcodesDevolution_Postcodes { get; set; }
    }
}
