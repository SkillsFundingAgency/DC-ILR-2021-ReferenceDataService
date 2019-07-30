using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.EAS1920.EF.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Constants;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.EAS;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class EasRepositoryService : IReferenceDataRetrievalService<int, IReadOnlyCollection<EASFundingLine>>
    {
        private readonly decimal? defaultDecimalValue = null;
        private readonly IEasdbContext _easContext;

        public EasRepositoryService(IEasdbContext easContext)
        {
            _easContext = easContext;
        }

        public async Task<IReadOnlyCollection<EASFundingLine>> RetrieveAsync(int ukprn, CancellationToken cancellationToken)
        {
            var easFundingLines = await _easContext.FundingLines?
               .Select(f => new EASFundingLine
               {
                   FundLine = f.Name,
                   EasSubmissionValues = f.PaymentTypes
                   .Select(p => new EasSubmissionValue
                   {
                       PaymentName = p.PaymentName,
                       AdjustmentTypeName = p.AdjustmentType.Name,
                   }).ToList()
               }).ToListAsync(cancellationToken);

            var easValuesDictionary = await _easContext.EasSubmissions?
                .Where(u => u.Ukprn == ukprn.ToString())
                .SelectMany(es => es.EasSubmissionValues
                .Select(esv => new EasSubmissionDecodedValue
                {
                    FundingLine = esv.Payment.FundingLine.Name,
                    AdjustmentName = esv.Payment.AdjustmentType.Name,
                    PaymentName = esv.Payment.PaymentName,
                    Period = esv.CollectionPeriod,
                    PaymentValue = esv.PaymentValue
                }))
                .GroupBy(c => c.FundingLine, StringComparer.OrdinalIgnoreCase)
                .ToDictionaryAsync(
                   fundingLine => fundingLine.Key,
                   fundingLineValues => fundingLineValues.Select(flv => flv)
                   .GroupBy(p => p.PaymentName, StringComparer.OrdinalIgnoreCase)
                   .ToDictionary(
                       paymentName => paymentName.Key,
                       paymentNameValue => paymentNameValue
                       .ToDictionary(
                           k3 => k3.Period,
                           v3 => v3.PaymentValue), StringComparer.OrdinalIgnoreCase),
                   StringComparer.OrdinalIgnoreCase,
                   cancellationToken);

            return MapEasValues(easFundingLines, easValuesDictionary);
        }

        public IReadOnlyCollection<EASFundingLine> MapEasValues(List<EASFundingLine> easFundingLines, Dictionary<string, Dictionary<string, Dictionary<int, decimal?>>> easValuesDictionary)
        {
            foreach (var fundline in easFundingLines)
            {
                easValuesDictionary.TryGetValue(fundline.FundLine, out var easPayment);

                if (easPayment == null)
                {
                    continue;
                }

                foreach (var submissionValue in fundline.EasSubmissionValues)
                {
                    easPayment.TryGetValue(submissionValue.PaymentName, out var paymentValues);

                    if (paymentValues == null)
                    {
                        continue;
                    }

                    submissionValue.Period1 = paymentValues.TryGetValue(PopulationConstants.Period1, out var paymentValue1) ? paymentValue1 : defaultDecimalValue;
                    submissionValue.Period2 = paymentValues.TryGetValue(PopulationConstants.Period2, out var paymentValue2) ? paymentValue2 : defaultDecimalValue;
                    submissionValue.Period3 = paymentValues.TryGetValue(PopulationConstants.Period3, out var paymentValue3) ? paymentValue3 : defaultDecimalValue;
                    submissionValue.Period4 = paymentValues.TryGetValue(PopulationConstants.Period4, out var paymentValue4) ? paymentValue4 : defaultDecimalValue;
                    submissionValue.Period5 = paymentValues.TryGetValue(PopulationConstants.Period5, out var paymentValue5) ? paymentValue5 : defaultDecimalValue;
                    submissionValue.Period6 = paymentValues.TryGetValue(PopulationConstants.Period6, out var paymentValue6) ? paymentValue6 : defaultDecimalValue;
                    submissionValue.Period7 = paymentValues.TryGetValue(PopulationConstants.Period7, out var paymentValue7) ? paymentValue7 : defaultDecimalValue;
                    submissionValue.Period8 = paymentValues.TryGetValue(PopulationConstants.Period8, out var paymentValue8) ? paymentValue8 : defaultDecimalValue;
                    submissionValue.Period9 = paymentValues.TryGetValue(PopulationConstants.Period9, out var paymentValue9) ? paymentValue9 : defaultDecimalValue;
                    submissionValue.Period10 = paymentValues.TryGetValue(PopulationConstants.Period10, out var paymentValue10) ? paymentValue10 : defaultDecimalValue;
                    submissionValue.Period11 = paymentValues.TryGetValue(PopulationConstants.Period11, out var paymentValue11) ? paymentValue11 : defaultDecimalValue;
                    submissionValue.Period12 = paymentValues.TryGetValue(PopulationConstants.Period12, out var paymentValue12) ? paymentValue12 : defaultDecimalValue;
                }
            }

            return easFundingLines;
        }
    }
}
