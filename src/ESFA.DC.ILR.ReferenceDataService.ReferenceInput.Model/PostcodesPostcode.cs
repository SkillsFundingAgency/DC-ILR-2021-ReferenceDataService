using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class PostcodesPostcode
    {
        public PostcodesPostcode()
        {
            PostcodesDasDisadvantage = new HashSet<PostcodesDasDisadvantage>();
            PostcodesEfaDisadvantage = new HashSet<PostcodesEfaDisadvantage>();
            PostcodesMcaglaSof = new HashSet<PostcodesMcaglaSof>();
            PostcodesOnsdata = new HashSet<PostcodesOnsdata>();
            PostcodesSfaAreaCost = new HashSet<PostcodesSfaAreaCost>();
            PostcodesSfaDisadvantage = new HashSet<PostcodesSfaDisadvantage>();
        }

        public int Id { get; set; }
        public string PostCode { get; set; }

        public virtual ICollection<PostcodesDasDisadvantage> PostcodesDasDisadvantage { get; set; }
        public virtual ICollection<PostcodesEfaDisadvantage> PostcodesEfaDisadvantage { get; set; }
        public virtual ICollection<PostcodesMcaglaSof> PostcodesMcaglaSof { get; set; }
        public virtual ICollection<PostcodesOnsdata> PostcodesOnsdata { get; set; }
        public virtual ICollection<PostcodesSfaAreaCost> PostcodesSfaAreaCost { get; set; }
        public virtual ICollection<PostcodesSfaDisadvantage> PostcodesSfaDisadvantage { get; set; }
    }
}
