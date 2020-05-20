using System;
using ESFA.DC.ILR.FundingService.Data.External.CollectionPeriod;
using ESFA.DC.ILR.FundingService.Data.External.CollectionPeriod.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Tests
{
    public class CollectionPeriodDataServiceTests
    {
        [Fact]
        public void CollectionPeriods()
        {
            var externalCachePeriods = new Periods
            {
                Period1 = new DateTime(2020, 8, 1),
                Period2 = new DateTime(2020, 9, 1),
                Period3 = new DateTime(2020, 10, 1),
                Period4 = new DateTime(2020, 11, 1),
                Period5 = new DateTime(2020, 12, 1),
                Period6 = new DateTime(2021, 1, 1),
                Period7 = new DateTime(2021, 2, 1),
                Period8 = new DateTime(2021, 3, 1),
                Period9 = new DateTime(2021, 4, 1),
                Period10 = new DateTime(2021, 5, 1),
                Period11 = new DateTime(2021, 6, 1),
                Period12 = new DateTime(2021, 7, 1),
            };

            var externalDataCahceMock = new Mock<IExternalDataCache>();

            externalDataCahceMock.Setup(m => m.Periods).Returns(externalCachePeriods);

            var expectedPeriods = new Periods
            {
                Period1 = new DateTime(2020, 8, 1),
                Period2 = new DateTime(2020, 9, 1),
                Period3 = new DateTime(2020, 10, 1),
                Period4 = new DateTime(2020, 11, 1),
                Period5 = new DateTime(2020, 12, 1),
                Period6 = new DateTime(2021, 1, 1),
                Period7 = new DateTime(2021, 2, 1),
                Period8 = new DateTime(2021, 3, 1),
                Period9 = new DateTime(2021, 4, 1),
                Period10 = new DateTime(2021, 5, 1),
                Period11 = new DateTime(2021, 6, 1),
                Period12 = new DateTime(2021, 7, 1),
            };

            NewService(externalDataCahceMock.Object).CollectionPeriods().Should().BeEquivalentTo(expectedPeriods);
        }

        private CollectionPeriodDataService NewService(IExternalDataCache referenceDataCache = null)
        {
            return new CollectionPeriodDataService(referenceDataCache);
        }
    }
}
