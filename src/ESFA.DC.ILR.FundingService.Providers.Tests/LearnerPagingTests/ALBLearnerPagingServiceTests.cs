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
    public class ALBLearnerPagingServiceTests
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

            NewService().ProvideDtos(99, message).Should().HaveCount(1);
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

            NewService().ProvideDtos(99, message).Should().HaveCount(4);
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
                        LearningDelivery = new MessageLearnerLearningDelivery[]
                        {
                            new MessageLearnerLearningDelivery
                            {
                                LearnAimRef = "AimRef",
                                FundModel = 99,
                                AimSeqNumber = 1,
                                CompStatus = 2,
                                DelLocPostCode = "DelLocPostCode",
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
                                LearningDeliveryFAM = new MessageLearnerLearningDeliveryLearningDeliveryFAM[]
                                {
                                    new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                    {
                                        LearnDelFAMCode = "1",
                                        LearnDelFAMType = "ADL"
                                    },
                                    new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                    {
                                        LearnDelFAMCode = "1",
                                        LearnDelFAMType = "RES"
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
                }
            };

            var expectedDto = new ALBLearnerDto
            {
                LearnRefNumber = "Learner_1",
                LearningDeliveries = new List<LearningDelivery>
                {
                    new LearningDelivery
                    {
                        LearnAimRef = "AimRef",
                        FundModel = 99,
                        AimSeqNumber = 1,
                        CompStatus = 2,
                        DelLocPostCode = "DelLocPostCode",
                        LearnActEndDate = new DateTime(2018, 8, 1),
                        LearnPlanEndDate = new DateTime(2018, 8, 1),
                        LearnStartDate = new DateTime(2018, 8, 1),
                        OrigLearnStartDate = new DateTime(2018, 8, 1),
                        OtherFundAdj = 3,
                        Outcome = 4,
                        PriorLearnFundAdj = 5,
                        LearningDeliveryFAMs = new List<LearningDeliveryFAM>
                        {
                            new LearningDeliveryFAM
                            {
                                LearnDelFAMCode = "1",
                                LearnDelFAMType = "ADL"
                            },
                            new LearningDeliveryFAM
                            {
                                LearnDelFAMCode = "1",
                                LearnDelFAMType = "RES"
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

            NewService().ProvideDtos(99, message).First().Should().BeEquivalentTo(expectedDto);
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
                        LearningDelivery = new MessageLearnerLearningDelivery[]
                        {
                            new MessageLearnerLearningDelivery
                            {
                                LearnAimRef = "AimRef",
                                FundModel = 99,
                                AimSeqNumber = 1,
                                CompStatus = 2,
                                DelLocPostCode = "DelLocPostCode",
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
                            }
                        }
                    }
                }
            };

            var expectedDto = new ALBLearnerDto
            {
                LearnRefNumber = "Learner_1",
                LearningDeliveries = new List<LearningDelivery>
                {
                    new LearningDelivery
                    {
                        LearnAimRef = "AimRef",
                        FundModel = 99,
                        AimSeqNumber = 1,
                        CompStatus = 2,
                        DelLocPostCode = "DelLocPostCode",
                        LearnActEndDate = new DateTime(2018, 8, 1),
                        LearnPlanEndDate = new DateTime(2018, 8, 1),
                        LearnStartDate = new DateTime(2018, 8, 1),
                        OrigLearnStartDate = new DateTime(2018, 8, 1),
                        OtherFundAdj = 3,
                        Outcome = 4,
                        PriorLearnFundAdj = 5,
                    }
                }
            };

            NewService().ProvideDtos(99, message).First().Should().BeEquivalentTo(expectedDto);
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
                                FundModel = 99
                            }
                        }
                    });
            }

            return learners;
        }

        private ALBLearnerPagingService NewService()
        {
            return new ALBLearnerPagingService();
        }
    }
}
