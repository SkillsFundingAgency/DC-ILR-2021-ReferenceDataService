using System;
using System.Linq;

namespace ESFA.DC.ILR.ReferenceDataService.ILRReferenceData.Model.Interface
{
    public interface IIlrReferenceDataContext : IDisposable
    {
        IQueryable<Rule> Rules { get; }

        IQueryable<Lookup> Lookups { get; }
    }
}
