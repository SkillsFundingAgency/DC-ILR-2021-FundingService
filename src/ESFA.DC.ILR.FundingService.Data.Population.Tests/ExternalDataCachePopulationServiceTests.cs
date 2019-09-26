using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests
{
    public class ExternalDataCachePopulationServiceTests
    {
        [Fact]
        public async Task Populate()
        {
            var message = new TestMessage()
            {
                LearningProviderEntity = new TestLearningProvider
                {
                    UKPRN = 1
                },
                Learners = new List<TestLearner>
                {
                    new TestLearner
                    {
                        LearnRefNumber = "TestLearnRefNumber",
                        ULN = 1234567890
                    }
                }
            };

            var referenceData = new ReferenceDataService.Model.ReferenceDataRoot
            {
                MetaDatas = new ReferenceDataService.Model.MetaData.MetaData
                {
                    ReferenceDataVersions = new ReferenceDataService.Model.MetaData.ReferenceDataVersion
                    {
                        LarsVersion = new ReferenceDataService.Model.MetaData.ReferenceDataVersions.LarsVersion { Version = "LarsVersion" },
                        OrganisationsVersion = new ReferenceDataService.Model.MetaData.ReferenceDataVersions.OrganisationsVersion { Version = "OrganisationVersion" },
                        PostcodesVersion = new ReferenceDataService.Model.MetaData.ReferenceDataVersions.PostcodesVersion { Version = "PostcodesVersion" },
                    }
                }
            };

            var postcodesMapperServiceMock = new Mock<IPostcodesMapperService>();

            var postcodesCurrentVersion = "PostcodesVersion";
            var sfaAreaCosts = new Dictionary<string, IEnumerable<SfaAreaCost>>();
            var sfaDisadvantages = new Dictionary<string, IEnumerable<SfaDisadvantage>>();
            var dasDisadvantages = new Dictionary<string, IEnumerable<DasDisadvantage>>();
            var efaDisadvantages = new Dictionary<string, IEnumerable<EfaDisadvantage>>();
            var postcodeRoots = new Dictionary<string, PostcodeRoot>();

            postcodesMapperServiceMock.Setup(p => p.MapPostcodes(It.IsAny<IReadOnlyCollection<ReferenceDataService.Model.Postcodes.Postcode>>())).Returns(postcodeRoots);

            var largeEmployersMapperServiceMock = new Mock<ILargeEmployersMapperService>();

            var largeEmployers = new Dictionary<int, IReadOnlyCollection<LargeEmployers>>();

            largeEmployersMapperServiceMock.Setup(l => l.MapLargeEmployers(It.IsAny<List<ReferenceDataService.Model.Employers.Employer>>())).Returns(largeEmployers).Verifiable();

            var larsMapperServiceMock = new Mock<ILARSMapperService>();

            var larsCurrentVersion = "LarsVersion";
            var larsLearningDeliveries = new Dictionary<string, LARSLearningDelivery>();
            var larsStandards = new Dictionary<int, LARSStandard>();

            larsMapperServiceMock.Setup(l => l.MapLARSLearningDeliveries(It.IsAny<List<ReferenceDataService.Model.LARS.LARSLearningDelivery>>())).Returns(larsLearningDeliveries).Verifiable();
            larsMapperServiceMock.Setup(l => l.MapLARSStandards(It.IsAny<List<ReferenceDataService.Model.LARS.LARSStandard>>())).Returns(larsStandards).Verifiable();

            var organisationsMapperServiceMock = new Mock<IOrganisationsMapperService>();

            var organisationCurrentVersion = "OrganisationVersion";
            var orgFundings = new Dictionary<int, IReadOnlyCollection<OrgFunding>>();

            organisationsMapperServiceMock.Setup(o => o.MapOrgFundings(
                It.IsAny<IReadOnlyCollection<ReferenceDataService.Model.Organisations.Organisation>>())).Returns(orgFundings).Verifiable();

            var appsEarningsHistoryMapperServiceMock = new Mock<IAppsEarningsHistoryMapperService>();

            var aecHistory = new Dictionary<long, IReadOnlyCollection<AECEarningsHistory>>();

            appsEarningsHistoryMapperServiceMock
                .Setup(a => a.MapAppsEarningsHistories(It.IsAny<IReadOnlyCollection<ReferenceDataService.Model.AppEarningsHistory.ApprenticeshipEarningsHistory>>()))
                .Returns(aecHistory).Verifiable();

            var fcsMapperServiceMock = new Mock<IFCSMapperService>();

            var fcsContractAllocations = new List<FCSContractAllocation>();

            fcsMapperServiceMock.Setup(f => f.MapFCSContractAllocations(It.IsAny<IReadOnlyCollection<ReferenceDataService.Model.FCS.FcsContractAllocation>>())).Returns(fcsContractAllocations).Verifiable();

            var externalDataCache = NewService(
                postcodesMapperServiceMock.Object,
                organisationsMapperServiceMock.Object,
                largeEmployersMapperServiceMock.Object,
                appsEarningsHistoryMapperServiceMock.Object,
                fcsMapperServiceMock.Object,
                larsMapperServiceMock.Object)
                .PopulateAsync(referenceData, CancellationToken.None);

            postcodesMapperServiceMock.VerifyAll();
            largeEmployersMapperServiceMock.VerifyAll();
            larsMapperServiceMock.VerifyAll();
            organisationsMapperServiceMock.VerifyAll();
            appsEarningsHistoryMapperServiceMock.VerifyAll();
            fcsMapperServiceMock.VerifyAll();

            externalDataCache.PostcodeCurrentVersion.Should().Be(postcodesCurrentVersion);
            externalDataCache.PostcodeRoots.Should().BeSameAs(postcodeRoots);

            externalDataCache.LargeEmployers.Should().BeSameAs(largeEmployers);

            externalDataCache.LARSCurrentVersion.Should().Be(larsCurrentVersion);
            externalDataCache.LARSLearningDelivery.Should().BeSameAs(larsLearningDeliveries);
            externalDataCache.LARSStandards.Should().BeSameAs(larsStandards);

            externalDataCache.OrgVersion.Should().Be(organisationCurrentVersion);
            externalDataCache.OrgFunding.Should().BeSameAs(orgFundings);
        }

        private ExternalDataCachePopulationService NewService(
            IPostcodesMapperService postcodesMapperService = null,
            IOrganisationsMapperService organisationsMapperService = null,
            ILargeEmployersMapperService largeEmployersMapperService = null,
            IAppsEarningsHistoryMapperService appsEarningsHistoryMapperService = null,
            IFCSMapperService fcsMapperService = null,
            ILARSMapperService larsMapperService = null)
        {
            return new ExternalDataCachePopulationService(
                postcodesMapperService,
                organisationsMapperService,
                largeEmployersMapperService,
                appsEarningsHistoryMapperService,
                fcsMapperService,
                larsMapperService);
        }
    }
}
