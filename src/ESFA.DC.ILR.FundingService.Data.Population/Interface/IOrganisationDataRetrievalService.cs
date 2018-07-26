using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IOrganisationDataRetrievalService
    {
        string CurrentVersion();

        IDictionary<long, IEnumerable<OrgFunding>> OrgFundingsForUkprns(IEnumerable<long> ukprns);
    }
}
