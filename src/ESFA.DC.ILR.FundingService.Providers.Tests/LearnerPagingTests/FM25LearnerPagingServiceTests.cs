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
    public class FM25LearnerPagingServiceTests
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

            NewService().ProvideDtos(25, message).Should().HaveCount(1);
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

            NewService().ProvideDtos(25, message).Should().HaveCount(4);
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
        public void ProvideDtos_NoLearners()
        {
            IMessage message = new Message
            {
                LearningProvider = new MessageLearningProvider
                {
                    UKPRN = 12345678
                },
            };

            NewService().ProvideDtos(25, message).Should().HaveCount(0);
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
                        EngGrade = "1",
                        MathGrade = "2",
                        PlanEEPHoursSpecified = true, 
                        PlanEEPHours = 1, 
                        PlanLearnHoursSpecified = true, 
                        PlanLearnHours = 1,
                        Postcode = "Postcode",
                        ULN = 1000,
                        LearnerFAM = new MessageLearnerLearnerFAM[]
                        {
                            new MessageLearnerLearnerFAM
                            {
                                LearnFAMCode = 1, 
                                LearnFAMType = "ECF"
                            },
                            new MessageLearnerLearnerFAM
                            {
                                LearnFAMCode = 1,
                                LearnFAMType = "EDF"
                            },
                            new MessageLearnerLearnerFAM
                            {
                                LearnFAMCode = 2,
                                LearnFAMType = "EDF"
                            },
                            new MessageLearnerLearnerFAM
                            {
                                LearnFAMCode = 1,
                                LearnFAMType = "EHC"
                            },
                            new MessageLearnerLearnerFAM
                            {
                                LearnFAMCode = 1,
                                LearnFAMType = "HNS"
                            },
                        },
                        LearningDelivery = new MessageLearnerLearningDelivery[]
                        {
                            new MessageLearnerLearningDelivery
                            {
                                LearnAimRef = "AimRef",
                                AimType = 1,
                                FundModel = 25,
                                AimSeqNumber = 1,
                                CompStatus = 2,
                                LearnActEndDateSpecified = true,
                                LearnActEndDate = new DateTime(2018, 8, 1),
                                LearnPlanEndDate = new DateTime(2018, 8, 1),
                                LearnStartDate = new DateTime(2018, 8, 1),
                                ProgTypeSpecified = true, 
                                ProgType = 1,
                                WithdrawReasonSpecified = true, 
                                WithdrawReason = 1,
                                LearningDeliveryFAM = new MessageLearnerLearningDeliveryLearningDeliveryFAM[]
                                {
                                    new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                    {
                                        LearnDelFAMCode = "1",
                                        LearnDelFAMType = "SOF"
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
                                }
                            }
                        }
                    }
                },
                LearnerDestinationandProgression = new MessageLearnerDestinationandProgression[]
                {
                    new MessageLearnerDestinationandProgression
                    {
                        LearnRefNumber = "Learner_1",
                        DPOutcome = new MessageLearnerDestinationandProgressionDPOutcome[]
                        {
                           new MessageLearnerDestinationandProgressionDPOutcome
                           {
                               OutCode = 1,
                               OutType = "Type"
                           },
                           new MessageLearnerDestinationandProgressionDPOutcome
                           {
                               OutCode = 2,
                               OutType = "Type"
                           }
                        }
                    }
                }
            };

            var expectedDto = new FM25LearnerDto
            {
                LearnRefNumber = "Learner_1",
                DateOfBirth = new DateTime(1990, 8, 1),
                EngGrade = "1",
                MathGrade = "2",
                PlanEEPHours = 1,
                PlanLearnHours = 1,
                Postcode = "Postcode",
                ULN = 1000,
                LrnFAM_ECF = 1,
                LrnFAM_EDF1 = 1,
                LrnFAM_EDF2 = 2,
                LrnFAM_EHC = 1,
                LrnFAM_HNS = 1,
                DPOutcomes = new List<DPOutcome>
                {
                    new DPOutcome
                    {
                        OutCode = 1,
                        OutType = "Type"
                    },
                    new DPOutcome
                    {
                        OutCode = 2,
                        OutType = "Type"
                    }
                },
                LearningDeliveries = new List<LearningDelivery>
                {
                    new LearningDelivery
                    {
                        LearnAimRef = "AimRef",
                        AimType = 1,
                        FundModel = 25,
                        AimSeqNumber = 1,
                        CompStatus = 2,
                        LearnActEndDate = new DateTime(2018, 8, 1),
                        LearnPlanEndDate = new DateTime(2018, 8, 1),
                        LearnStartDate = new DateTime(2018, 8, 1),
                        ProgType = 1,
                        WithdrawReason = 1,
                        LrnDelFAM_SOF = "1",
                        LrnDelFAM_LDM1 = "1",
                        LrnDelFAM_LDM2 = "2",
                        LrnDelFAM_LDM3 = "3",
                        LearningDeliveryFAMs = new List<LearningDeliveryFAM>
                        {
                            new LearningDeliveryFAM
                            {
                                LearnDelFAMCode = "1",
                                LearnDelFAMType = "SOF"
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
                        }
                    }
                }
            };

            NewService().ProvideDtos(25, message).First().Should().BeEquivalentTo(expectedDto);
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
                                FundModel = 25
                            }
                        }
                    });
            }

            return learners;
        }

        private FM25LearnerPagingService NewService()
        {
            return new FM25LearnerPagingService();
        }
    }
}
