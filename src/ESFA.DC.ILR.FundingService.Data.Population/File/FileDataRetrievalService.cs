using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.File.Model;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.File
{
    public class FileDataRetrievalService : IFileDataRetrievalService
    {
        public IDictionary<string, IEnumerable<DPOutcome>> RetrieveDPOutcomes(IMessage message)
        {
            return message?
                .LearnerDestinationAndProgressions?
                .ToDictionary(
                    l => l.LearnRefNumber,
                    l => l.DPOutcomes?
                        .Select(d => new DPOutcome()
                        {
                            OutCode = d.OutCode,
                            OutType = d.OutType,
                            OutCollDate = d.OutCollDate,
                            OutStartDate = d.OutStartDate,
                            OutEndDate = d.OutEndDateNullable
                        }).ToList() ?? new List<DPOutcome>() as IEnumerable<DPOutcome>)
                ?? new Dictionary<string, IEnumerable<DPOutcome>>();
        }

        public int RetrieveUKPRN(IMessage message)
        {
            return message.LearningProviderEntity.UKPRN;
        }
    }
}
