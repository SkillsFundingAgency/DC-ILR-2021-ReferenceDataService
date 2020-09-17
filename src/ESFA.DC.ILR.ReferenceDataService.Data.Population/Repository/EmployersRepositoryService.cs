using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Interfaces.Service.Clients;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.Employers.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class EmployersRepositoryService : IReferenceDataRetrievalService<IReadOnlyCollection<int>, IReadOnlyCollection<Employer>>
    {
        private const int BatchSize = 5000;
        private readonly IDbContextFactory<IEmployersContext> _employersContextFactory;
        private readonly IEDRSClientService _edrsClientService;
        private readonly FeatureConfiguration _featureConfiguration;
        private readonly ILogger _logger;

        public EmployersRepositoryService(
            IDbContextFactory<IEmployersContext> employersContextFactory,
            IEDRSClientService edrsClientService,
            FeatureConfiguration featureConfiguration,
            ILogger logger)
        {
            _employersContextFactory = employersContextFactory;
            _edrsClientService = edrsClientService;
            _featureConfiguration = featureConfiguration;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<Employer>> RetrieveAsync(IReadOnlyCollection<int> input, CancellationToken cancellationToken)
        {
            var edrsEmpIds = new List<int>();
            var largeEmployers = new List<ReferenceData.Employers.Model.LargeEmployer>();

            var batches = input.Batch(BatchSize);

            var stopwatch = new Stopwatch();
            if (Convert.ToBoolean(_featureConfiguration.EDRSAPIEnabled))
            {
                try
                {
                    stopwatch.Start();
                    var invalidErns = new HashSet<int>(await _edrsClientService.GetInvalidErns(input, cancellationToken));
                    var empIds = input.Except(invalidErns);
                    stopwatch.Stop();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
            }

            _logger.LogInfo("EDRS API call took: " + stopwatch.Elapsed.TotalSeconds + " secs");

            stopwatch.Reset();
            using (var context = _employersContextFactory.Create())
            {
                foreach (var batch in batches)
                {
                    stopwatch.Start();
                    edrsEmpIds.AddRange(
                        await context.Employers
                         .Where(e => batch.Contains(e.Urn))
                         .Select(e => e.Urn)
                         .ToListAsync(cancellationToken));
                    stopwatch.Stop();

                    largeEmployers.AddRange(
                        await context.LargeEmployers
                         .Where(e => batch.Contains(e.Ern))
                         .ToListAsync(cancellationToken));
                }

                _logger.LogInfo("EDRS DB batch calls took: " + stopwatch.Elapsed.TotalSeconds + " secs");

                return
                    edrsEmpIds
                    .Select(empId => new Employer
                    {
                        ERN = empId,
                        LargeEmployerEffectiveDates = largeEmployers.Where(le => le.Ern == empId)
                        .Select(le => new LargeEmployerEffectiveDates
                        {
                            EffectiveFrom = le.EffectiveFrom,
                            EffectiveTo = le.EffectiveTo,
                        }).ToList(),
                    }).ToList();
            }
        }
    }
}
