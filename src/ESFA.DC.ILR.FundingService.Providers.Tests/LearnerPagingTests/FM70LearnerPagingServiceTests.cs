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
    public class FM70LearnerPagingServiceTests
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

            NewService().ProvideDtos(70, message).Should().HaveCount(1);
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

            NewService().ProvideDtos(70, message).Should().HaveCount(4);
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
                        LearningDelivery = new MessageLearnerLearningDelivery[]
                        {
                            new MessageLearnerLearningDelivery
                            {
                                AchDateSpecified = true, 
                                AchDate = new DateTime(2018, 8, 1),
                                AddHoursSpecified = true, 
                                AddHours = 1,
                                LearnAimRef = "AimRef",
                                FundModel = 70,
                                AimSeqNumber = 1,
                                CompStatus = 2,
                                ConRefNumber = "ConRef",
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

            var expectedDto = new FM70LearnerDto
            {
                LearnRefNumber = "Learner_1",
                DateOfBirth = new DateTime(1990, 8, 1),
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
                        AchDate = new DateTime(2018, 8, 1), 
                        AddHours = 1,
                        LearnAimRef = "AimRef",
                        FundModel = 70,
                        AimSeqNumber = 1,
                        CompStatus = 2,
                        ConRefNumber = "ConRef",
                        LearnActEndDate = new DateTime(2018, 8, 1),
                        LearnPlanEndDate = new DateTime(2018, 8, 1),
                        LearnStartDate = new DateTime(2018, 8, 1),
                        OrigLearnStartDate = new DateTime(2018, 8, 1),
                        OtherFundAdj = 3,
                        Outcome = 4,
                        PriorLearnFundAdj = 5,
                        LrnDelFAM_RES = "1",
                        LrnDelFAM_LDM1 = "1",
                        LrnDelFAM_LDM2 = "2",
                        LrnDelFAM_LDM3 = "3",
                    }
                }
            };

            NewService().ProvideDtos(70, message).First().Should().BeEquivalentTo(expectedDto);
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
                                FundModel = 70
                            }
                        }
                    });
            }

            return learners;
        }

        private FM70LearnerPagingService NewService()
        {
            return new FM70LearnerPagingService();
        }
    }
}
