using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.FCS;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Tests
{
    public class FCSReferenceDataServiceTests
    {
        [Fact]
        public void FCSContractAllocation_Exists()
        {
            var conRef = "123";
            var contractAllocations = new List<FCSContractAllocation>();

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.FCSContractAllocations).Returns(new Dictionary<string, IEnumerable<FCSContractAllocation>>
            {
                { conRef, contractAllocations },
            });

            NewService(referenceDataCacheMock.Object).FcsContractsForConRef(conRef).Should().BeSameAs(contractAllocations);
        }

        [Fact]
        public void FCSContractAllocation_NotExists()
        {
            var conRef = "123";

            var referenceDataCacheMock = new Mock<IExternalDataCache>();

            referenceDataCacheMock.SetupGet(rdc => rdc.FCSContractAllocations).Returns(new Dictionary<string, IEnumerable<FCSContractAllocation>>
            {
                { conRef, null },
            });

            NewService(referenceDataCacheMock.Object).FcsContractsForConRef(conRef).Should().BeNull();
        }

        private FCSReferenceDataService NewService(IExternalDataCache referenceDataCache = null)
        {
            return new FCSReferenceDataService(referenceDataCache);
        }
    }
}
