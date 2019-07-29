using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Interface;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM36.Service.Input;
using ESFA.DC.ILR.FundingService.FM36.Service.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM36.Service.Tests
{
    public class DataEntityMapperTests
    {
        [Fact]
        public void MapTo()
        {
            var ukprn = 1;
            var learnAimRef = "LearnAimRef";
            var delLocPostCode = "Postcode";
            var stdCode = 1;

            var larsVersion = "1.0.0";
            var year = "1920";
            var collectionPeriod = "DefaultPeriod";

            var global = new Global
            {
                LARSVersion = larsVersion,
                UKPRN = ukprn,
                Year = year,
                CollectionPeriod = collectionPeriod
            };

            var learnerDtos = new List<FM36LearnerDto>
            {
                new FM36LearnerDto
                {
                    ULN = 1,
                    LearningDeliveries = new List<LearningDelivery>
                    {
                        new LearningDelivery
                        {
                            LearnAimRef = learnAimRef,
                            DelLocPostCode = delLocPostCode,
                            FundModel = 36,
                            StdCode = stdCode
                        }
                    }
                },
                new FM36LearnerDto
                {
                    ULN = 1,
                    LearningDeliveries = new List<LearningDelivery>
                    {
                        new LearningDelivery
                        {
                            LearnAimRef = learnAimRef,
                            DelLocPostCode = delLocPostCode,
                            FundModel = 36,
                            StdCode = stdCode
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

            var postcodesRefererenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var appsHistoryRefererenceDataServiceMock = new Mock<IAppsEarningsHistoryReferenceDataService>();
            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(global.LARSVersion);
            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learnAimRef)).Returns(larsLearningDelivery);
            larsReferenceDataServiceMock.Setup(l => l.LARSStandardForStandardCode(stdCode)).Returns(larsStandard);

            postcodesRefererenceDataServiceMock.Setup(l => l.DASDisadvantagesForPostcode(It.IsAny<string>())).Returns(dasDisadvantage);
            appsHistoryRefererenceDataServiceMock.Setup(l => l.AECEarningsHistory(It.IsAny<long>())).Returns(new List<AECEarningsHistory>());

            var dataEntities = NewService(
                larsReferenceDataServiceMock.Object,
                postcodesRefererenceDataServiceMock.Object,
                appsHistoryRefererenceDataServiceMock.Object)
                .MapTo(ukprn, learnerDtos);

            dataEntities.Should().HaveCount(2);
            dataEntities.SelectMany(d => d.Children).Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void MapTo_NullLearnerDto()
        {
            var ukprn = 1;
            var larsVersion = "1.0.0";
            var year = "1920";
            var collectionPeriod = "DefaultPeriod";

            var global = new Global
            {
                LARSVersion = larsVersion,
                UKPRN = ukprn,
                Year = year,
                CollectionPeriod = collectionPeriod
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsVersion);

            var dataEntities = NewService(larsReferenceDataServiceMock.Object).MapTo(ukprn, null);

            dataEntities.Should().HaveCount(1);
            dataEntities.Select(d => d.IsGlobal).First().Should().Be(true);
            dataEntities.Select(d => d.Children).First().Should().HaveCount(0);
            dataEntities.Select(d => d.EntityName).First().Should().Be("global");
            dataEntities.Select(d => d.Attributes).First().Should().HaveCount(4);
            dataEntities.Select(d => d.Attributes).First()["CollectionPeriod"].Value.Should().Be(global.CollectionPeriod);
            dataEntities.Select(d => d.Attributes).First()["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntities.Select(d => d.Attributes).First()["Year"].Value.Should().Be(global.Year);
            dataEntities.Select(d => d.Attributes).First()["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void MapTo_EmptyLearners()
        {
            var ukprn = 1;
            var larsVersion = "1.0.0";
            var year = "1920";
            var collectionPeriod = "DefaultPeriod";

            var global = new Global
            {
                LARSVersion = larsVersion,
                UKPRN = ukprn,
                Year = year,
                CollectionPeriod = collectionPeriod
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsVersion);

            var dataEntities = NewService(larsReferenceDataServiceMock.Object).MapTo(ukprn, new List<FM36LearnerDto>());

            dataEntities.Should().HaveCount(1);
            dataEntities.Select(d => d.IsGlobal).First().Should().Be(true);
            dataEntities.Select(d => d.Children).First().Should().HaveCount(0);
            dataEntities.Select(d => d.EntityName).First().Should().Be("global");
            dataEntities.Select(d => d.Attributes).First().Should().HaveCount(4);
            dataEntities.Select(d => d.Attributes).First()["CollectionPeriod"].Value.Should().Be(global.CollectionPeriod);
            dataEntities.Select(d => d.Attributes).First()["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntities.Select(d => d.Attributes).First()["Year"].Value.Should().Be(global.Year);
            dataEntities.Select(d => d.Attributes).First()["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void BuildGlobalDataEntity()
        {
            var ukprn = 1234;

            var learner = new FM36LearnerDto
            {
                PostcodePrior = "Postcode",
                ULN = 123456789
            };

            var global = new Global
            {
                LARSVersion = "1.0.0",
                UKPRN = ukprn
            };

            var postcodesRefererenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var appsHistoryRefererenceDataServiceMock = new Mock<IAppsEarningsHistoryReferenceDataService>();

            postcodesRefererenceDataServiceMock.Setup(l => l.DASDisadvantagesForPostcode(It.IsAny<string>())).Returns(new List<DasDisadvantage>());
            appsHistoryRefererenceDataServiceMock.Setup(l => l.AECEarningsHistory(It.IsAny<long>())).Returns(new List<AECEarningsHistory>());

            var dataEntity = NewService(
                postcodesReferenceDataService: postcodesRefererenceDataServiceMock.Object,
                appsEarningsHistoryReferenceDataService: appsHistoryRefererenceDataServiceMock.Object)
                .BuildGlobalDataEntity(learner, global);

            dataEntity.EntityName.Should().Be("global");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LARSVersion"].Value.Should().Be(global.LARSVersion);
            dataEntity.Attributes["CollectionPeriod"].Value.Should().Be(global.CollectionPeriod);
            dataEntity.Attributes["Year"].Value.Should().Be(global.Year);
            dataEntity.Attributes["UKPRN"].Value.Should().Be(global.UKPRN);
        }

        [Fact]
        public void BuildGlobal()
        {
            var larsCurrentVersion = "1.0.0";
            var collectionPeriod = "DefaultPeriod";
            var year = "1920";
            var ukprn = 1234;

            var larsRefererenceDataServiceMock = new Mock<ILARSReferenceDataService>();

            larsRefererenceDataServiceMock.Setup(l => l.LARSCurrentVersion()).Returns(larsCurrentVersion);

            var global = NewService(larsRefererenceDataServiceMock.Object).BuildGlobal(ukprn);

            global.LARSVersion.Should().Be(larsCurrentVersion);
            global.UKPRN.Should().Be(ukprn);
            global.Year.Should().Be(year);
            global.CollectionPeriod.Should().Be(collectionPeriod);
        }

        [Fact]
        public void BuildLearner()
        {
            var learner = new FM36LearnerDto()
            {
                LearnRefNumber = "ABC",
                DateOfBirth = new DateTime(2000, 8, 1),
                ULN = 1234567890,
                PrevUKPRN = 12345678,
                PMUKPRN = 99999999,
            };

            var postcodesRefererenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var appsHistoryRefererenceDataServiceMock = new Mock<IAppsEarningsHistoryReferenceDataService>();

            postcodesRefererenceDataServiceMock.Setup(l => l.DASDisadvantagesForPostcode(It.IsAny<string>())).Returns(new List<DasDisadvantage>());
            appsHistoryRefererenceDataServiceMock.Setup(l => l.AECEarningsHistory(It.IsAny<long>())).Returns(new List<AECEarningsHistory>());

            var dataEntity = NewService(
                postcodesReferenceDataService: postcodesRefererenceDataServiceMock.Object,
                appsEarningsHistoryReferenceDataService: appsHistoryRefererenceDataServiceMock.Object)
                .BuildLearnerDataEntity(learner);

            dataEntity.EntityName.Should().Be("Learner");
            dataEntity.Attributes.Should().HaveCount(5);
            dataEntity.Attributes["LearnRefNumber"].Value.Should().Be(learner.LearnRefNumber);
            dataEntity.Attributes["DateOfBirth"].Value.Should().Be(learner.DateOfBirth);
            dataEntity.Attributes["PrevUKPRN"].Value.Should().Be(learner.PrevUKPRN);
            dataEntity.Attributes["PMUKPRN"].Value.Should().Be(learner.PMUKPRN);
            dataEntity.Attributes["ULN"].Value.Should().Be(learner.ULN);

            dataEntity.Children.Should().BeEmpty();
        }

        [Fact]
        public void BuildLearningDelivery()
        {
            var learningDelivery = new LearningDelivery()
            {
                AimSeqNumber = 1,
                AchDate = new DateTime(2019, 8, 1),
                AimType = 2,
                CompStatus = 3,
                DelLocPostCode = "DelLocPostcode",
                FundModel = 4,
                FworkCode = 5,
                LearnActEndDate = new DateTime(2018, 1, 1),
                LearnAimRef = "LearnAimRef",
                LearnPlanEndDate = new DateTime(2019, 1, 1),
                LearnStartDate = new DateTime(2020, 1, 1),
                OrigLearnStartDate = new DateTime(2020, 1, 1),
                OtherFundAdj = 6,
                Outcome = 7,
                PriorLearnFundAdj = 8,
                ProgType = 9,
                PwayCode = 10,
                StdCode = 11,
                LearningDeliveryFAMs = new List<LearningDeliveryFAM>()
                {
                    new LearningDeliveryFAM() { LearnDelFAMType = "EEF", LearnDelFAMCode = "EEF" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM1" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM2" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM3" },
                    new LearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "LDM4" }
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
                    }
                },
            };

            var larsReferenceDataServiceMock = new Mock<ILARSReferenceDataService>();

            larsReferenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);

            var dataEntity = NewService(larsReferenceDataServiceMock.Object).BuildLearningDeliveryDataEntity(learningDelivery);

            dataEntity.EntityName.Should().Be("LearningDelivery");
            dataEntity.Attributes.Should().HaveCount(16);
            dataEntity.Attributes["AimSeqNumber"].Value.Should().Be(learningDelivery.AimSeqNumber);
            dataEntity.Attributes["AchDate"].Value.Should().Be(learningDelivery.AchDate);
            dataEntity.Attributes["AimType"].Value.Should().Be(learningDelivery.AimType);
            dataEntity.Attributes["CompStatus"].Value.Should().Be(learningDelivery.CompStatus);
            dataEntity.Attributes["FrameworkCommonComponent"].Value.Should().Be(2);
            dataEntity.Attributes["FworkCode"].Value.Should().Be(learningDelivery.FworkCode);
            dataEntity.Attributes["LearnAimRef"].Value.Should().Be(learningDelivery.LearnAimRef);
            dataEntity.Attributes["LearnActEndDate"].Value.Should().Be(learningDelivery.LearnActEndDate);
            dataEntity.Attributes["LearnPlanEndDate"].Value.Should().Be(learningDelivery.LearnPlanEndDate);
            dataEntity.Attributes["LearnStartDate"].Value.Should().Be(learningDelivery.LearnStartDate);
            dataEntity.Attributes["OrigLearnStartDate"].Value.Should().Be(learningDelivery.OrigLearnStartDate);
            dataEntity.Attributes["OtherFundAdj"].Value.Should().Be(learningDelivery.OtherFundAdj);
            dataEntity.Attributes["PriorLearnFundAdj"].Value.Should().Be(learningDelivery.PriorLearnFundAdj);
            dataEntity.Attributes["ProgType"].Value.Should().Be(learningDelivery.ProgType);
            dataEntity.Attributes["PwayCode"].Value.Should().Be(learningDelivery.PwayCode);
            dataEntity.Attributes["STDCode"].Value.Should().Be(learningDelivery.StdCode);
        }

        [Fact]
        public void BuildLearningDeliveryFAM()
        {
            var learningDeliveryFAM = new LearningDeliveryFAM
            {
                LearnDelFAMType = "EEF",
                LearnDelFAMCode = "1"
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
                AgreeId = "Id",
                DateEmpStatApp = new DateTime(2019, 1, 1),
                EmpId = 1,
                EmpStat = 2,
                SEM = 3
            };

            var dataEntity = NewService().BuildLearnerEmploymentStatus(learnerEmploymentStatus);

            dataEntity.EntityName.Should().Be("LearnerEmploymentStatus");
            dataEntity.Attributes.Should().HaveCount(5);
            dataEntity.Attributes["AgreeId"].Value.Should().Be(learnerEmploymentStatus.AgreeId);
            dataEntity.Attributes["DateEmpStatApp"].Value.Should().Be(learnerEmploymentStatus.DateEmpStatApp);
            dataEntity.Attributes["EmpId"].Value.Should().Be(learnerEmploymentStatus.EmpId);
            dataEntity.Attributes["EMPStat"].Value.Should().Be(learnerEmploymentStatus.EmpStat);
            dataEntity.Attributes["EmpStatMon_SEM"].Value.Should().Be(learnerEmploymentStatus.SEM);
        }

        [Fact]
        public void BuildSFAPostcodeDisadvantage()
        {
            var dasDisadvantage = new DasDisadvantage
            {
                Uplift = 1.2m,
                EffectiveFrom = new DateTime(2019, 1, 1),
                EffectiveTo = new DateTime(2019, 1, 1)
            };

            var dataEntity = NewService().BuildDASPostcodeDisadvantage(dasDisadvantage);

            dataEntity.EntityName.Should().Be("SFA_PostcodeDisadvantage");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["DisApprenticeshipUplift"].Value.Should().Be(dasDisadvantage.Uplift);
            dataEntity.Attributes["DisUpEffectiveFrom"].Value.Should().Be(dasDisadvantage.EffectiveFrom);
            dataEntity.Attributes["DisUpEffectiveTo"].Value.Should().Be(dasDisadvantage.EffectiveTo);
        }

        [Fact]
        public void BuildApprenticeshipsEarningsHistory()
        {
            var appsHistory = new AECEarningsHistory
            {
                AppIdentifier = "AppIdentifierInput",
                AppProgCompletedInTheYearInput = false,
                DaysInYear = 1,
                CollectionReturnCode = "1",
                CollectionYear = "1920",
                HistoricEffectiveTNPStartDateInput = new DateTime(2019, 8, 1),
                HistoricEmpIdEndWithinYear = 2,
                HistoricEmpIdStartWithinYear = 3,
                FworkCode = 4,
                HistoricLearner1618StartInput = false,
                HistoricPMRAmount = 1.0m,
                ProgrammeStartDateIgnorePathway = new DateTime(2019, 8, 1),
                ProgrammeStartDateMatchPathway = new DateTime(2019, 8, 1),
                ProgType = 5,
                PwayCode = 6,
                STDCode = 7,
                HistoricTNP1Input = 1.0m,
                HistoricTNP2Input = 1.0m,
                HistoricTNP3Input = 1.0m,
                HistoricTNP4Input = 1.0m,
                HistoricTotal1618UpliftPaymentsInTheYearInput = 1.0m,
                TotalProgAimPaymentsInTheYear = 1.0m,
                ULN = 1234567890,
                UKPRN = 12345678,
                UptoEndDate = new DateTime(2019, 8, 1),
                HistoricVirtualTNP3EndOfTheYearInput = 1.0m,
                HistoricVirtualTNP4EndOfTheYearInput = 1.0m,
                HistoricLearnDelProgEarliestACT2DateInput = new DateTime(2019, 8, 1),
            };

            var dataEntity = NewService().BuildApprenticeshipsEarningsHistory(appsHistory);

            dataEntity.EntityName.Should().Be("HistoricEarningInput");
            dataEntity.Attributes.Should().HaveCount(29);
            dataEntity.Attributes["AppIdentifierInput"].Value.Should().Be(appsHistory.AppIdentifier);
            dataEntity.Attributes["AppProgCompletedInTheYearInput"].Value.Should().Be(appsHistory.AppProgCompletedInTheYearInput);
            dataEntity.Attributes["HistoricCollectionReturnInput"].Value.Should().Be(appsHistory.CollectionReturnCode);
            dataEntity.Attributes["HistoricCollectionYearInput"].Value.Should().Be(appsHistory.CollectionYear);
            dataEntity.Attributes["HistoricDaysInYearInput"].Value.Should().Be(appsHistory.DaysInYear);
            dataEntity.Attributes["HistoricEffectiveTNPStartDateInput"].Value.Should().Be(appsHistory.HistoricEffectiveTNPStartDateInput);
            dataEntity.Attributes["HistoricEmpIdEndWithinYearInput"].Value.Should().Be(appsHistory.HistoricEmpIdEndWithinYear);
            dataEntity.Attributes["HistoricEmpIdStartWithinYearInput"].Value.Should().Be(appsHistory.HistoricEmpIdStartWithinYear);
            dataEntity.Attributes["HistoricFworkCodeInput"].Value.Should().Be(appsHistory.FworkCode);
            dataEntity.Attributes["HistoricLearnDelProgEarliestACT2DateInput"].Value.Should().Be(appsHistory.HistoricLearnDelProgEarliestACT2DateInput);
            dataEntity.Attributes["HistoricLearner1618AtStartInput"].Value.Should().Be(appsHistory.HistoricLearner1618StartInput);
            dataEntity.Attributes["HistoricPMRAmountInput"].Value.Should().Be(appsHistory.HistoricPMRAmount);
            dataEntity.Attributes["HistoricProgrammeStartDateIgnorePathwayInput"].Value.Should().Be(appsHistory.ProgrammeStartDateIgnorePathway);
            dataEntity.Attributes["HistoricProgrammeStartDateMatchPathwayInput"].Value.Should().Be(appsHistory.ProgrammeStartDateMatchPathway);
            dataEntity.Attributes["HistoricProgTypeInput"].Value.Should().Be(appsHistory.ProgType);
            dataEntity.Attributes["HistoricPwayCodeInput"].Value.Should().Be(appsHistory.PwayCode);
            dataEntity.Attributes["HistoricTotal1618UpliftPaymentsInTheYearInput"].Value.Should().Be(appsHistory.HistoricTotal1618UpliftPaymentsInTheYearInput);
            dataEntity.Attributes["HistoricTotalProgAimPaymentsInTheYearInput"].Value.Should().Be(appsHistory.TotalProgAimPaymentsInTheYear);
            dataEntity.Attributes["HistoricSTDCodeInput"].Value.Should().Be(appsHistory.STDCode);
            dataEntity.Attributes["HistoricTNP1Input"].Value.Should().Be(appsHistory.HistoricTNP1Input);
            dataEntity.Attributes["HistoricTNP2Input"].Value.Should().Be(appsHistory.HistoricTNP2Input);
            dataEntity.Attributes["HistoricTNP3Input"].Value.Should().Be(appsHistory.HistoricTNP3Input);
            dataEntity.Attributes["HistoricTNP4Input"].Value.Should().Be(appsHistory.HistoricTNP4Input);
            dataEntity.Attributes["HistoricUKPRNInput"].Value.Should().Be(appsHistory.UKPRN);
            dataEntity.Attributes["HistoricULNInput"].Value.Should().Be(appsHistory.ULN);
            dataEntity.Attributes["HistoricUptoEndDateInput"].Value.Should().Be(appsHistory.UptoEndDate);
            dataEntity.Attributes["HistoricVirtualTNP3EndofTheYearInput"].Value.Should().Be(appsHistory.HistoricVirtualTNP3EndOfTheYearInput);
            dataEntity.Attributes["HistoricVirtualTNP4EndofTheYearInput"].Value.Should().Be(appsHistory.HistoricVirtualTNP4EndOfTheYearInput);
        }

        [Fact]
        public void BuildApprenticeshipFinancialRecord()
        {
            var appFinRecord = new AppFinRecord
            {
                AFinCode = 1,
                AFinAmount = 2,
                AFinDate = new DateTime(2019, 8, 1),
                AFinType = "Type"
            };

            var dataEntity = NewService().BuildApprenticeshipFinancialRecord(appFinRecord);

            dataEntity.EntityName.Should().Be("ApprenticeshipFinancialRecord");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["AFinAmount"].Value.Should().Be(appFinRecord.AFinAmount);
            dataEntity.Attributes["AFinCode"].Value.Should().Be(appFinRecord.AFinCode);
            dataEntity.Attributes["AFinDate"].Value.Should().Be(appFinRecord.AFinDate);
            dataEntity.Attributes["AFinType"].Value.Should().Be(appFinRecord.AFinType);
        }

        [Fact]
        public void BuildLARSStandardApprenticeshipFunding()
        {
            var larsSAP = new LARSStandardApprenticeshipFunding
            {
                SixteenToEighteenEmployerAdditionalPayment = 1.0m,
                SixteenToEighteenProviderAdditionalPayment = 2.0m,
                SixteenToEighteenFrameworkUplift = 3.0m,
                CareLeaverAdditionalPayment = 4.0m,
                EffectiveFrom = new DateTime(2019, 8, 1),
                FundingCategory = "Type",
                MaxEmployerLevyCap = 5.0m,
                ReservedValue2 = 6.0m,
                ReservedValue3 = 7.0m
            };

            var dataEntity = NewService().BuildLARSStandardApprenticeshipFunding(larsSAP);

            dataEntity.EntityName.Should().Be("Standard_LARS_ApprenticshipFunding");
            dataEntity.Attributes.Should().HaveCount(10);
            dataEntity.Attributes["StandardAF1618EmployerAdditionalPayment"].Value.Should().Be(larsSAP.SixteenToEighteenEmployerAdditionalPayment);
            dataEntity.Attributes["StandardAF1618ProviderAdditionalPayment"].Value.Should().Be(larsSAP.SixteenToEighteenProviderAdditionalPayment);
            dataEntity.Attributes["StandardAF1618FrameworkUplift"].Value.Should().Be(larsSAP.SixteenToEighteenFrameworkUplift);
            dataEntity.Attributes["StandardAFCareLeaverAdditionalPayment"].Value.Should().Be(larsSAP.CareLeaverAdditionalPayment);
            dataEntity.Attributes["StandardAFEffectiveFrom"].Value.Should().Be(larsSAP.EffectiveFrom);
            dataEntity.Attributes["StandardAFEffectiveTo"].Value.Should().Be(larsSAP.EffectiveTo);
            dataEntity.Attributes["StandardAFFundingCategory"].Value.Should().Be(larsSAP.FundingCategory);
            dataEntity.Attributes["StandardAFMaxEmployerLevyCap"].Value.Should().Be(larsSAP.MaxEmployerLevyCap);
            dataEntity.Attributes["StandardAFReservedValue2"].Value.Should().Be(larsSAP.ReservedValue2);
            dataEntity.Attributes["StandardAFReservedValue3"].Value.Should().Be(larsSAP.ReservedValue3);
        }

        [Fact]
        public void BuildLARSFrameworkApprenticeshipFunding()
        {
            var larsFAP = new LARSFrameworkApprenticeshipFunding
            {
                SixteenToEighteenEmployerAdditionalPayment = 1.0m,
                SixteenToEighteenProviderAdditionalPayment = 2.0m,
                SixteenToEighteenFrameworkUplift = 3.0m,
                CareLeaverAdditionalPayment = 4.0m,
                EffectiveFrom = new DateTime(2019, 8, 1),
                FundingCategory = "Type",
                MaxEmployerLevyCap = 5.0m,
                ReservedValue2 = 6.0m,
                ReservedValue3 = 7.0m
            };

            var dataEntity = NewService().BuildLARSFrameworkApprenticeshipFunding(larsFAP);

            dataEntity.EntityName.Should().Be("Framework_LARS_ApprenticshipFunding");
            dataEntity.Attributes.Should().HaveCount(10);
            dataEntity.Attributes["FrameworkAF1618EmployerAdditionalPayment"].Value.Should().Be(larsFAP.SixteenToEighteenEmployerAdditionalPayment);
            dataEntity.Attributes["FrameworkAF1618ProviderAdditionalPayment"].Value.Should().Be(larsFAP.SixteenToEighteenProviderAdditionalPayment);
            dataEntity.Attributes["FrameworkAF1618FrameworkUplift"].Value.Should().Be(larsFAP.SixteenToEighteenFrameworkUplift);
            dataEntity.Attributes["FrameworkAFCareLeaverAdditionalPayment"].Value.Should().Be(larsFAP.CareLeaverAdditionalPayment);
            dataEntity.Attributes["FrameworkAFEffectiveFrom"].Value.Should().Be(larsFAP.EffectiveFrom);
            dataEntity.Attributes["FrameworkAFEffectiveTo"].Value.Should().Be(larsFAP.EffectiveTo);
            dataEntity.Attributes["FrameworkAFFundingCategory"].Value.Should().Be(larsFAP.FundingCategory);
            dataEntity.Attributes["FrameworkAFMaxEmployerLevyCap"].Value.Should().Be(larsFAP.MaxEmployerLevyCap);
            dataEntity.Attributes["FrameworkAFReservedValue2"].Value.Should().Be(larsFAP.ReservedValue2);
            dataEntity.Attributes["FrameworkAFReservedValue3"].Value.Should().Be(larsFAP.ReservedValue3);
        }

        [Fact]
        public void BuildLARSFrameworkCommonComponent()
        {
            var larsFCC = new LARSFrameworkCommonComponent
            {
                CommonComponent = 1,
                EffectiveFrom = new DateTime(2019, 8, 1)
            };

            var dataEntity = NewService().BuildLARSFrameworkCommonComponent(larsFCC);

            dataEntity.EntityName.Should().Be("LARS_FrameworkCmnComp");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["LARSFrameworkCommonComponentCode"].Value.Should().Be(larsFCC.CommonComponent);
            dataEntity.Attributes["LARSFrameworkCommonComponentEffectiveFrom"].Value.Should().Be(larsFCC.EffectiveFrom);
            dataEntity.Attributes["LARSFrameworkCommonComponentEffectiveTo"].Value.Should().Be(larsFCC.EffectiveTo);
        }

        [Fact]
        public void BuildLARSStandardCommonComponent()
        {
            var larsSCC = new LARSStandardCommonComponent
            {
                CommonComponent = 1,
                EffectiveFrom = new DateTime(2019, 8, 1)
            };

            var dataEntity = NewService().BuildLARSStandardCommonComponent(larsSCC);

            dataEntity.EntityName.Should().Be("LARS_StandardCommonComponent");
            dataEntity.Attributes.Should().HaveCount(3);
            dataEntity.Attributes["LARSStandardCommonComponentCode"].Value.Should().Be(larsSCC.CommonComponent);
            dataEntity.Attributes["LARSStandardCommonComponentEffectiveFrom"].Value.Should().Be(larsSCC.EffectiveFrom);
            dataEntity.Attributes["LARSStandardCommonComponentEffectiveTo"].Value.Should().Be(larsSCC.EffectiveTo);
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
                WeightingFactor = "G"
            };

            var dataEntity = NewService().BuildLARSFunding(larsFunding);

            dataEntity.EntityName.Should().Be("LearningDeliveryLARS_Funding");
            dataEntity.Attributes.Should().HaveCount(4);
            dataEntity.Attributes["LARSFundCategory"].Value.Should().Be(larsFunding.FundingCategory);
            dataEntity.Attributes["LARSFundEffectiveFrom"].Value.Should().Be(larsFunding.EffectiveFrom);
            dataEntity.Attributes["LARSFundEffectiveTo"].Value.Should().Be(larsFunding.EffectiveTo);
            dataEntity.Attributes["LARSFundWeightedRate"].Value.Should().Be(larsFunding.RateWeighted);
        }

        private DataEntityMapper NewService(
            ILARSReferenceDataService larsReferenceDataService = null,
            IPostcodesReferenceDataService postcodesReferenceDataService = null,
            IAppsEarningsHistoryReferenceDataService appsEarningsHistoryReferenceDataService = null)
        {
            return new DataEntityMapper(larsReferenceDataService, postcodesReferenceDataService, appsEarningsHistoryReferenceDataService);
        }
    }
}
