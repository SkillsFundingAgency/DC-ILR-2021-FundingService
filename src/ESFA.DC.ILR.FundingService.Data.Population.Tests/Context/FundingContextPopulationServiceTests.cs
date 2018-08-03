using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.Context;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.Context;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.Model.Interface;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.Context
{
    public class FundingContextPopulationServiceTests
    {
        [Fact]
        public void Populate()
        {
            var validLearners = new List<ILearner>();

            var validLearnerDataRetrievalServiceMock = new Mock<IValidLearnersDataRetrievalService>();

            validLearnerDataRetrievalServiceMock.Setup(rds => rds.Retrieve()).Returns(validLearners).Verifiable();

            var fundingContext = new Mock<FundingContext>();

            fundingContext.SetupSet(fc => fc.ValidLearners = validLearners).Verifiable();

            NewService(fundingContext.Object, validLearnerDataRetrievalServiceMock.Object).Populate();

            fundingContext.Verify();
            validLearnerDataRetrievalServiceMock.Verify();
        }

        private FundingContextPopulationService NewService(IFundingContext fundingContext = null, IValidLearnersDataRetrievalService validLearnersDataRetrievalService = null)
        {
            return new FundingContextPopulationService(fundingContext, validLearnersDataRetrievalService);
        }
    }
}
