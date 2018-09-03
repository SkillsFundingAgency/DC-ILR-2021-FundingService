using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESFA.DC.ILR.FundingService.Stubs.Database.Model
{
    public class AEC_LatestInYearHistory
    {
        public string AppIdentifier { get; set; }

        public bool? AppProgCompletedInTheYearOutput { get; set; }

        public string CollectionYear { get; set; }

        public string CollectionReturnCode { get; set; }

        public int? DaysInYear { get; set; }

        public int? FworkCode { get; set; }

        public DateTime? HistoricEffectiveTNPStartDateOutput { get; set; }

        public long? HistoricEmpIdEndWithinYear { get; set; }

        public long? HistoricEmpIdStartWithinYear { get; set; }

        public bool? HistoricLearner1618StartOutput { get; set; }

        public decimal? HistoricPMRAmount { get; set; }

        public decimal? HistoricTNP1Output { get; set; }

        public decimal? HistoricTNP2Output { get; set; }

        public decimal? HistoricTNP3Output { get; set; }

        public decimal? HistoricTNP4Output { get; set; }

        public decimal? HistoricTotal1618 { get; set; }

        public decimal? HistoricVirtualTNP3EndOfTheYearOutput { get; set; }

        public decimal? HistoricVirtualTNP4EndOfTheYearOutput { get; set; }

        public DateTime? HistoricLearnDelProgEarliestACT2DateOutput { get; set; }

        public bool LatestInYear { get; set; }

        public string LearnRefNumber { get; set; }

        public DateTime? ProgrammeStartDateIgnorePathway { get; set; }

        public DateTime? ProgrammeStartDateMatchPathway { get; set; }

        public int? ProgType { get; set; }

        public int? PwayCode { get; set; }

        public int? STDCode { get; set; }

        public decimal? TotalProgAimPaymentsInTheYear { get; set; }

        public DateTime? UptoEndDate { get; set; }

        public int UKPRN { get; set; }

        public long ULN { get; set; }
    }
}
