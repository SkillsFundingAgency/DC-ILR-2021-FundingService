using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.External.CollectionPeriod.Model;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.FundingService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.CollectionDates;
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
                    },
                    CollectionDates = new IlrCollectionDates
                    {
                        CensusDates = new List<CensusDate>
                        {
                            new CensusDate { Period = 1, Start = new DateTime(2020, 8, 1) },
                            new CensusDate { Period = 2, Start = new DateTime(2020, 9, 1) },
                            new CensusDate { Period = 3, Start = new DateTime(2020, 10, 1) },
                            new CensusDate { Period = 4, Start = new DateTime(2020, 11, 1) },
                            new CensusDate { Period = 5, Start = new DateTime(2020, 12, 1) },
                            new CensusDate { Period = 6, Start = new DateTime(2021, 1, 1) },
                            new CensusDate { Period = 7, Start = new DateTime(2021, 2, 1) },
                            new CensusDate { Period = 8, Start = new DateTime(2021, 3, 1) },
                            new CensusDate { Period = 9, Start = new DateTime(2021, 4, 1) },
                            new CensusDate { Period = 10, Start = new DateTime(2021, 5, 1) },
                            new CensusDate { Period = 11, Start = new DateTime(2021, 6, 1) },
                            new CensusDate { Period = 12, Start = new DateTime(2021, 7, 1) },
                        }
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

            var metaDataMapperServiceMock = new Mock<IMetaDataMapperService>();

            var periods = new Periods
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

            metaDataMapperServiceMock.Setup(mm => mm.BuildPeriods(referenceData.MetaDatas)).Returns(periods).Verifiable();
            metaDataMapperServiceMock.Setup(mm => mm.GetReferenceDataVersions(referenceData.MetaDatas)).Returns(referenceData.MetaDatas.ReferenceDataVersions).Verifiable();

            var externalDataCache = NewService(
                metaDataMapperServiceMock.Object,
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
            metaDataMapperServiceMock.VerifyAll();

            externalDataCache.PostcodeCurrentVersion.Should().Be(postcodesCurrentVersion);
            externalDataCache.PostcodeRoots.Should().BeSameAs(postcodeRoots);

            externalDataCache.LargeEmployers.Should().BeSameAs(largeEmployers);

            externalDataCache.LARSCurrentVersion.Should().Be(larsCurrentVersion);
            externalDataCache.LARSLearningDelivery.Should().BeSameAs(larsLearningDeliveries);
            externalDataCache.LARSStandards.Should().BeSameAs(larsStandards);

            externalDataCache.OrgVersion.Should().Be(organisationCurrentVersion);
            externalDataCache.OrgFunding.Should().BeSameAs(orgFundings);

            externalDataCache.Periods.Should().BeEquivalentTo(periods);
        }

        private ExternalDataCachePopulationService NewService(
            IMetaDataMapperService metaDataMapperService = null,
            IPostcodesMapperService postcodesMapperService = null,
            IOrganisationsMapperService organisationsMapperService = null,
            ILargeEmployersMapperService largeEmployersMapperService = null,
            IAppsEarningsHistoryMapperService appsEarningsHistoryMapperService = null,
            IFCSMapperService fcsMapperService = null,
            ILARSMapperService larsMapperService = null)
        {
            return new ExternalDataCachePopulationService(
                metaDataMapperService,
                postcodesMapperService,
                organisationsMapperService,
                largeEmployersMapperService,
                appsEarningsHistoryMapperService,
                fcsMapperService,
                larsMapperService);
        }
    }
}
