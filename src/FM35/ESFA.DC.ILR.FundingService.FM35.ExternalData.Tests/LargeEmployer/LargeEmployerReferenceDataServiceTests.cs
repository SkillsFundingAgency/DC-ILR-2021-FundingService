using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.ExternalData.Tests.LargeEmployer
{
    public class LargeEmployerReferenceDataServiceTests
    {
        /// <summary>
        /// Return LargeEmployers Data.
        /// </summary>
        [Fact(DisplayName = "LargeEmployers - Does exist"), Trait("LargeEmployersReferenceDataService", "Unit")]
        public void LargeEmployers_Exists()
        {
            // ARRANGE
            var largeEmployersServiceMock = LargeEmployersTestRun(LargeEmployersTestValue.ERN);

            // ACT
            var largeEmployersExists = largeEmployersServiceMock.LargeEmployersforEmpID(LargeEmployersTestValue.ERN);

            // ASSERT
            largeEmployersExists.Should().NotBeNull();
        }

        /// <summary>
        /// Return LargeEmployers Data.
        /// </summary>
        [Fact(DisplayName = "LargeEmployers - Correct"), Trait("LargeEmployersReferenceDataService", "Unit")]
        public void LargeEmployers_Correct()
        {
            // ARRANGE
            var largeEmployersServiceMock = LargeEmployersTestRun(LargeEmployersTestValue.ERN);

            // ACT
            var largeEmployersCorrect = largeEmployersServiceMock.LargeEmployersforEmpID(LargeEmployersTestValue.ERN);

            // ASSERT
            largeEmployersCorrect.First().Should().BeEquivalentTo(LargeEmployersTestValue);
        }

        private readonly Mock<IReferenceDataCache> referenceDataCacheMock = new Mock<IReferenceDataCache>();

        private ILargeEmployersReferenceDataService MockTestObject(IReferenceDataCache @object)
        {
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = new LargeEmployersReferenceDataService(@object);

            return largeEmployersReferenceDataService;
        }

        private ILargeEmployersReferenceDataService LargeEmployersTestRun(int lEmpId)
        {
            var largeEmployersMock = referenceDataCacheMock;
            largeEmployersMock.SetupGet(rdc => rdc.LargeEmployers).Returns(new Dictionary<int, IEnumerable<LargeEmployers>>
            {
                { LargeEmployersTestValue.ERN, LargeEmployersList(LargeEmployersTestValue) },
            });

            return MockTestObject(largeEmployersMock.Object);
        }

        private IList<LargeEmployers> LargeEmployersList(LargeEmployers largeEmployers)
        {
            return new List<LargeEmployers>
            {
                largeEmployers,
                new LargeEmployers
                 {
                     ERN = 108833730,
                     EffectiveFrom = new DateTime(2017, 08, 01),
                     EffectiveTo = null,
                 },
            };
        }

        private static readonly LargeEmployers LargeEmployersTestValue =
            new LargeEmployers
            {
                ERN = 107733730,
                EffectiveFrom = new DateTime(2017, 08, 01),
                EffectiveTo = null,
            };
    }
}
