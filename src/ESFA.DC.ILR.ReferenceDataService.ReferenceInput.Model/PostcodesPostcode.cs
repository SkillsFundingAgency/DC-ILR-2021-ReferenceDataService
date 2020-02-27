using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class PostcodesPostcode
    {
        public PostcodesPostcode()
        {
            PostcodesDasDisadvantages = new HashSet<PostcodesDasDisadvantage>();
            PostcodesEfaDisadvantages = new HashSet<PostcodesEfaDisadvantage>();
            PostcodesMcaglaSofs = new HashSet<PostcodesMcaglaSof>();
            PostcodesOnsdatas = new HashSet<PostcodesOnsdata>();
            PostcodesSfaAreaCosts = new HashSet<PostcodesSfaAreaCost>();
            PostcodesSfaDisadvantages = new HashSet<PostcodesSfaDisadvantage>();
        }

        public int Id { get; set; }
        public string PostCode { get; set; }

        public virtual ICollection<PostcodesDasDisadvantage> PostcodesDasDisadvantages { get; set; }
        public virtual ICollection<PostcodesEfaDisadvantage> PostcodesEfaDisadvantages { get; set; }
        public virtual ICollection<PostcodesMcaglaSof> PostcodesMcaglaSofs { get; set; }
        public virtual ICollection<PostcodesOnsdata> PostcodesOnsdatas { get; set; }
        public virtual ICollection<PostcodesSfaAreaCost> PostcodesSfaAreaCosts { get; set; }
        public virtual ICollection<PostcodesSfaDisadvantage> PostcodesSfaDisadvantages { get; set; }
    }
}
