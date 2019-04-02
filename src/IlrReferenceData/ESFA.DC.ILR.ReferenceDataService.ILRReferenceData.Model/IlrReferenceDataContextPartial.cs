using System.Linq;
using ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model
{
    public partial class IlrReferenceDataContext : IIlrReferenceDataContext
    {
        IQueryable<Rule> IIlrReferenceDataContext.Rules => Rules;

        IQueryable<Lookup> IIlrReferenceDataContext.Lookups => Lookups;
    }
}
