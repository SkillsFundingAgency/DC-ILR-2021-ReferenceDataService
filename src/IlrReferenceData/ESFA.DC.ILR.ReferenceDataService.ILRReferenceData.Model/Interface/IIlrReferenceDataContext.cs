using System.Linq;

namespace ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model.Interface
{
    public interface IIlrReferenceDataContext
    {
        IQueryable<Rule> Rules { get; }

        IQueryable<Lookup> Lookups { get; }
    }
}
