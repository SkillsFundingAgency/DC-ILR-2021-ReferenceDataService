using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.ILR.ReferenceDataService.Model.EPAOrganisations;
using ESFA.DC.ILR.ReferenceDataService.Model.LARS;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.Organisations;
using ESFA.DC.ILR.ReferenceDataService.Model.Postcodes;
using ESFA.DC.ILR.ReferenceDataService.Model.PostcodesDevolution;

namespace ESFA.DC.ILR.ReferenceDataService.Service
{
    public interface IZipFileService
    {
        Task SaveCollectionZipAsync(
            string zipFileName,
            string container,
            MetaData metaDatas,
            DevolvedPostcodes devolvedPostocdes,
            IReadOnlyCollection<Employer> employers,
            IReadOnlyCollection<EPAOrganisation> ePAOrganisations,
            IReadOnlyCollection<LARSFrameworkDesktop> larsFrameworks,
            IReadOnlyCollection<LARSFrameworkAimDesktop> larsFrameworkAims,
            IReadOnlyCollection<LARSLearningDelivery> larsLearningDeliveries,
            IReadOnlyCollection<LARSStandard> larsStandards,
            IReadOnlyCollection<Organisation> organisations,
            IReadOnlyCollection<Postcode> postcodes,
            CancellationToken cancellationToken);
    }
}
