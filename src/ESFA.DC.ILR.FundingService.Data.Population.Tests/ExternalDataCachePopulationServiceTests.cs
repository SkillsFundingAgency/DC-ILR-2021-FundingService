using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Dto.Interfaces;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests
{
    public class ExternalDataCachePopulationServiceTests
    {
        [Fact]
        public void Populate()
        {
            var message = new TestMessage()
            {
                LearningProviderEntity = new TestLearningProvider()
                {
                    UKPRN = 1
                }
            };

            var fundingServiceDtoMock = new Mock<IFundingServiceDto>();

            fundingServiceDtoMock.SetupGet(f => f.Message).Returns(message);

            var externalDataCache = new ExternalDataCache();

            var postcodesDataRetrievalServiceMock = new Mock<IPostcodesDataRetrievalService>();

            var postcodesCurrentVersion = "PostcodesVersion";
            var sfaAreaCosts = new Dictionary<string, IEnumerable<SfaAreaCost>>();
            var sfaDisadvantages = new Dictionary<string, IEnumerable<SfaDisadvantage>>();
            var efaDisadvantages = new Dictionary<string, IEnumerable<EfaDisadvantage>>();

            postcodesDataRetrievalServiceMock.Setup(p => p.UniquePostcodes(message)).Returns(new List<string>()).Verifiable();
            postcodesDataRetrievalServiceMock.Setup(p => p.CurrentVersion()).Returns(postcodesCurrentVersion).Verifiable();
            postcodesDataRetrievalServiceMock.Setup(p => p.SfaAreaCostsForPostcodes(It.IsAny<List<string>>())).Returns(sfaAreaCosts).Verifiable();
            postcodesDataRetrievalServiceMock.Setup(p => p.SfaDisadvantagesForPostcodes(It.IsAny<List<string>>())).Returns(sfaDisadvantages).Verifiable();
            postcodesDataRetrievalServiceMock.Setup(p => p.EfaDisadvantagesForPostcodes(It.IsAny<List<string>>())).Returns(efaDisadvantages).Verifiable();

            var largeEmployersDataRetrievalServiceMock = new Mock<ILargeEmployersDataRetrievalService>();

            var largeEmployers = new Dictionary<int, IEnumerable<LargeEmployers>>();

            largeEmployersDataRetrievalServiceMock.Setup(l => l.UniqueEmployerIds(message)).Returns(new List<int>()).Verifiable();
            largeEmployersDataRetrievalServiceMock.Setup(l => l.LargeEmployersForEmployerIds(It.IsAny<List<int>>())).Returns(largeEmployers).Verifiable();

            var larsDataRetrievalServiceMock = new Mock<ILARSDataRetrievalService>();

            var larsCurrentVersion = "LarsVersion";
            var larsAnnualValues = new Dictionary<string, IEnumerable<LARSAnnualValue>>();
            var larsFrameworkAims = new Dictionary<string, IEnumerable<LARSFrameworkAims>>();
            var larsLearningDeliveryCategories = new Dictionary<string, IEnumerable<LARSLearningDeliveryCategory>>();
            var larsFundings = new Dictionary<string, IEnumerable<LARSFunding>>();
            var larsLearningDeliveries = new Dictionary<string, LARSLearningDelivery>();

            larsDataRetrievalServiceMock.Setup(l => l.UniqueLearnAimRefs(message)).Returns(new List<string>()).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.CurrentVersion()).Returns(larsCurrentVersion).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSAnnualValuesForLearnAimRefs(It.IsAny<List<string>>())).Returns(larsAnnualValues).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSFrameworkAimsForLearnAimRefs(It.IsAny<List<string>>())).Returns(larsFrameworkAims).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSLearningDeliveryCategoriesForLearnAimRefs(It.IsAny<List<string>>())).Returns(larsLearningDeliveryCategories).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSFundingsForLearnAimRefs(It.IsAny<List<string>>())).Returns(larsFundings).Verifiable();
            larsDataRetrievalServiceMock.Setup(l => l.LARSLearningDeliveriesForLearnAimRefs(It.IsAny<List<string>>())).Returns(larsLearningDeliveries).Verifiable();

            var organisationDataRetrievalServiceMock = new Mock<IOrganisationDataRetrievalService>();

            var organisationCurrentVersion = "OrganisationVersion";
            var orgFundings = new Dictionary<long, IEnumerable<OrgFunding>>();

            organisationDataRetrievalServiceMock.Setup(o => o.CurrentVersion()).Returns(organisationCurrentVersion).Verifiable();
            organisationDataRetrievalServiceMock.Setup(o => o.OrgFundingsForUkprns(It.IsAny<List<long>>())).Returns(orgFundings).Verifiable();

            NewService(
                externalDataCache,
                postcodesDataRetrievalServiceMock.Object,
                largeEmployersDataRetrievalServiceMock.Object,
                larsDataRetrievalServiceMock.Object,
                organisationDataRetrievalServiceMock.Object,
                fundingServiceDtoMock.Object)
                .Populate();

            postcodesDataRetrievalServiceMock.VerifyAll();
            largeEmployersDataRetrievalServiceMock.VerifyAll();
            larsDataRetrievalServiceMock.VerifyAll();
            organisationDataRetrievalServiceMock.VerifyAll();

            externalDataCache.PostcodeCurrentVersion.Should().Be(postcodesCurrentVersion);
            externalDataCache.SfaAreaCost.Should().BeSameAs(sfaAreaCosts);
            externalDataCache.SfaDisadvantage.Should().BeSameAs(sfaDisadvantages);
            externalDataCache.EfaDisadvantage.Should().BeSameAs(efaDisadvantages);

            externalDataCache.LargeEmployers.Should().BeSameAs(largeEmployers);

            externalDataCache.LARSCurrentVersion.Should().Be(larsCurrentVersion);
            externalDataCache.LARSAnnualValue.Should().BeSameAs(larsAnnualValues);
            externalDataCache.LARSFrameworkAims.Should().BeSameAs(larsFrameworkAims);
            externalDataCache.LARSFunding.Should().BeSameAs(larsFundings);
            externalDataCache.LARSLearningDelivery.Should().BeSameAs(larsLearningDeliveries);
            externalDataCache.LARSLearningDeliveryCategory.Should().BeSameAs(larsLearningDeliveryCategories);

            externalDataCache.OrgVersion.Should().Be(organisationCurrentVersion);
            externalDataCache.OrgFunding.Should().BeSameAs(orgFundings);
        }

        private ExternalDataCachePopulationService NewService(
            IExternalDataCache externalDataCache = null,
            IPostcodesDataRetrievalService postcodesDataRetrievalService = null,
            ILargeEmployersDataRetrievalService largeEmployersDataRetrievalService = null,
            ILARSDataRetrievalService larsDataRetrievalService = null,
            IOrganisationDataRetrievalService organisationDataRetrievalService = null,
            IFundingServiceDto fundingServiceDto = null)
        {
            return new ExternalDataCachePopulationService(externalDataCache, postcodesDataRetrievalService, largeEmployersDataRetrievalService, larsDataRetrievalService, organisationDataRetrievalService, fundingServiceDto);
        }
    }
}
