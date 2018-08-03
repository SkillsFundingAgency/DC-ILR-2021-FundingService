using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Tests
{
    public class LargeEmployerReferenceDataServiceTests
    {
        [Fact]
        public void LargeEmployers_Exists()
        {
            var ern = 123;
            var largeEmployers = new List<LargeEmployers>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LargeEmployers).Returns(new Dictionary<int, IEnumerable<LargeEmployers>>
            {
                { ern, largeEmployers },
            });

            NewService(referenceDataCacheMock.Object).LargeEmployersforEmpID(ern).Should().BeSameAs(largeEmployers);
        }

        [Fact]
        public void LargeEmployers_NotExists()
        {
            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.LargeEmployers).Returns(new Dictionary<int, IEnumerable<LargeEmployers>>
            {
                { 1, null },
            });

            NewService(referenceDataCacheMock.Object).LargeEmployersforEmpID(2).Should().BeNull();
        }

        private LargeEmployersReferenceDataService NewService(IExternalDataCache referenceDataCache = null)
        {
            return new LargeEmployersReferenceDataService(referenceDataCache);
        }
    }
}
