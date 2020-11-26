using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.EAS2021.EF;
using ESFA.DC.EAS2021.EF.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Abstract;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using ESFA.DC.ReferenceData.LARS.Model.Interface;
using ESFA.DC.ReferenceData.Organisations.Model.Interface;
using ESFA.DC.ReferenceData.Postcodes.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population
{
    public class MetaDataRetrievalService : AbstractMetaDataRetrievalService, IMetaDataRetrievalService
    {
        private readonly IDbContextFactory<IEasdbContext> _easContextFactory;

        public MetaDataRetrievalService(
            IDbContextFactory<IEasdbContext> easContextFactory,
            IDbContextFactory<IEmployersContext> employersContextFactory,
            IDbContextFactory<ILARSContext> larsContextFactory,
            IDbContextFactory<IOrganisationsContext> organisationsContextFactory,
            IDbContextFactory<IPostcodesContext> postcodesContextFactory,
            IIlrReferenceDataRepositoryService ilReferenceDataRepositoryService,
            IDateTimeProvider dateTimeProvider)
            : base(employersContextFactory, larsContextFactory, organisationsContextFactory, postcodesContextFactory, ilReferenceDataRepositoryService, dateTimeProvider)
        {
            _easContextFactory = easContextFactory;
        }

        public async Task<MetaData> RetrieveAsync(int ukprn, CancellationToken cancellationToken)
        {
            var metaData = await RetrieveAsync(cancellationToken);
            var latestEAS = await RetrieveLatestEasAsync(ukprn, cancellationToken);

            metaData.ReferenceDataVersions.EasFileDetails = new EasFileDetails
            {
                FileName = Path.GetFileName(latestEAS?.FileName),
                UploadDateTime = latestEAS?.DateTime
            };

            return metaData;
        }

        private async Task<SourceFile> RetrieveLatestEasAsync(int ukprn, CancellationToken cancellationToken)
        {
            using (var context = _easContextFactory.Create())
            {
                return await context.SourceFiles.OrderByDescending(x => x.SourceFileId).FirstOrDefaultAsync(v => v.Ukprn == ukprn.ToString(), cancellationToken);
            }
        }
    }
}
