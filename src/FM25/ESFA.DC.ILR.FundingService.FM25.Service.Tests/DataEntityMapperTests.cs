using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM25.Service.Input;
using ESFA.DC.ILR.FundingService.FM25.Service.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Tests
{
    public class DataEntityMapperTests
    {
        [Fact]
        public void MapTo()
        {
            var ukprn = 1;
            var larsVersion = "1.0.0";
            var orgVersion = "1.0.0";
            var postcodeVersion = "1.0.0";
            var learnAimRef = "LearnAimRef";
            var postcode = "Postcode";
            var uplift = 1.2m;

            var global = new Global
            {
                LARSVersion = larsVersion,
                UKPRN = ukprn,
                OrgVersion = orgVersion,
                PostcodesVersion = postcodeVersion,
                AreaCostFactor1618 = "HISTORIC AREA COST FACTOR VALUE",
                DisadvantageProportion = "HISTORIC DISADVANTAGE FUNDING PROPORTION VALUE",
                HistoricLargeProgrammeProportion = "HISTORIC LARGE PROGRAMME PROPORTION VALUE",
                ProgrammeWeighting = "HISTORIC PROGRAMME COST WEIGHTING FACTOR VALUE",
                RetentionFactor = "HISTORIC RETENTION FACTOR VALUE",
                Level3ProgMathsAndEnglishProportion = "HISTORIC LEVEL 3 PROGRAMME MATHS AND ENGLISH PROPORTION",
                SpecialistResources = true
            };

            var learnerDtos = new List<FM25LearnerDto>
            {
                new FM25LearnerDto
                {
                    Postcode = postcode,
                    LearningDeliveries = new List<LearningDelivery>
                    {
                        new LearningDelivery
                        {
                            LearnAimRef = learnAimRef,
                            FundModel = 25,
                        }
                    }
                },
                new FM25LearnerDto
                {
                    Postcode = postcode,
                    LearningDeliveries = new List<LearningDelivery>
                    {
                        new LearningDelivery
                        {
                            LearnAimRef = learnAimRef,
                            FundModel = 25,
                        }
                    }
                },
            };

            var larsLearningDelivery = new LARSLearningDelivery
            {
                AwardOrgCode = "awardOrgCode",
                EFACOFType = 1,
                LearnAimRefTitle = "learnAimRefTitle",
                LearnAimRefType = "learnAimRefType",
                RegulatedCreditValue = 2,
                NotionalNVQLevelv2 = "NVQLevel",
                LARSFundings = new List<LARSFunding>
                {
                    new LARSFunding
                    {
                        FundingCategory = "Matrix",
                        RateWeighted = 1.0m,
                        WeightingFactor = "G",
                        EffectiveFrom = new DateTime(2019, 1, 1),
                        EffectiveTo = new DateTime(2020, 1, 1),
                    }
                }
            };

            var orgFundings = TestOrgFundingDataset();

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var orgReferenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsVersion);
            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learnAimRef)).Returns(larsLearningDelivery);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationVersion()).Returns(orgVersion);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationFundingForUKPRN(ukprn)).Returns(orgFundings);
            postcodesReferenceDataServiceMock.Setup(p => p.PostcodesCurrentVersion()).Returns(postcodeVersion);
            postcodesReferenceDataServiceMock.Setup(p => p.LatestEFADisadvantagesUpliftForPostcode(postcode)).Returns(uplift);

            var dataEntities = NewService(
                larsReferenceDataService: larsReferenceDataServiceMock.Object,
                organisationReferenceDataService: orgReferenceDataServiceMock.Object,
                postcodesReferenceDataService: postcodesReferenceDataServiceMock.Object).MapTo(ukprn, learnerDtos);

            dataEntities.Should().HaveCount(2);
            dataEntities.SelectMany(d => d.Children).Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void MapTo_NullLearnerDto()
        {
            var ukprn = 1;
            var larsVersion = "1.0.0";
            var orgVersion = "1.0.0";
            var postcodeVersion = "1.0.0";

            var global = new Global
            {
                LARSVersion = larsVersion,
                UKPRN = ukprn,
                OrgVersion = orgVersion,
                PostcodesVersion = postcodeVersion,
                AreaCostFactor1618 = "HISTORIC AREA COST FACTOR VALUE",
                DisadvantageProportion = "HISTORIC DISADVANTAGE FUNDING PROPORTION VALUE",
                HistoricLargeProgrammeProportion = "HISTORIC LARGE PROGRAMME PROPORTION VALUE",
                ProgrammeWeighting = "HISTORIC PROGRAMME COST WEIGHTING FACTOR VALUE",
                RetentionFactor = "HISTORIC RETENTION FACTOR VALUE",
                Level3ProgMathsAndEnglishProportion = "HISTORIC LEVEL 3 PROGRAMME MATHS AND ENGLISH PROPORTION",
                SpecialistResources = true
            };

            var orgFundings = TestOrgFundingDataset();

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var orgReferenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsVersion);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationVersion()).Returns(orgVersion);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationFundingForUKPRN(ukprn)).Returns(orgFundings);
            postcodesReferenceDataServiceMock.Setup(p => p.PostcodesCurrentVersion()).Returns(postcodeVersion);

            var dataEntities = NewService(
                larsReferenceDataService: larsReferenceDataServiceMock.Object,
                organisationReferenceDataService: orgReferenceDataServiceMock.Object,
                postcodesReferenceDataService: postcodesReferenceDataServiceMock.Object).MapTo(ukprn, null);

            dataEntities.Should().HaveCount(1);
            dataEntities.Select(d => d.IsGlobal).First().Should().Be(true);
            dataEntities.Select(d => d.Children).First().Should().HaveCount(0);
            dataEntities.Select(d => d.EntityName).First().Should().Be("global");
            dataEntities.Select(d => d.Attributes).First().Should().HaveCount(11);
            dataEntities.Select(d => d.Attributes).First()["OrgVersion"].Value.Should().Be(global.OrgVersion);
            dataEntities.Select(d => d.Attributes).First()["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntities.Select(d => d.Attributes).First()["PostcodeDisadvantageVersion"].Value.Should().Be(global.PostcodesVersion);
            dataEntities.Select(d => d.Attributes).First()["UKPRN"].Value.Should().Be(global.UKPRN);
            dataEntities.Select(d => d.Attributes).First()["AreaCostFactor1618"].Value.Should().Be(global.AreaCostFactor1618);
            dataEntities.Select(d => d.Attributes).First()["DisadvantageProportion"].Value.Should().Be(global.DisadvantageProportion);
            dataEntities.Select(d => d.Attributes).First()["HistoricLargeProgrammeProportion"].Value.Should().Be(global.HistoricLargeProgrammeProportion);
            dataEntities.Select(d => d.Attributes).First()["ProgrammeWeighting"].Value.Should().Be(global.ProgrammeWeighting);
            dataEntities.Select(d => d.Attributes).First()["RetentionFactor"].Value.Should().Be(global.RetentionFactor);
            dataEntities.Select(d => d.Attributes).First()["Level3ProgMathsandEnglishProportion"].Value.Should().Be(global.Level3ProgMathsAndEnglishProportion);
            dataEntities.Select(d => d.Attributes).First()["SpecialistResources"].Value.Should().Be(global.SpecialistResources);
        }

        [Fact]
        public void MapTo_EmptyLearners()
        {
            var ukprn = 1;
            var larsVersion = "1.0.0";
            var orgVersion = "1.0.0";
            var postcodeVersion = "1.0.0";

            var global = new Global
            {
                LARSVersion = larsVersion,
                UKPRN = ukprn,
                OrgVersion = orgVersion,
                PostcodesVersion = postcodeVersion,
                AreaCostFactor1618 = "HISTORIC AREA COST FACTOR VALUE",
                DisadvantageProportion = "HISTORIC DISADVANTAGE FUNDING PROPORTION VALUE",
                HistoricLargeProgrammeProportion = "HISTORIC LARGE PROGRAMME PROPORTION VALUE",
                ProgrammeWeighting = "HISTORIC PROGRAMME COST WEIGHTING FACTOR VALUE",
                RetentionFactor = "HISTORIC RETENTION FACTOR VALUE",
                Level3ProgMathsAndEnglishProportion = "HISTORIC LEVEL 3 PROGRAMME MATHS AND ENGLISH PROPORTION",
                SpecialistResources = true
            };

            var orgFundings = TestOrgFundingDataset();

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var orgReferenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsVersion);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationVersion()).Returns(orgVersion);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationFundingForUKPRN(ukprn)).Returns(orgFundings);
            postcodesReferenceDataServiceMock.Setup(p => p.PostcodesCurrentVersion()).Returns(postcodeVersion);

            var dataEntities = NewService(
                larsReferenceDataService: larsReferenceDataServiceMock.Object,
                organisationReferenceDataService: orgReferenceDataServiceMock.Object,
                postcodesReferenceDataService: postcodesReferenceDataServiceMock.Object).MapTo(ukprn, new List<FM25LearnerDto>());

            dataEntities.Should().HaveCount(1);
            dataEntities.Select(d => d.IsGlobal).First().Should().Be(true);
            dataEntities.Select(d => d.Children).First().Should().HaveCount(0);
            dataEntities.Select(d => d.EntityName).First().Should().Be("global");
            dataEntities.Select(d => d.Attributes).First().Should().HaveCount(11);
            dataEntities.Select(d => d.Attributes).First()["OrgVersion"].Value.Should().Be(global.OrgVersion);
            dataEntities.Select(d => d.Attributes).First()["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntities.Select(d => d.Attributes).First()["PostcodeDisadvantageVersion"].Value.Should().Be(global.PostcodesVersion);
            dataEntities.Select(d => d.Attributes).First()["UKPRN"].Value.Should().Be(global.UKPRN);
            dataEntities.Select(d => d.Attributes).First()["AreaCostFactor1618"].Value.Should().Be(global.AreaCostFactor1618);
            dataEntities.Select(d => d.Attributes).First()["DisadvantageProportion"].Value.Should().Be(global.DisadvantageProportion);
            dataEntities.Select(d => d.Attributes).First()["HistoricLargeProgrammeProportion"].Value.Should().Be(global.HistoricLargeProgrammeProportion);
            dataEntities.Select(d => d.Attributes).First()["ProgrammeWeighting"].Value.Should().Be(global.ProgrammeWeighting);
            dataEntities.Select(d => d.Attributes).First()["RetentionFactor"].Value.Should().Be(global.RetentionFactor);
            dataEntities.Select(d => d.Attributes).First()["Level3ProgMathsandEnglishProportion"].Value.Should().Be(global.Level3ProgMathsAndEnglishProportion);
            dataEntities.Select(d => d.Attributes).First()["SpecialistResources"].Value.Should().Be(global.SpecialistResources);
        }

        [Fact]
        public void BuildDPOutcome()
        {
            var dpOutcome = new DPOutcome()
            {
                OutCode = 1,
                OutType = "Type",
            };

            var dataEntity = NewService().BuildDPOutcome(dpOutcome);

            dataEntity.EntityName.Should().Be("DPOutcome");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["OutCode"].Value.Should().Be(dpOutcome.OutCode);
            dataEntity.Attributes["OutType"].Value.Should().Be(dpOutcome.OutType);
        }

        [Fact]
        public void BuildLearningDeliveryLARSValidity()
        {
            var larsValidity = new LARSValidity()
            {
                Category = "Category",
                LastNewStartDate = new DateTime(2017, 1, 1),
                StartDate = new DateTime(2019, 1, 1)
            };

            var dataEntity = NewService().BuildLearningDeliveryLARSValidity(larsValidity);

            dataEntity.EntityName.Should().Be("LearningDeliveryLARSValidity");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["ValidityCategory"].Value.Should().Be(larsValidity.Category);
            dataEntity.Attributes["ValidityLastNewStartDate"].Value.Should().Be(larsValidity.LastNewStartDate);
            dataEntity.Attributes["ValidityStartDate"].Value.Should().Be(larsValidity.StartDate);
        }

        [Fact]
        public void BuildLearner()
        {
            var learner = new FM25LearnerDto()
            {
                DateOfBirth = new DateTime(1990, 12, 25),
                EngGrade = "A",
                LearnRefNumber = "ABC",
                MathGrade = "B",
                PlanEEPHours = 2,
                PlanLearnHours = 3,
                Postcode = "postcode",
                ULN = 123456,
                LrnFAM_ECF = 1,
                LrnFAM_EDF1 = 2,
                LrnFAM_EDF2 = 3,
                LrnFAM_EHC = 4,
                LrnFAM_HNS = 5,
                LrnFAM_MCF = 6,
            };

            var uplift = 1.1m;

            var efaDisadvantageOne = new EfaDisadvantage
            {
                Postcode = "CV1 2WT",
                Uplift = 1.0m,
                EffectiveFrom = new DateTime(2000, 01, 01),
                EffectiveTo = new DateTime(2015, 07, 31),
            };

            var efaDisadvatageTwo = new EfaDisadvantage
            {
                Postcode = "CV1 2WT",
                Uplift = uplift,
                EffectiveFrom = new DateTime(2015, 08, 01),
                EffectiveTo = new DateTime(2020, 07, 31),
            };

            var efaDisadvantages = new List<EfaDisadvantage>()
            {
               efaDisadvantageOne,
               efaDisadvatageTwo
            };

            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var organisationsDataServiceMock = new Mock<IOrganisationReferenceDataService>();

            postcodesReferenceDataServiceMock.Setup(p => p.LatestEFADisadvantagesUpliftForPostcode(learner.Postcode)).Returns(efaDisadvatageTwo.Uplift);
            organisationsDataServiceMock.Setup(l => l.SpecialistResourcesForCampusIdentifier(It.IsAny<string>())).Returns(new List<CampusIdentifierSpecResource>());

            var dataEntity = NewService(
                organisationReferenceDataService: organisationsDataServiceMock.Object,
                postcodesReferenceDataService: postcodesReferenceDataServiceMock.Object)
                .BuildLearnerDataEntity(learner);

            dataEntity.EntityName.Should().Be("Learner");
            dataEntity.Attributes.Should().HaveCount(14);
            dataEntity.Attributes["DateOfBirth"].Value.Should().Be(learner.DateOfBirth);
            dataEntity.Attributes["EngGrade"].Value.Should().Be(learner.EngGrade);
            dataEntity.Attributes["LearnRefNumber"].Value.Should().Be(learner.LearnRefNumber);
            dataEntity.Attributes["LrnFAM_ECF"].Value.Should().Be(1);
            dataEntity.Attributes["LrnFAM_EDF1"].Value.Should().Be(2);
            dataEntity.Attributes["LrnFAM_EDF2"].Value.Should().Be(3);
            dataEntity.Attributes["LrnFAM_EHC"].Value.Should().Be(4);
            dataEntity.Attributes["LrnFAM_HNS"].Value.Should().Be(5);
            dataEntity.Attributes["LrnFAM_MCF"].Value.Should().Be(6);
            dataEntity.Attributes["MathGrade"].Value.Should().Be(learner.MathGrade);
            dataEntity.Attributes["PlanEEPHours"].Value.Should().Be(learner.PlanEEPHours);
            dataEntity.Attributes["PlanLearnHours"].Value.Should().Be(learner.PlanLearnHours);
            dataEntity.Attributes["PostcodeDisadvantageUplift"].Value.Should().Be(uplift);
            dataEntity.Attributes["ULN"].Value.Should().Be(learner.ULN);

            dataEntity.Children.Should().BeEmpty();
        }

        [Fact]
        public void BuildLearner_NoPostcodeUplifts()
        {
            var learner = new FM25LearnerDto()
            {
                Postcode = "postcode",
            };

            decimal? efaDisadvantagesUplift = null;

            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var organisationsDataServiceMock = new Mock<IOrganisationReferenceDataService>();

            postcodesReferenceDataServiceMock.Setup(p => p.LatestEFADisadvantagesUpliftForPostcode(learner.Postcode)).Returns(efaDisadvantagesUplift);
            organisationsDataServiceMock.Setup(l => l.SpecialistResourcesForCampusIdentifier(It.IsAny<string>())).Returns(new List<CampusIdentifierSpecResource>());

            var dataEntity = NewService(
                organisationReferenceDataService: organisationsDataServiceMock.Object,
                postcodesReferenceDataService: postcodesReferenceDataServiceMock.Object)
                .BuildLearnerDataEntity(learner);

            dataEntity.Attributes["PostcodeDisadvantageUplift"].Value.Should().BeNull();
        }

        [Fact]
        public void BuildGlobalDataEntity()
        {
            var global = new Global()
            {
                AreaCostFactor1618 = "AreaCostFactor1618",
                DisadvantageProportion = "DisadvantageProportion",
                HistoricLargeProgrammeProportion = "HistoricLargeProgrammeProportion",
                LARSVersion = "LARSVersion",
                OrgVersion = "OrgVersion",
                PostcodesVersion = "PostcodesVersion",
                ProgrammeWeighting = "ProgrammeWeighting",
                RetentionFactor = "RetentionFactor",
                Level3ProgMathsAndEnglishProportion = "HISTORIC LEVEL 3 PROGRAMME MATHS AND ENGLISH PROPORTION",
                SpecialistResources = true,
                UKPRN = 1234
            };

            var dataEntity = NewService().BuildGlobalDataEntity(null, global);

            dataEntity.EntityName.Should().Be("global");
            dataEntity.Attributes.Should().HaveCount(11);
            dataEntity.Attributes["AreaCostFactor1618"].Value.Should().Be(global.AreaCostFactor1618);
            dataEntity.Attributes["DisadvantageProportion"].Value.Should().Be(global.DisadvantageProportion);
            dataEntity.Attributes["HistoricLargeProgrammeProportion"].Value.Should().Be(global.HistoricLargeProgrammeProportion);
            dataEntity.Attributes["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntity.Attributes["OrgVersion"].Value.Should().Be(global.OrgVersion);
            dataEntity.Attributes["PostcodeDisadvantageVersion"].Value.Should().Be(global.PostcodesVersion);
            dataEntity.Attributes["ProgrammeWeighting"].Value.Should().Be(global.ProgrammeWeighting);
            dataEntity.Attributes["RetentionFactor"].Value.Should().Be(global.RetentionFactor);
            dataEntity.Attributes["Level3ProgMathsandEnglishProportion"].Value.Should().Be(global.Level3ProgMathsAndEnglishProportion);
            dataEntity.Attributes["SpecialistResources"].Value.Should().Be(global.SpecialistResources);
            dataEntity.Attributes["UKPRN"].Value.Should().Be(global.UKPRN);

            dataEntity.Children.Should().HaveCount(0);
        }

        [Fact]
        public void BuildGlobal()
        {
            var larsCurrentVersion = "1.0.0";
            var orgCurrentVersion = "2.0.0";
            var postcodesCurrentVersion = "3.0.0";
            var ukprn = 1234;
            var effectiveFrom = new DateTime(2020, 8, 1);
            var fundFactorType = "EFA 16-19";

            var historicAreaCostFactorValue = "HISTORIC AREA COST FACTOR VALUE";
            var historicDisadvantageFundingProportionValue = "HISTORIC DISADVANTAGE FUNDING PROPORTION VALUE";
            var historicLargeProgrammeProportionValue = "HISTORIC LARGE PROGRAMME PROPORTION VALUE";
            var historicProgrammeCostWeightingFactorValue = "HISTORIC PROGRAMME COST WEIGHTING FACTOR VALUE";
            var historicRetentionFactorValue = "HISTORIC RETENTION FACTOR VALUE";
            var historicLevel3ProgMathsAndEnglishProportion = "HISTORIC LEVEL 3 PROGRAMME MATHS AND ENGLISH PROPORTION";
            var specialistResources = true;

            var larsRefererenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var orgReferenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var orgFundings = new List<OrgFunding>()
            {
                new OrgFunding() { OrgFundFactType = fundFactorType, OrgFundEffectiveFrom = effectiveFrom, OrgFundFactor = "HISTORIC AREA COST FACTOR", OrgFundFactValue = historicAreaCostFactorValue },
                new OrgFunding() { OrgFundFactType = fundFactorType, OrgFundEffectiveFrom = effectiveFrom, OrgFundFactor = "HISTORIC DISADVANTAGE FUNDING PROPORTION", OrgFundFactValue = historicDisadvantageFundingProportionValue },
                new OrgFunding() { OrgFundFactType = fundFactorType, OrgFundEffectiveFrom = effectiveFrom, OrgFundFactor = "HISTORIC LARGE PROGRAMME PROPORTION", OrgFundFactValue = historicLargeProgrammeProportionValue },
                new OrgFunding() { OrgFundFactType = fundFactorType, OrgFundEffectiveFrom = effectiveFrom, OrgFundFactor = "HISTORIC PROGRAMME COST WEIGHTING FACTOR", OrgFundFactValue = historicProgrammeCostWeightingFactorValue },
                new OrgFunding() { OrgFundFactType = fundFactorType, OrgFundEffectiveFrom = effectiveFrom, OrgFundFactor = "HISTORIC RETENTION FACTOR", OrgFundFactValue = historicRetentionFactorValue },
                new OrgFunding() { OrgFundFactType = fundFactorType, OrgFundEffectiveFrom = effectiveFrom, OrgFundFactor = "HISTORIC LEVEL 3 PROGRAMME MATHS AND ENGLISH PROPORTION", OrgFundFactValue = historicLevel3ProgMathsAndEnglishProportion },
                new OrgFunding() { OrgFundFactType = fundFactorType, OrgFundEffectiveFrom = new DateTime(2017, 1, 1), OrgFundFactor = "SPECIALIST RESOURCES", OrgFundFactValue = "1" },
                new OrgFunding() { OrgFundFactType = fundFactorType, OrgFundEffectiveFrom = new DateTime(2017, 8, 1), OrgFundFactor = "HISTORIC AREA COST FACTOR" },
                new OrgFunding() { OrgFundFactType = "NOT EFA", OrgFundEffectiveFrom = new DateTime(2017, 8, 1), OrgFundFactor = "HISTORIC AREA COST FACTOR" },
                new OrgFunding() { OrgFundFactType = "NOT EFA",  OrgFundEffectiveFrom = effectiveFrom, OrgFundFactor = "HISTORIC AREA COST FACTOR" },
            };

            larsRefererenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsCurrentVersion);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationVersion()).Returns(orgCurrentVersion);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationFundingForUKPRN(ukprn)).Returns(orgFundings);
            postcodesReferenceDataServiceMock.Setup(p => p.PostcodesCurrentVersion()).Returns(postcodesCurrentVersion);

            var global = NewService(larsRefererenceDataServiceMock.Object, orgReferenceDataServiceMock.Object, postcodesReferenceDataServiceMock.Object).BuildGlobal(ukprn);

            global.AreaCostFactor1618.Should().Be(historicAreaCostFactorValue);
            global.DisadvantageProportion.Should().Be(historicDisadvantageFundingProportionValue);
            global.HistoricLargeProgrammeProportion.Should().Be(historicLargeProgrammeProportionValue);
            global.LARSVersion.Should().Be(larsCurrentVersion);
            global.OrgVersion.Should().Be(orgCurrentVersion);
            global.PostcodesVersion.Should().Be(postcodesCurrentVersion);
            global.ProgrammeWeighting.Should().Be(historicProgrammeCostWeightingFactorValue);
            global.RetentionFactor.Should().Be(historicRetentionFactorValue);
            global.Level3ProgMathsAndEnglishProportion.Should().Be(historicLevel3ProgMathsAndEnglishProportion);
            global.SpecialistResources.Should().Be(specialistResources);
            global.UKPRN.Should().Be(ukprn);
        }

        [Fact]
        public void BuildGlobal_MissingOrgFundings()
        {
            var larsCurrentVersion = "1.0.0";
            var orgCurrentVersion = "2.0.0";
            var postcodesCurrentVersion = "3.0.0";
            var ukprn = 1234;

            var larsRefererenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var orgReferenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var orgFundings = new List<OrgFunding>();

            larsRefererenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsCurrentVersion);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationVersion()).Returns(orgCurrentVersion);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationFundingForUKPRN(ukprn)).Returns(orgFundings);
            postcodesReferenceDataServiceMock.Setup(p => p.PostcodesCurrentVersion()).Returns(postcodesCurrentVersion);

            var global = NewService(larsRefererenceDataServiceMock.Object, orgReferenceDataServiceMock.Object, postcodesReferenceDataServiceMock.Object).BuildGlobal(ukprn);

            global.AreaCostFactor1618.Should().Be(null);
            global.DisadvantageProportion.Should().Be(null);
            global.HistoricLargeProgrammeProportion.Should().Be(null);
            global.LARSVersion.Should().Be(larsCurrentVersion);
            global.OrgVersion.Should().Be(orgCurrentVersion);
            global.PostcodesVersion.Should().Be(postcodesCurrentVersion);
            global.ProgrammeWeighting.Should().Be(null);
            global.RetentionFactor.Should().Be(null);
            global.Level3ProgMathsAndEnglishProportion.Should().Be(null);
            global.SpecialistResources.Should().Be(false);
            global.UKPRN.Should().Be(ukprn);
        }

        [Fact]
        public void BuildCampusIdentifierSpecResource()
        {
            var campusIdentifierSpecResource = new CampusIdentifierSpecResource
            {
                CampusIdentifier = "Id",
                EffectiveFrom = new DateTime(2019, 1, 1),
                SpecialistResources = "Y",
            };

            var dataEntity = NewService().BuildSpecialistResources(campusIdentifierSpecResource);

            dataEntity.EntityName.Should().Be("Camps_Identifiers_Reference_DataFunding");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["EffectiveFrom"].Value.Should().Be(campusIdentifierSpecResource.EffectiveFrom);
            dataEntity.Attributes["EffectiveTo"].Value.Should().Be(campusIdentifierSpecResource.EffectiveTo);
            dataEntity.Attributes["CampusFundingSpecialistResources"].Value.Should().Be(campusIdentifierSpecResource.SpecialistResources);
        }

        [Fact]
        public void BuildLearningDelivery()
        {
            var learningDelivery = new LearningDelivery()
            {
                AimSeqNumber = 1,
                AimType = 2,
                CompStatus = 3,
                FundModel = 4,
                LearnActEndDate = new DateTime(2017, 1, 1),
                LearnAimRef = "LearnAimRef",
                LearnPlanEndDate = new DateTime(2019, 1, 1),
                LearnStartDate = new DateTime(2020, 1, 1),
                PHours = 7,
                ProgType = 8,
                WithdrawReason = 9,
                LearningDeliveryFAMs = new List<LearningDeliveryFAM>()
                {
                    new LearningDeliveryFAM() { LearnDelFAMType = "SOF", LearnDelFAMCode = "SOF" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM1" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM2" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM3" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM4" },
                }
            };

            var larsLearningDelivery = new LARSLearningDelivery()
            {
                AwardOrgCode = "awardOrgCode",
                EFACOFType = 1,
                LearnAimRefTitle = "learnAimRefTitle",
                LearnAimRefType = "learnAimRefType",
                SectorSubjectAreaTier2 = 1.0m,
                GuidedLearningHours = 1,
                NotionalNVQLevel = "NotionalNVQLevel"
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);

            var dataEntity = NewService(larsReferenceDataServiceMock.Object).BuildLearningDeliveryDataEntity(learningDelivery);

            dataEntity.EntityName.Should().Be("LearningDelivery");
            dataEntity.Attributes.Should().HaveCount(18);
            dataEntity.Attributes["AimSeqNumber"].Value.Should().Be(learningDelivery.AimSeqNumber);
            dataEntity.Attributes["AimType"].Value.Should().Be(learningDelivery.AimType);
            dataEntity.Attributes["AwardOrgCode"].Value.Should().Be(larsLearningDelivery.AwardOrgCode);
            dataEntity.Attributes["CompStatus"].Value.Should().Be(learningDelivery.CompStatus);
            dataEntity.Attributes["EFACOFType"].Value.Should().Be(larsLearningDelivery.EFACOFType);
            dataEntity.Attributes["FundModel"].Value.Should().Be(learningDelivery.FundModel);
            dataEntity.Attributes["GuidedLearningHours"].Value.Should().Be(larsLearningDelivery.GuidedLearningHours);
            dataEntity.Attributes["LearnActEndDate"].Value.Should().Be(learningDelivery.LearnActEndDate);
            dataEntity.Attributes["LearnAimRef"].Value.Should().Be(learningDelivery.LearnAimRef);
            dataEntity.Attributes["LearnAimRefTitle"].Value.Should().Be(larsLearningDelivery.LearnAimRefTitle);
            dataEntity.Attributes["LearnAimRefType"].Value.Should().Be(larsLearningDelivery.LearnAimRefType);
            dataEntity.Attributes["LearnPlanEndDate"].Value.Should().Be(learningDelivery.LearnPlanEndDate);
            dataEntity.Attributes["LearnStartDate"].Value.Should().Be(learningDelivery.LearnStartDate);
            dataEntity.Attributes["NotionalNVQLevel"].Value.Should().Be(larsLearningDelivery.NotionalNVQLevel);
            dataEntity.Attributes["PHours"].Value.Should().Be(learningDelivery.PHours);
            dataEntity.Attributes["ProgType"].Value.Should().Be(learningDelivery.ProgType);
            dataEntity.Attributes["SectorSubjectAreaTier2"].Value.Should().Be(larsLearningDelivery.SectorSubjectAreaTier2);
            dataEntity.Attributes["WithdrawReason"].Value.Should().Be(learningDelivery.WithdrawReason);
        }

        [Fact]
        public void BuildLearningDeliveryFAM()
        {
            var learningDeliveryFAM = new LearningDeliveryFAM
            {
                LearnDelFAMType = "ADL",
                LearnDelFAMCode = "ADL"
            };

            var dataEntity = NewService().BuildLearningDeliveryFAM(learningDeliveryFAM);

            dataEntity.EntityName.Should().Be("LearningDeliveryFAM");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LearnDelFAMCode"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMCode);
            dataEntity.Attributes["LearnDelFAMDateFrom"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMDateFrom);
            dataEntity.Attributes["LearnDelFAMDateTo"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMDateTo);
            dataEntity.Attributes["LearnDelFAMType"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMType);
        }

        private List<OrgFunding> TestOrgFundingDataset()
        {
            var historicAreaCostFactorValue = "HISTORIC AREA COST FACTOR VALUE";
            var historicDisadvantageFundingProportionValue = "HISTORIC DISADVANTAGE FUNDING PROPORTION VALUE";
            var historicLargeProgrammeProportionValue = "HISTORIC LARGE PROGRAMME PROPORTION VALUE";
            var historicProgrammeCostWeightingFactorValue = "HISTORIC PROGRAMME COST WEIGHTING FACTOR VALUE";
            var historicRetentionFactorValue = "HISTORIC RETENTION FACTOR VALUE";
            var historicLevel3ProgMathsAndEnglishProportion = "HISTORIC LEVEL 3 PROGRAMME MATHS AND ENGLISH PROPORTION";

            return new List<OrgFunding>
            {
                new OrgFunding() { OrgFundFactType = "EFA 16-19", OrgFundEffectiveFrom = new DateTime(2020, 8, 1), OrgFundFactor = "HISTORIC AREA COST FACTOR", OrgFundFactValue = historicAreaCostFactorValue },
                new OrgFunding() { OrgFundFactType = "EFA 16-19", OrgFundEffectiveFrom = new DateTime(2020, 8, 1), OrgFundFactor = "HISTORIC DISADVANTAGE FUNDING PROPORTION", OrgFundFactValue = historicDisadvantageFundingProportionValue },
                new OrgFunding() { OrgFundFactType = "EFA 16-19", OrgFundEffectiveFrom = new DateTime(2020, 8, 1), OrgFundFactor = "HISTORIC LARGE PROGRAMME PROPORTION", OrgFundFactValue = historicLargeProgrammeProportionValue },
                new OrgFunding() { OrgFundFactType = "EFA 16-19", OrgFundEffectiveFrom = new DateTime(2020, 8, 1), OrgFundFactor = "HISTORIC PROGRAMME COST WEIGHTING FACTOR", OrgFundFactValue = historicProgrammeCostWeightingFactorValue },
                new OrgFunding() { OrgFundFactType = "EFA 16-19", OrgFundEffectiveFrom = new DateTime(2020, 8, 1), OrgFundFactor = "HISTORIC RETENTION FACTOR", OrgFundFactValue = historicRetentionFactorValue },
                new OrgFunding() { OrgFundFactType = "EFA 16-19", OrgFundEffectiveFrom = new DateTime(2020, 8, 1), OrgFundFactor = "HISTORIC LEVEL 3 PROGRAMME MATHS AND ENGLISH PROPORTION", OrgFundFactValue = historicLevel3ProgMathsAndEnglishProportion },
                new OrgFunding() { OrgFundFactType = "EFA 16-19", OrgFundEffectiveFrom = new DateTime(2020, 8, 1), OrgFundFactor = "SPECIALIST RESOURCES", OrgFundFactValue = "1" },
                new OrgFunding() { OrgFundFactType = "EFA 16-19", OrgFundEffectiveFrom = new DateTime(2020, 8, 1), OrgFundFactor = "HISTORIC AREA COST FACTOR" },
                new OrgFunding() { OrgFundFactType = "NOT EFA", OrgFundEffectiveFrom = new DateTime(2020, 8, 1), OrgFundFactor = "HISTORIC AREA COST FACTOR" }
            };
        }

        private DataEntityMapper NewService(ILARSReferenceDataService larsReferenceDataService = null, IOrganisationReferenceDataService organisationReferenceDataService = null, IPostcodesReferenceDataService postcodesReferenceDataService = null)
        {
            return new DataEntityMapper(larsReferenceDataService, organisationReferenceDataService, postcodesReferenceDataService);
        }
    }
}
