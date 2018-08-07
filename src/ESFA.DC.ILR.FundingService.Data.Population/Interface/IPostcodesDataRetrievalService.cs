using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.Data.Population.Interface
{
    public interface IPostcodesDataRetrievalService
    {
        IEnumerable<string> UniquePostcodes(IMessage message);

        string CurrentVersion();

        IDictionary<string, IEnumerable<SfaAreaCost>> SfaAreaCostsForPostcodes(IEnumerable<string> postcodes);

        IDictionary<string, IEnumerable<SfaDisadvantage>> SfaDisadvantagesForPostcodes(IEnumerable<string> postcodes);

        IDictionary<string, IEnumerable<EfaDisadvantage>> EfaDisadvantagesForPostcodes(IEnumerable<string> postcodes);
    }
}
