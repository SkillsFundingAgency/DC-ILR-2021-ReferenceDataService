﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.EAS2021.EF.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Configuration.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Constants;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Model;
using ESFA.DC.ILR.ReferenceDataService.Model.EAS;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Repository
{
    public class EasRepositoryService : IReferenceDataRetrievalService<int, IReadOnlyCollection<EasFundingLine>>
    {
        private readonly IDbContextFactory<IEasdbContext> _easContextFactory;

        public EasRepositoryService(IDbContextFactory<IEasdbContext> easContextFactory)
        {
            _easContextFactory = easContextFactory;
        }

        public async Task<IReadOnlyCollection<EasFundingLine>> RetrieveAsync(int ukprn, CancellationToken cancellationToken)
        {
            using (var context = _easContextFactory.Create())
            {
                var easFundingLines = await context.FundingLines?
               .Select(f => new EasFundingLine
               {
                   FundLine = f.Name,
                   EasSubmissionValues = f.PaymentTypes
                   .Select(p => new EasSubmissionValue
                   {
                       PaymentName = p.PaymentName,
                       AdjustmentTypeName = p.AdjustmentType.Name,
                   }).ToList()
               }).ToListAsync(cancellationToken);

                var easValuesList = await context.EasSubmissions?
                    .Where(u => u.Ukprn == ukprn.ToString())
                    .SelectMany(es => es.EasSubmissionValues
                    .Select(esv => new EasSubmissionDecodedValue
                    {
                        FundingLine = esv.Payment.FundingLine.Name,
                        AdjustmentName = esv.Payment.AdjustmentType.Name,
                        PaymentName = esv.Payment.PaymentName,
                        Period = esv.CollectionPeriod,
                        PaymentValue = esv.PaymentValue,
                        DevolvedAreaSof = esv.DevolvedAreaSoF
                    }))
                    .ToListAsync(cancellationToken);

                var easValuesDictionary = BuildEasDictionary(easValuesList);

                return MapEasValues(easFundingLines, easValuesDictionary);
            }
        }

        public IReadOnlyCollection<EasFundingLine> MapEasValues(List<EasFundingLine> easFundingLines, IDictionary<string, Dictionary<string, Dictionary<int, List<EasPaymentValue>>>> easValuesDictionary)
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

                    submissionValue.Period1 = paymentValues.TryGetValue(PopulationConstants.Period1, out var paymentValue1) ? paymentValue1 : null;
                    submissionValue.Period2 = paymentValues.TryGetValue(PopulationConstants.Period2, out var paymentValue2) ? paymentValue2 : null;
                    submissionValue.Period3 = paymentValues.TryGetValue(PopulationConstants.Period3, out var paymentValue3) ? paymentValue3 : null;
                    submissionValue.Period4 = paymentValues.TryGetValue(PopulationConstants.Period4, out var paymentValue4) ? paymentValue4 : null;
                    submissionValue.Period5 = paymentValues.TryGetValue(PopulationConstants.Period5, out var paymentValue5) ? paymentValue5 : null;
                    submissionValue.Period6 = paymentValues.TryGetValue(PopulationConstants.Period6, out var paymentValue6) ? paymentValue6 : null;
                    submissionValue.Period7 = paymentValues.TryGetValue(PopulationConstants.Period7, out var paymentValue7) ? paymentValue7 : null;
                    submissionValue.Period8 = paymentValues.TryGetValue(PopulationConstants.Period8, out var paymentValue8) ? paymentValue8 : null;
                    submissionValue.Period9 = paymentValues.TryGetValue(PopulationConstants.Period9, out var paymentValue9) ? paymentValue9 : null;
                    submissionValue.Period10 = paymentValues.TryGetValue(PopulationConstants.Period10, out var paymentValue10) ? paymentValue10 : null;
                    submissionValue.Period11 = paymentValues.TryGetValue(PopulationConstants.Period11, out var paymentValue11) ? paymentValue11 : null;
                    submissionValue.Period12 = paymentValues.TryGetValue(PopulationConstants.Period12, out var paymentValue12) ? paymentValue12 : null;
                }
            }

            return easFundingLines;
        }

        private IDictionary<string, Dictionary<string, Dictionary<int, List<EasPaymentValue>>>> BuildEasDictionary(List<EasSubmissionDecodedValue> easSubmissionDecodedValues)
        {
            var sofDictionary =
                easSubmissionDecodedValues
                .GroupBy(x => new { x.FundingLine, x.AdjustmentName, x.PaymentName, x.Period, x.PaymentValue })
                .ToDictionary(
                    k => k.Key,
                    v => v.Select(s => s.DevolvedAreaSof).ToList());

            return easSubmissionDecodedValues?
                .GroupBy(c => c.FundingLine, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                   fundingLine => fundingLine.Key,
                   fundingLineValues => fundingLineValues.Select(flv => flv)
                   .GroupBy(p => p.PaymentName, StringComparer.OrdinalIgnoreCase)
                   .ToDictionary(
                       paymentName => paymentName.Key,
                       paymentNameValue => paymentNameValue
                       .GroupBy(p => p.Period)
                       .ToDictionary(
                           k3 => k3.Key,
                           v3 => v3.Select(
                               eas => new EasPaymentValue(
                               eas.PaymentValue, eas.DevolvedAreaSof == -1 ? null : (int?)eas.DevolvedAreaSof)).ToList()),
                       StringComparer.OrdinalIgnoreCase),
                   StringComparer.OrdinalIgnoreCase);
        }
    }
}
