using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Providers.LearnerPaging;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Providers.Tests.LearnerPagingTests
{
    public class FM36LearnerPagingServiceTests
    {
        [Fact]
        public void ProvideDtos()
        {
            IMessage message = new Message
            {
                LearningProvider = new MessageLearningProvider
                {
                    UKPRN = 12345678
                },
                Learner = BuildLearners(10).ToArray(),
            };

            NewService().ProvideDtos(36, message).Should().HaveCount(1);
        }

        [Fact]
        public void ProvideDtos_MultiplePages()
        {
            IMessage message = new Message
            {
                LearningProvider = new MessageLearningProvider
                {
                    UKPRN = 12345678
                },
                Learner = BuildLearners(1600).ToArray(),
            };

            NewService().ProvideDtos(36, message).Should().HaveCount(4);
        }

        [Fact]
        public void ProvideDtos_FundModelMisMatch()
        {
            IMessage message = new Message
            {
                LearningProvider = new MessageLearningProvider
                {
                    UKPRN = 12345678
                },
                Learner = BuildLearners(10).ToArray(),
            };

            NewService().ProvideDtos(1, message).Should().HaveCount(0);
        }

        [Fact]
        public void ProvideDtos_DtoAsExpected()
        {
            IMessage message = new Message
            {
                LearningProvider = new MessageLearningProvider
                {
                    UKPRN = 12345678
                },
                Learner = new MessageLearner[]
                {
                    new MessageLearner
                    {
                        LearnRefNumber = "Learner_1",
                        DateOfBirthSpecified = true,
                        DateOfBirth = new DateTime(1990, 8, 1),
                        PostcodePrior = "PostCodePrior",
                        PMUKPRNSpecified = true, 
                        PMUKPRN = 1,
                        PrevUKPRNSpecified = true, 
                        PrevUKPRN = 2,
                        ULN = 1,
                        LearnerEmploymentStatus = new MessageLearnerLearnerEmploymentStatus[]
                        {
                            new MessageLearnerLearnerEmploymentStatus
                            {
                                EmpIdSpecified = true,
                                EmpId = 1,
                                AgreeId = "AgreeId",
                                DateEmpStatApp = new DateTime(2019, 8, 1),
                                EmpStat = 2,
                                EmploymentStatusMonitoring = new MessageLearnerLearnerEmploymentStatusEmploymentStatusMonitoring[]
                                {
                                    new MessageLearnerLearnerEmploymentStatusEmploymentStatusMonitoring
                                    {
                                        ESMType = "SEM",
                                        ESMCode = 1
                                    },
                                    new MessageLearnerLearnerEmploymentStatusEmploymentStatusMonitoring
                                    {
                                        ESMType = "HNS",
                                        ESMCode = 1
                                    }
                                }
                            }
                        },
                        LearningDelivery = new MessageLearnerLearningDelivery[]
                        {
                            new MessageLearnerLearningDelivery
                            {
                                AimType = 1,
                                LearnAimRef = "AimRef",
                                FundModel = 36,
                                AimSeqNumber = 1,
                                CompStatus = 2,
                                FworkCodeSpecified = true, 
                                FworkCode = 1,
                                LearnActEndDateSpecified = true,
                                LearnActEndDate = new DateTime(2018, 8, 1),
                                LearnPlanEndDate = new DateTime(2018, 8, 1),
                                LearnStartDate = new DateTime(2018, 8, 1),
                                OrigLearnStartDateSpecified = true,
                                OrigLearnStartDate = new DateTime(2018, 8, 1),
                                OtherFundAdjSpecified = true,
                                OtherFundAdj = 3,
                                OutcomeSpecified = true,
                                Outcome = 4,
                                PriorLearnFundAdjSpecified = true,
                                PriorLearnFundAdj = 5,
                                ProgTypeSpecified = true,
                                ProgType = 25,
                                PwayCodeSpecified = true, 
                                PwayCode = 1,
                                StdCodeSpecified = true,
                                StdCode = 1,
                                LearningDeliveryFAM = new MessageLearnerLearningDeliveryLearningDeliveryFAM[]
                                {
                                    new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                    {
                                        LearnDelFAMCode = "1",
                                        LearnDelFAMType = "EEF"
                                    },
                                    new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                    {
                                        LearnDelFAMCode = "1",
                                        LearnDelFAMType = "LDM"
                                    },
                                    new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                    {
                                        LearnDelFAMCode = "2",
                                        LearnDelFAMType = "LDM"
                                    },
                                    new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                    {
                                        LearnDelFAMCode = "3",
                                        LearnDelFAMType = "LDM"
                                    }
                                },
                                AppFinRecord = new MessageLearnerLearningDeliveryAppFinRecord[]
                                {
                                    new MessageLearnerLearningDeliveryAppFinRecord
                                    {
                                        AFinAmount = 1,
                                        AFinCode = 1,
                                        AFinDate = new DateTime(2018, 8, 1),
                                        AFinType = "PMR"
                                    },
                                    new MessageLearnerLearningDeliveryAppFinRecord
                                    {
                                        AFinAmount = 1,
                                        AFinCode = 1,
                                        AFinDate = new DateTime(2018, 8, 1),
                                        AFinType = "TNP"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var expectedDto = new FM36LearnerDto
            {
                LearnRefNumber = "Learner_1",
                DateOfBirth = new DateTime(1990, 8, 1),
                PostcodePrior = "PostCodePrior",
                PMUKPRN = 1,
                PrevUKPRN = 2,
                ULN = 1,
                LearnerEmploymentStatuses = new List<LearnerEmploymentStatus>
                {
                    new LearnerEmploymentStatus
                    {
                        AgreeId = "AgreeId",
                        EmpId = 1,
                        DateEmpStatApp = new DateTime(2019, 8, 1),
                        EmpStat = 2,
                        SEM = 1
                    }
                },
                LearningDeliveries = new List<LearningDelivery>
                {
                    new LearningDelivery
                    {
                        AimType = 1,
                        LearnAimRef = "AimRef",
                        FundModel = 36,
                        AimSeqNumber = 1,
                        CompStatus = 2,
                        FworkCode = 1,
                        LearnActEndDate = new DateTime(2018, 8, 1),
                        LearnPlanEndDate = new DateTime(2018, 8, 1),
                        LearnStartDate = new DateTime(2018, 8, 1),
                        OrigLearnStartDate = new DateTime(2018, 8, 1),
                        OtherFundAdj = 3,
                        PriorLearnFundAdj = 5,
                        ProgType = 25,
                        PwayCode = 1,
                        StdCode = 1,
                        LearningDeliveryFAMs = new List<LearningDeliveryFAM>
                        {
                            new LearningDeliveryFAM
                            {
                                LearnDelFAMCode = "1",
                                LearnDelFAMType = "EEF"
                            },
                            new LearningDeliveryFAM
                            {
                                LearnDelFAMCode = "1",
                                LearnDelFAMType = "LDM"
                            },
                            new LearningDeliveryFAM
                            {
                                LearnDelFAMCode = "2",
                                LearnDelFAMType = "LDM"
                            },
                            new LearningDeliveryFAM
                            {
                                LearnDelFAMCode = "3",
                                LearnDelFAMType = "LDM"
                            }
                        },
                        AppFinRecords = new List<AppFinRecord>
                        {
                            new AppFinRecord
                            {
                                AFinAmount = 1,
                                AFinCode = 1,
                                AFinDate = new DateTime(2018, 8, 1),
                                AFinType = "PMR"
                            },
                            new AppFinRecord
                            {
                                AFinAmount = 1,
                                AFinCode = 1,
                                AFinDate = new DateTime(2018, 8, 1),
                                AFinType = "TNP"
                            }
                        }
                    }
                }
            };

            NewService().ProvideDtos(36, message).First().Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public void ProvideDtos_DtoAsExpected_NoLDFAMs()
        {
            IMessage message = new Message
            {
                LearningProvider = new MessageLearningProvider
                {
                    UKPRN = 12345678
                },
                Learner = new MessageLearner[]
                {
                    new MessageLearner
                    {
                        LearnRefNumber = "Learner_1",
                        DateOfBirthSpecified = true,
                        DateOfBirth = new DateTime(1990, 8, 1),
                        PostcodePrior = "PostCodePrior",
                        PMUKPRNSpecified = true,
                        PMUKPRN = 1,
                        PrevUKPRNSpecified = true,
                        PrevUKPRN = 2,
                        ULN = 1,
                        LearnerEmploymentStatus = new MessageLearnerLearnerEmploymentStatus[]
                        {
                            new MessageLearnerLearnerEmploymentStatus
                            {
                                EmpIdSpecified = true,
                                EmpId = 1,
                                AgreeId = "AgreeId",
                                DateEmpStatApp = new DateTime(2019, 8, 1),
                                EmpStat = 2,
                                EmploymentStatusMonitoring = new MessageLearnerLearnerEmploymentStatusEmploymentStatusMonitoring[]
                                {
                                    new MessageLearnerLearnerEmploymentStatusEmploymentStatusMonitoring
                                    {
                                        ESMType = "SEM",
                                        ESMCode = 1
                                    },
                                    new MessageLearnerLearnerEmploymentStatusEmploymentStatusMonitoring
                                    {
                                        ESMType = "HNS",
                                        ESMCode = 1
                                    }
                                }
                            }
                        },
                        LearningDelivery = new MessageLearnerLearningDelivery[]
                        {
                            new MessageLearnerLearningDelivery
                            {
                                AimType = 1,
                                LearnAimRef = "AimRef",
                                FundModel = 36,
                                AimSeqNumber = 1,
                                CompStatus = 2,
                                FworkCodeSpecified = true,
                                FworkCode = 1,
                                LearnActEndDateSpecified = true,
                                LearnActEndDate = new DateTime(2018, 8, 1),
                                LearnPlanEndDate = new DateTime(2018, 8, 1),
                                LearnStartDate = new DateTime(2018, 8, 1),
                                OrigLearnStartDateSpecified = true,
                                OrigLearnStartDate = new DateTime(2018, 8, 1),
                                OtherFundAdjSpecified = true,
                                OtherFundAdj = 3,
                                OutcomeSpecified = true,
                                Outcome = 4,
                                PriorLearnFundAdjSpecified = true,
                                PriorLearnFundAdj = 5,
                                ProgTypeSpecified = true,
                                ProgType = 25,
                                PwayCodeSpecified = true,
                                PwayCode = 1,
                                StdCodeSpecified = true,
                                StdCode = 1,
                                AppFinRecord = new MessageLearnerLearningDeliveryAppFinRecord[]
                                {
                                    new MessageLearnerLearningDeliveryAppFinRecord
                                    {
                                        AFinAmount = 1,
                                        AFinCode = 1,
                                        AFinDate = new DateTime(2018, 8, 1),
                                        AFinType = "PMR"
                                    },
                                    new MessageLearnerLearningDeliveryAppFinRecord
                                    {
                                        AFinAmount = 1,
                                        AFinCode = 1,
                                        AFinDate = new DateTime(2018, 8, 1),
                                        AFinType = "TNP"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var expectedDto = new FM36LearnerDto
            {
                LearnRefNumber = "Learner_1",
                DateOfBirth = new DateTime(1990, 8, 1),
                PostcodePrior = "PostCodePrior",
                PMUKPRN = 1,
                PrevUKPRN = 2,
                ULN = 1,
                LearnerEmploymentStatuses = new List<LearnerEmploymentStatus>
                {
                    new LearnerEmploymentStatus
                    {
                        AgreeId = "AgreeId",
                        EmpId = 1,
                        DateEmpStatApp = new DateTime(2019, 8, 1),
                        EmpStat = 2,
                        SEM = 1
                    }
                },
                LearningDeliveries = new List<LearningDelivery>
                {
                    new LearningDelivery
                    {
                        AimType = 1,
                        LearnAimRef = "AimRef",
                        FundModel = 36,
                        AimSeqNumber = 1,
                        CompStatus = 2,
                        FworkCode = 1,
                        LearnActEndDate = new DateTime(2018, 8, 1),
                        LearnPlanEndDate = new DateTime(2018, 8, 1),
                        LearnStartDate = new DateTime(2018, 8, 1),
                        OrigLearnStartDate = new DateTime(2018, 8, 1),
                        OtherFundAdj = 3,
                        PriorLearnFundAdj = 5,
                        ProgType = 25,
                        PwayCode = 1,
                        StdCode = 1,
                        AppFinRecords = new List<AppFinRecord>
                        {
                            new AppFinRecord
                            {
                                AFinAmount = 1,
                                AFinCode = 1,
                                AFinDate = new DateTime(2018, 8, 1),
                                AFinType = "PMR"
                            },
                            new AppFinRecord
                            {
                                AFinAmount = 1,
                                AFinCode = 1,
                                AFinDate = new DateTime(2018, 8, 1),
                                AFinType = "TNP"
                            }
                        }
                    }
                }
            };

            NewService().ProvideDtos(36, message).First().Should().BeEquivalentTo(expectedDto);
        }

        private IEnumerable<MessageLearner> BuildLearners(int numberOfLearners)
        {
            var learners = new List<MessageLearner>();

            while (learners.Count() < numberOfLearners)
            {
                learners.Add(
                    new MessageLearner
                    {
                        LearnRefNumber = "Learner_" + learners.Count().ToString(),
                        LearningDelivery = new MessageLearnerLearningDelivery[]
                        {
                            new MessageLearnerLearningDelivery
                            {
                                LearnAimRef = "AimRef",
                                FundModel = 36
                            }
                        }
                    });
            }

            return learners;
        }

        private FM36LearnerPagingService NewService()
        {
            return new FM36LearnerPagingService();
        }
    }
}
