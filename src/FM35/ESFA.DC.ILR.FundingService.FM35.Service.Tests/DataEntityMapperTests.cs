using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Organisation.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM35.Service.Input;
using ESFA.DC.ILR.FundingService.FM35.Service.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM35.Service.Tests
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
            var postcodePrior = "Postcode";
            var empId = 1;

            var global = new Global
            {
                LARSVersion = larsVersion,
                UKPRN = ukprn,
                OrgVersion = orgVersion,
                PostcodeDisadvantageVersion = postcodeVersion
            };

            var learnerDtos = new List<FM35LearnerDto>
            {
                new FM35LearnerDto
                {
                    PostcodePrior = postcodePrior,
                    LearnerEmploymentStatuses = new List<LearnerEmploymentStatus>
                    {
                        new LearnerEmploymentStatus
                        {
                            EmpId = empId
                        }
                    },
                    LearningDeliveries = new List<LearningDelivery>
                    {
                        new LearningDelivery
                        {
                            LearnAimRef = learnAimRef,
                            FundModel = 35,
                        }
                    }
                },
                new FM35LearnerDto
                {
                    PostcodePrior = postcodePrior,
                    LearnerEmploymentStatuses = new List<LearnerEmploymentStatus>
                    {
                        new LearnerEmploymentStatus
                        {
                            EmpId = empId
                        }
                    },
                    LearningDeliveries = new List<LearningDelivery>
                    {
                        new LearningDelivery
                        {
                            LearnAimRef = learnAimRef,
                            FundModel = 35,
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

            var larsStandard = new LARSStandard
            {
                StandardCode = 1,
            };

            IEnumerable<DasDisadvantage> dasDisadvantage = new List<DasDisadvantage>
            {
                new DasDisadvantage
                {
                    Postcode = "DelLocPostcode",
                    EffectiveFrom = new DateTime(2019, 1, 1),
                    EffectiveTo = new DateTime(2020, 1, 1),
                    Uplift = 1.2m
                }
            };

            var largeEmployersReferenceDataServiceMock = new Mock<ILargeEmployersReferenceDataService>();
            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var orgReferenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            largeEmployersReferenceDataServiceMock.Setup(l => l.LargeEmployersforEmpID(empId)).Returns(new List<LargeEmployers> { new LargeEmployers() });
            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(global.LARSVersion);
            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learnAimRef)).Returns(larsLearningDelivery);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationFundingForUKPRN(global.UKPRN)).Returns(new List<OrgFunding> { new OrgFunding() });
            postcodesReferenceDataServiceMock.Setup(o => o.SFADisadvantagesForPostcode(postcodePrior)).Returns(new List<SfaDisadvantage>());

            var dataEntities = NewService(
                largeEmployersReferenceDataServiceMock.Object,
                larsReferenceDataServiceMock.Object,
                orgReferenceDataServiceMock.Object,
                postcodesReferenceDataServiceMock.Object)
                .MapTo(ukprn, learnerDtos);

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
                PostcodeDisadvantageVersion = postcodeVersion
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var orgReferenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsVersion);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationVersion()).Returns(orgVersion);
            postcodesReferenceDataServiceMock.Setup(p => p.PostcodesCurrentVersion()).Returns(postcodeVersion);

            var dataEntities = NewService(
                larsReferenceDataService: larsReferenceDataServiceMock.Object,
                organisationReferenceDataService: orgReferenceDataServiceMock.Object,
                postcodesReferenceDataService: postcodesReferenceDataServiceMock.Object).MapTo(ukprn, null);

            dataEntities.Should().HaveCount(1);
            dataEntities.Select(d => d.IsGlobal).First().Should().Be(true);
            dataEntities.Select(d => d.Children).First().Should().HaveCount(0);
            dataEntities.Select(d => d.EntityName).First().Should().Be("global");
            dataEntities.Select(d => d.Attributes).First().Should().HaveCount(4);
            dataEntities.Select(d => d.Attributes).First()["OrgVersion"].Value.Should().Be(global.OrgVersion);
            dataEntities.Select(d => d.Attributes).First()["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntities.Select(d => d.Attributes).First()["PostcodeDisadvantageVersion"].Value.Should().Be(global.PostcodeDisadvantageVersion);
            dataEntities.Select(d => d.Attributes).First()["UKPRN"].Value.Should().Be(global.UKPRN);
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
                PostcodeDisadvantageVersion = postcodeVersion
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var orgReferenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsVersion);
            orgReferenceDataServiceMock.Setup(o => o.OrganisationVersion()).Returns(orgVersion);
            postcodesReferenceDataServiceMock.Setup(p => p.PostcodesCurrentVersion()).Returns(postcodeVersion);

            var dataEntities = NewService(
                larsReferenceDataService: larsReferenceDataServiceMock.Object,
                organisationReferenceDataService: orgReferenceDataServiceMock.Object,
                postcodesReferenceDataService: postcodesReferenceDataServiceMock.Object).MapTo(ukprn, new List<FM35LearnerDto>());

            dataEntities.Should().HaveCount(1);
            dataEntities.Select(d => d.IsGlobal).First().Should().Be(true);
            dataEntities.Select(d => d.Children).First().Should().HaveCount(0);
            dataEntities.Select(d => d.EntityName).First().Should().Be("global");
            dataEntities.Select(d => d.Attributes).First().Should().HaveCount(4);
            dataEntities.Select(d => d.Attributes).First()["OrgVersion"].Value.Should().Be(global.OrgVersion);
            dataEntities.Select(d => d.Attributes).First()["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntities.Select(d => d.Attributes).First()["PostcodeDisadvantageVersion"].Value.Should().Be(global.PostcodeDisadvantageVersion);
            dataEntities.Select(d => d.Attributes).First()["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void BuildGlobalDataEntity()
        {
            var ukprn = 1234;

            var learner = new FM35LearnerDto
            {
                PostcodePrior = "Postcode"
            };

            var global = new Global
            {
                LARSVersion = "1.0.0",
                PostcodeDisadvantageVersion = "2.0.0",
                OrgVersion = "3.0.0",
                UKPRN = ukprn
            };

            var organisationRefererenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();
            var postcodesRefererenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            organisationRefererenceDataServiceMock.Setup(o => o.OrganisationFundingForUKPRN(global.UKPRN)).Returns(new List<OrgFunding> { new OrgFunding() });
            postcodesRefererenceDataServiceMock.Setup(o => o.SFADisadvantagesForPostcode(learner.PostcodePrior)).Returns(new List<SfaDisadvantage>());

            var dataEntity = NewService(
                organisationReferenceDataService: organisationRefererenceDataServiceMock.Object,
                postcodesReferenceDataService: postcodesRefererenceDataServiceMock.Object).BuildGlobalDataEntity(learner, global);

            dataEntity.EntityName.Should().Be("global");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntity.Attributes["PostcodeDisadvantageVersion"].Value.Should().Be(global.PostcodeDisadvantageVersion);
            dataEntity.Attributes["OrgVersion"].Value.Should().Be(global.OrgVersion);
            dataEntity.Attributes["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void BuildGlobal()
        {
            var larsVersion = "1.0.0";
            var postcodeDisadvantageVersion = "2.0.0";
            var orgVersion = "3.0.0";
            var ukprn = 1234;

            var larsRefererenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodesRefererenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var organisationRefererenceDataServiceMock = new Mock<IOrganisationReferenceDataService>();

            larsRefererenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsVersion);
            postcodesRefererenceDataServiceMock.Setup(p => p.PostcodesCurrentVersion()).Returns(postcodeDisadvantageVersion);
            organisationRefererenceDataServiceMock.Setup(o => o.OrganisationVersion()).Returns(orgVersion);

            var global = NewService(
                larsReferenceDataService: larsRefererenceDataServiceMock.Object,
                postcodesReferenceDataService: postcodesRefererenceDataServiceMock.Object,
                organisationReferenceDataService: organisationRefererenceDataServiceMock.Object)
                .BuildGlobal(ukprn);

            global.LARSVersion.Should().Be(larsVersion);
            global.UKPRN.Should().Be(ukprn);
            global.PostcodeDisadvantageVersion.Should().Be(postcodeDisadvantageVersion);
            global.OrgVersion.Should().Be(orgVersion);
        }

        [Fact]
        public void BuildOrgFunding()
        {
            var orgFunding = new OrgFunding()
            {
                UKPRN = 12345678,
                OrgFundFactor = "Factor",
                OrgFundFactType = "Type",
                OrgFundFactValue = "Value",
                OrgFundEffectiveFrom = new DateTime(2019, 8, 1),
                OrgFundEffectiveTo = null,
            };

            var dataEntity = NewService().BuildOrgFundingDataEntity(orgFunding);

            dataEntity.EntityName.Should().Be("OrgFunding");
            dataEntity.Attributes.Should().HaveCount(5);
            dataEntity.Attributes["OrgFundFactor"].Value.Should().Be(orgFunding.OrgFundFactor);
            dataEntity.Attributes["OrgFundFactType"].Value.Should().Be(orgFunding.OrgFundFactType);
            dataEntity.Attributes["OrgFundFactValue"].Value.Should().Be(orgFunding.OrgFundFactValue);
            dataEntity.Attributes["OrgFundEffectiveFrom"].Value.Should().Be(orgFunding.OrgFundEffectiveFrom);
            dataEntity.Attributes["OrgFundEffectiveTo"].Value.Should().Be(orgFunding.OrgFundEffectiveTo);

            dataEntity.Children.Should().BeEmpty();
        }

        [Fact]
        public void BuildLearner()
        {
            var learner = new FM35LearnerDto()
            {
                LearnRefNumber = "ABC",
                DateOfBirth = new DateTime(2000, 8, 1)
            };

            var postcodesRefererenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var organisationsDataServiceMock = new Mock<IOrganisationReferenceDataService>();

            postcodesRefererenceDataServiceMock.Setup(l => l.SFADisadvantagesForPostcode(It.IsAny<string>())).Returns(new List<SfaDisadvantage>());
            organisationsDataServiceMock.Setup(l => l.SpecialistResourcesForCampusIdentifier(It.IsAny<string>())).Returns(new List<CampusIdentifierSpecResource>());

            var dataEntity = NewService(
                organisationReferenceDataService: organisationsDataServiceMock.Object,
                postcodesReferenceDataService: postcodesRefererenceDataServiceMock.Object)
                .BuildLearnerDataEntity(learner);

            dataEntity.EntityName.Should().Be("Learner");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["LearnRefNumber"].Value.Should().Be(learner.LearnRefNumber);
            dataEntity.Attributes["DateOfBirth"].Value.Should().Be(learner.DateOfBirth);

            dataEntity.Children.Should().BeEmpty();
        }

        [Fact]
        public void BuildLearningDelivery()
        {
            var learningDelivery = new LearningDelivery()
            {
                AimSeqNumber = 1,
                AimType = 2,
                CompStatus = 3,
                DelLocPostCode = "DelLocPostcode",
                FundModel = 4,
                FworkCode = 5,
                LearnActEndDate = new DateTime(2019, 2, 1),
                LearnAimRef = "LearnAimRef",
                LearnPlanEndDate = new DateTime(2019, 1, 1),
                LearnStartDate = new DateTime(2020, 1, 1),
                OrigLearnStartDate = new DateTime(2020, 1, 1),
                OtherFundAdj = 6,
                Outcome = 7,
                PriorLearnFundAdj = 8,
                ProgType = 9,
                PwayCode = 10,
                LearningDeliveryFAMs = new List<LearningDeliveryFAM>()
                {
                    new LearningDeliveryFAM() { LearnDelFAMType = "EEF", LearnDelFAMCode = "EEF" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM1" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM2" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM3" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM4" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "FFI", LearnDelFAMCode = "FFI" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "RES", LearnDelFAMCode = "RES" },
                },
            };

            var frameworks = new List<LARSFramework>
            {
                new LARSFramework
                {
                    EffectiveFromNullable = new DateTime(2019, 1, 1),
                    EffectiveTo = new DateTime(2020, 1, 1),
                    FworkCode = 5,
                    ProgType = 9,
                    PwayCode = 10,
                    LARSFrameworkAim = new LARSFrameworkAim
                    {
                        EffectiveFrom = new DateTime(2019, 1, 1),
                        EffectiveTo = new DateTime(2020, 1, 1),
                        FrameworkComponentType = 1,
                    }
                },
                new LARSFramework
                {
                    EffectiveFromNullable = new DateTime(2019, 1, 1),
                    EffectiveTo = new DateTime(2020, 1, 1),
                    FworkCode = 9,
                    ProgType = 9,
                    PwayCode = 5,
                    LARSFrameworkAim = new LARSFrameworkAim
                    {
                        EffectiveFrom = new DateTime(2019, 1, 1),
                        EffectiveTo = new DateTime(2020, 1, 1),
                        FrameworkComponentType = 1,
                    }
                }
            };

            var larsLearningDelivery = new LARSLearningDelivery
            {
                AwardOrgCode = "awardOrgCode",
                EFACOFType = 1,
                FrameworkCommonComponent = 2,
                LearnAimRefTitle = "learnAimRefTitle",
                LearnAimRefType = "learnAimRefType",
                RegulatedCreditValue = 3,
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
                    },
                },
                LARSAnnualValues = new List<LARSAnnualValue>
                {
                    new LARSAnnualValue()
                },
                LARSLearningDeliveryCategories = new List<LARSLearningDeliveryCategory>
                {
                    new LARSLearningDeliveryCategory()
                },
                LARSFrameworks = frameworks
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);
            postcodesReferenceDataServiceMock.Setup(p => p.SFAAreaCostsForPostcode(learningDelivery.DelLocPostCode)).Returns(new List<SfaAreaCost> { new SfaAreaCost() });

            var dataEntity = NewService(
                larsReferenceDataService: larsReferenceDataServiceMock.Object,
                postcodesReferenceDataService: postcodesReferenceDataServiceMock.Object).BuildLearningDeliveryDataEntity(learningDelivery);

            dataEntity.EntityName.Should().Be("LearningDelivery");
            dataEntity.Attributes.Should().HaveCount(20);
            dataEntity.Attributes["AchDate"].Value.Should().Be(learningDelivery.AchDate);
            dataEntity.Attributes["AddHours"].Value.Should().Be(learningDelivery.AddHours);
            dataEntity.Attributes["AimSeqNumber"].Value.Should().Be(learningDelivery.AimSeqNumber);
            dataEntity.Attributes["AimType"].Value.Should().Be(learningDelivery.AimType);
            dataEntity.Attributes["CompStatus"].Value.Should().Be(learningDelivery.CompStatus);
            dataEntity.Attributes["EmpOutcome"].Value.Should().Be(learningDelivery.EmpOutcome);
            dataEntity.Attributes["EnglandFEHEStatus"].Value.Should().Be(larsLearningDelivery.EnglandFEHEStatus);
            dataEntity.Attributes["EnglPrscID"].Value.Should().Be(larsLearningDelivery.EnglPrscID);
            dataEntity.Attributes["FrameworkCommonComponent"].Value.Should().Be(2);
            dataEntity.Attributes["FworkCode"].Value.Should().Be(learningDelivery.FworkCode);
            dataEntity.Attributes["LearnActEndDate"].Value.Should().Be(learningDelivery.LearnActEndDate);
            dataEntity.Attributes["LearnPlanEndDate"].Value.Should().Be(learningDelivery.LearnPlanEndDate);
            dataEntity.Attributes["LearnStartDate"].Value.Should().Be(learningDelivery.LearnStartDate);
            dataEntity.Attributes["OrigLearnStartDate"].Value.Should().Be(learningDelivery.OrigLearnStartDate);
            dataEntity.Attributes["OtherFundAdj"].Value.Should().Be(learningDelivery.OtherFundAdj);
            dataEntity.Attributes["PriorLearnFundAdj"].Value.Should().Be(learningDelivery.PriorLearnFundAdj);
            dataEntity.Attributes["ProgType"].Value.Should().Be(learningDelivery.ProgType);
            dataEntity.Attributes["PwayCode"].Value.Should().Be(learningDelivery.PwayCode);
        }

        [Fact]
        public void BuildLearningDeliveryFAM()
        {
            var learningDeliveryFAM = new LearningDeliveryFAM
            {
                LearnDelFAMType = "EEF",
                LearnDelFAMCode = "1",
            };

            var dataEntity = NewService().BuildLearningDeliveryFAM(learningDeliveryFAM);

            dataEntity.EntityName.Should().Be("LearningDeliveryFAM");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LearnDelFAMCode"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMCode);
            dataEntity.Attributes["LearnDelFAMDateFrom"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMDateFrom);
            dataEntity.Attributes["LearnDelFAMDateTo"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMDateTo);
            dataEntity.Attributes["LearnDelFAMType"].Value.Should().Be(learningDeliveryFAM.LearnDelFAMType);
        }

        [Fact]
        public void BuildLearnerEmploymentStatus()
        {
            var learnerEmploymentStatus = new LearnerEmploymentStatus
            {
                DateEmpStatApp = new DateTime(2019, 1, 1),
                EmpId = 1,
                EmpStat = 2,
            };

            var largeEmployersReferenceDataServiceMock = new Mock<ILargeEmployersReferenceDataService>();

            largeEmployersReferenceDataServiceMock.Setup(l => l.LargeEmployersforEmpID(learnerEmploymentStatus.EmpId)).Returns(new List<LargeEmployers> { new LargeEmployers() });

            var dataEntity = NewService(largeEmployersReferenceDataService: largeEmployersReferenceDataServiceMock.Object).BuildLearnerEmploymentStatus(learnerEmploymentStatus);

            dataEntity.EntityName.Should().Be("LearnerEmploymentStatus");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["DateEmpStatApp"].Value.Should().Be(learnerEmploymentStatus.DateEmpStatApp);
            dataEntity.Attributes["EmpId"].Value.Should().Be(learnerEmploymentStatus.EmpId);
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
            dataEntity.Attributes["SpecialistResources"].Value.Should().Be(campusIdentifierSpecResource.SpecialistResources);
        }

        [Fact]
        public void BuildLargeEmployer()
        {
            var largeEmployer = new LargeEmployers
            {
                ERN = 100,
                EffectiveFrom = new DateTime(2019, 1, 1),
                EffectiveTo = null,
            };

            var dataEntity = NewService().BuildLargeEmployer(largeEmployer);

            dataEntity.EntityName.Should().Be("LargeEmployerReferenceData");
            dataEntity.Attributes.Should().HaveCount(2);
            dataEntity.Attributes["LargeEmpEffectiveFrom"].Value.Should().Be(largeEmployer.EffectiveFrom);
            dataEntity.Attributes["LargeEmpEffectiveTo"].Value.Should().Be(largeEmployer.EffectiveTo);

            dataEntity.Children.Should().BeNullOrEmpty();
        }

        [Fact]
        public void BuildSFAPostcodeDisadvantage()
        {
            var sfaDisadvantage = new SfaDisadvantage
            {
                Uplift = 1.2m,
                EffectiveFrom = new DateTime(2019, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
            };

            var dataEntity = NewService().BuildSFAPostcodeDisadvantage(sfaDisadvantage);

            dataEntity.EntityName.Should().Be("SFA_PostcodeDisadvantage");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["DisUplift"].Value.Should().Be(sfaDisadvantage.Uplift);
            dataEntity.Attributes["DisUpEffectiveFrom"].Value.Should().Be(sfaDisadvantage.EffectiveFrom);
            dataEntity.Attributes["DisUpEffectiveTo"].Value.Should().Be(sfaDisadvantage.EffectiveTo);
        }

        [Fact]
        public void BuildSFAAreaCost()
        {
            var sfaAreaCost = new SfaAreaCost
            {
                AreaCostFactor = 1.2m,
                EffectiveFrom = new DateTime(2019, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1),
            };

            var dataEntity = NewService().BuildSFAAreaCost(sfaAreaCost);

            dataEntity.EntityName.Should().Be("SFA_PostcodeAreaCost");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["AreaCosFactor"].Value.Should().Be(sfaAreaCost.AreaCostFactor);
            dataEntity.Attributes["AreaCosEffectiveFrom"].Value.Should().Be(sfaAreaCost.EffectiveFrom);
            dataEntity.Attributes["AreaCosEffectiveTo"].Value.Should().Be(sfaAreaCost.EffectiveTo);
        }

        [Fact]
        public void BuildLARSFunding()
        {
            var larsFunding = new LARSFunding
            {
                FundingCategory = "FundingCategory",
                EffectiveFrom = new DateTime(2019, 1, 1),
                EffectiveTo = new DateTime(2020, 1, 1),
                RateWeighted = 1.0m,
                RateUnWeighted = 2.0m,
                WeightingFactor = "G",
            };

            var dataEntity = NewService().BuildLARSFunding(larsFunding);

            dataEntity.EntityName.Should().Be("LearningDeliveryLARS_Funding");
            dataEntity.Attributes.Should().HaveCount(6);
            dataEntity.Attributes["LARSFundCategory"].Value.Should().Be(larsFunding.FundingCategory);
            dataEntity.Attributes["LARSFundEffectiveFrom"].Value.Should().Be(larsFunding.EffectiveFrom);
            dataEntity.Attributes["LARSFundEffectiveTo"].Value.Should().Be(larsFunding.EffectiveTo);
            dataEntity.Attributes["LARSFundWeightedRate"].Value.Should().Be(larsFunding.RateWeighted);
            dataEntity.Attributes["LARSFundUnweightedRate"].Value.Should().Be(larsFunding.RateUnWeighted);
            dataEntity.Attributes["LARSFundWeightingFactor"].Value.Should().Be(larsFunding.WeightingFactor);
        }

        [Fact]
        public void BuildLARSLearningDeliveryCategories()
        {
            var larsLearningDeliveryCategory = new LARSLearningDeliveryCategory
            {
                CategoryRef = 1,
                EffectiveFrom = new DateTime(2019, 1, 1),
                EffectiveTo = new DateTime(2020, 1, 1),
            };

            var dataEntity = NewService().BuildLARSLearningDeliveryCategories(larsLearningDeliveryCategory);

            dataEntity.EntityName.Should().Be("LearningDeliveryLARSCategory");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["LearnDelCatRef"].Value.Should().Be(larsLearningDeliveryCategory.CategoryRef);
            dataEntity.Attributes["LearnDelCatDateFrom"].Value.Should().Be(larsLearningDeliveryCategory.EffectiveFrom);
            dataEntity.Attributes["LearnDelCatDateTo"].Value.Should().Be(larsLearningDeliveryCategory.EffectiveTo);
        }

        [Fact]
        public void BuildLARSAnnualValue()
        {
            var larsAnnualValue = new LARSAnnualValue
            {
                BasicSkillsType = 1,
                EffectiveFrom = new DateTime(2019, 1, 1),
                EffectiveTo = new DateTime(2020, 1, 1),
            };

            var dataEntity = NewService().BuildLARSAnnualValue(larsAnnualValue);

            dataEntity.EntityName.Should().Be("LearningDeliveryAnnualValue");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["LearnDelAnnValBasicSkillsTypeCode"].Value.Should().Be(larsAnnualValue.BasicSkillsType);
            dataEntity.Attributes["LearnDelAnnValDateFrom"].Value.Should().Be(larsAnnualValue.EffectiveFrom);
            dataEntity.Attributes["LearnDelAnnValDateTo"].Value.Should().Be(larsAnnualValue.EffectiveTo);
        }

        private DataEntityMapper NewService(
            ILargeEmployersReferenceDataService largeEmployersReferenceDataService = null,
            ILARSReferenceDataService larsReferenceDataService = null,
            IOrganisationReferenceDataService organisationReferenceDataService = null,
            IPostcodesReferenceDataService postcodesReferenceDataService = null)
        {
            return new DataEntityMapper(
                largeEmployersReferenceDataService,
                larsReferenceDataService,
                organisationReferenceDataService,
                postcodesReferenceDataService);
        }
    }
}
