using System;
using System.Collections.Generic;

namespace ESFA.DC.ILR.ReferenceDataService.ReferenceInput.Model
{
    public partial class AppsEarningHistoryApprenticeshipEarningsHistory
    {
        public int Id { get; set; }
        public string AppIdentifier { get; set; }
        public bool? AppProgCompletedInTheYearInput { get; set; }
        public string CollectionYear { get; set; }
        public string CollectionReturnCode { get; set; }
        public int? DaysInYear { get; set; }
        public int? FworkCode { get; set; }
        public DateTime? HistoricEffectiveTnpstartDateInput { get; set; }
        public long? HistoricEmpIdEndWithinYear { get; set; }
        public long? HistoricEmpIdStartWithinYear { get; set; }
        public bool? HistoricLearner1618StartInput { get; set; }
        public decimal? HistoricPmramount { get; set; }
        public decimal? HistoricTnp1input { get; set; }
        public decimal? HistoricTnp2input { get; set; }
        public decimal? HistoricTnp3input { get; set; }
        public decimal? HistoricTnp4input { get; set; }
        public decimal? HistoricTotal1618UpliftPaymentsInTheYearInput { get; set; }
        public decimal? HistoricVirtualTnp3endOfTheYearInput { get; set; }
        public decimal? HistoricVirtualTnp4endOfTheYearInput { get; set; }
        public DateTime? HistoricLearnDelProgEarliestAct2dateInput { get; set; }
        public bool LatestInYear { get; set; }
        public string LearnRefNumber { get; set; }
        public DateTime? ProgrammeStartDateIgnorePathway { get; set; }
        public DateTime? ProgrammeStartDateMatchPathway { get; set; }
        public int? ProgType { get; set; }
        public int? PwayCode { get; set; }
        public int? Stdcode { get; set; }
        public decimal? TotalProgAimPaymentsInTheYear { get; set; }
        public DateTime? UptoEndDate { get; set; }
        public int Ukprn { get; set; }
        public long Uln { get; set; }
    }
}
