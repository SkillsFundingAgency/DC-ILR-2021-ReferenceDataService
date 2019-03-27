﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class MetaDataRetrievalService : IMetaDataRetrievalService
    {
        private const string EmployersVersionName = "Employers Version";
        private const string LarsVersionName = "LARS Version";
        private const string OrganisationsVersionName = "Organisations Version";
        private const string PostcodesVersionName = "Potcodes Version";

        private readonly IEmployersContext _employersContext;
        private readonly ILARSContext _larsContext;
        private readonly IOrganisationsContext _organisationsContext;
        private readonly IPostcodesContext _postcodesContext;
        private readonly IValidationErrorsRepositoryService _validationErrorsRepositoryService;

        public MetaDataRetrievalService(
            IEmployersContext employersContext,
            ILARSContext larsContext,
            IOrganisationsContext organisationsContext,
            IPostcodesContext postcodesContext,
            IValidationErrorsRepositoryService validationErrorsRepositoryService)
        {
            _employersContext = employersContext;
            _larsContext = larsContext;
            _organisationsContext = organisationsContext;
            _postcodesContext = postcodesContext;
            _validationErrorsRepositoryService = validationErrorsRepositoryService;
        }

        public async Task<MetaData> RetrieveAsync(CancellationToken cancellationToken)
        {
            return new MetaData
            {
                ReferenceDataVersions = new ReferenceDataVersion
                {
                    Employers =
                        await _employersContext.LargeEmployerSourceFiles
                        .OrderByDescending(v => v.Id)
                        .Select(v => new EmployersVersion(v.Id.ToString()))
                        .FirstOrDefaultAsync(cancellationToken),
                    LarsVersion = await _larsContext.LARS_Versions
                        .OrderByDescending(v => v.MainDataSchemaName)
                        .Select(v => new LarsVersion(v.MainDataSchemaName))
                        .FirstOrDefaultAsync(cancellationToken),
                    OrganisationsVersion = await _organisationsContext.OrgVersions
                        .OrderByDescending(v => v.MainDataSchemaName)
                        .Select(v => new OrganisationsVersion(v.MainDataSchemaName))
                        .FirstOrDefaultAsync(cancellationToken),
                    PostcodesVersion = await _postcodesContext.VersionInfos
                        .OrderByDescending(v => v.VersionNumber)
                        .Select(v => new PostcodesVersion(v.VersionNumber))
                        .FirstOrDefaultAsync(cancellationToken)
                },
                ValidationErrors = await _validationErrorsRepositoryService.RetrieveAsync(cancellationToken)
            };
        }
    }
}