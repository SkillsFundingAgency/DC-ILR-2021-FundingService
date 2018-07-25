using System;
using System.Collections.Generic;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Organisatons.Model;
using ESFA.DC.Data.Organisatons.Model.Interface;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.FundingService.Tests.Common;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.ALB.ExternalData.Tests
{
    public class ReferenceDataCachePopulationServiceTests
    {
        /// <summary>
        /// Return LARS Version Data from LARS database
        /// </summary>
        [Fact(DisplayName = "MockDB - LARS Version Data - Value Correct"), Trait("RefDataCachePopulation", "Unit")]
        public void MockDB_LARSVersionData_ValueCorrect()
        {
            // ARRANGE
            // Use Test Helpers

            // ACT
            var output = MockDBOutput();

            // ASSERT
            output.LARSCurrentVersion.Should().Be("Version_005");
        }

        #region Test Helpers

        private IExternalDataCache MockDBOutput()
        {
            var larsMock = new Mock<ILARS>();
            var organisationsMock = new Mock<IOrganisations>();

            IExternalDataCache referenceDataCache = new ExternalDataCache();

            larsMock.Setup(x => x.LARS_Version).Returns(MockLARSVersionArray().AsMockDbSet());
            larsMock.Setup(x => x.LARS_AnnualValue).Returns(new List<LARS_AnnualValue>().AsMockDbSet);
            larsMock.Setup(x => x.LARS_LearningDeliveryCategory).Returns(new List<LARS_LearningDeliveryCategory>().AsMockDbSet);
            larsMock.Setup(x => x.LARS_FrameworkAims).Returns(new List<LARS_FrameworkAims>().AsMockDbSet());
            
            organisationsMock.Setup(x => x.Org_Version).Returns(new List<Org_Version>() { new Org_Version() { MainDataSchemaName = "test" }}.AsMockDbSet());
            organisationsMock.Setup(x => x.Org_Funding).Returns(new List<Org_Funding>().AsMockDbSet());


            var fundingServiceDtoMock = new Mock<IFundingServiceDto>();

            fundingServiceDtoMock
                .SetupGet(m => m.Message)
                .Returns(
                    new TestMessage()
                    {
                        LearningProviderEntity = new TestLearningProvider()
                        {
                            UKPRN = 1
                        },
                        Learners = new List<TestLearner>()
                    });

            var postcodesDataRetrievalServiceMock = new Mock<IPostcodesDataRetrievalService>();
            var largeEmployersDataRetrievalServiceMock = new Mock<ILargeEmployersDataRetrievalService>();
            var larsDataRetrievalServiceMock = new Mock<ILARSDataRetrievalService>();

            var service = NewService(referenceDataCache, postcodesDataRetrievalServiceMock.Object, largeEmployersDataRetrievalServiceMock.Object, larsDataRetrievalServiceMock.Object, larsMock.Object, organisationsMock.Object, fundingServiceDtoMock.Object);
            service.Populate();

            return referenceDataCache;
        }

        #region Test Data

        private static LARS_Version[] MockLARSVersionArray()
        {
            return new LARS_Version[]
            {
                LarsVersionTestValue,
            };
        }

        private static readonly LARS_Version LarsVersionTestValue =
            new LARS_Version()
            {
                MajorNumber = 5,
                MinorNumber = 0,
                MaintenanceNumber = 0,
                MainDataSchemaName = "Version_005",
                RefDataSchemaName = "REF_Version_005",
                ActivationDate = DateTime.Parse("2017-07-01"),
                ExpiryDate = null,
                Description = "Fifth Version of LARS",
                Comment = null,
                Created_On = DateTime.Parse("2017-07-01"),
                Created_By = "System",
                Modified_On = DateTime.Parse("2018-07-01"),
                Modified_By = "System"
            };

        #endregion

        #endregion

        private ExternalDataCachePopulationService NewService(
            IExternalDataCache referenceDataCache = null,
            IPostcodesDataRetrievalService postcodesDataRetrievalService = null,
            ILargeEmployersDataRetrievalService largeEmployersDataRetrievalService = null,
            ILARSDataRetrievalService larsDataRetrievalService = null,
            ILARS lars = null,
            IOrganisations organisations = null,
            IFundingServiceDto fundingServiceDto = null)
        {
            return new ExternalDataCachePopulationService(referenceDataCache, postcodesDataRetrievalService, largeEmployersDataRetrievalService, larsDataRetrievalService, lars, organisations, fundingServiceDto);
        }
    }
}
