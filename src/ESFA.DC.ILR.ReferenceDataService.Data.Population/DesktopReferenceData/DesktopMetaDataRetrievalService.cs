using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Abstract;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.DesktopReferenceData
{
    public class DesktopMetaDataRetrievalService : AbstractMetaDataRetrievalService, IDesktopMetaDataRetrievalService
    {
        private const string _easFileName = "N/A";

        public DesktopMetaDataRetrievalService(
            IDbContextFactory<IEmployersContext> employersContextFactory,
            IDbContextFactory<ILARSContext> larsContextFactory,
            IDbContextFactory<IOrganisationsContext> organisationsContextFactory,
            IDbContextFactory<IPostcodesContext> postcodesContextFactory,
            IIlrReferenceDataRepositoryService ilReferenceDataRepositoryService,
            IDateTimeProvider dateTimeProvider)
            : base(employersContextFactory, larsContextFactory, organisationsContextFactory, postcodesContextFactory, ilReferenceDataRepositoryService, dateTimeProvider)
        {
        }

        public async Task<MetaData> RetrieveDesktopMetaDataAsync(CancellationToken cancellationToken)
        {
            var metaData = await RetrieveAsync(cancellationToken);

            metaData.ReferenceDataVersions.EasFileDetails = new EasFileDetails
            {
                FileName = _easFileName
            };

            return metaData;
        }
    }
}
