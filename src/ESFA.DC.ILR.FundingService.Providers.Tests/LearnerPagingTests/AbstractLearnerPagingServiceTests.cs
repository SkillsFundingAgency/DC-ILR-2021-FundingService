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
    public class AbstractLearnerPagingServiceTests
    {
        [Fact]
        public void BuildPages()
        {
            var learners = BuildLearners(10, 99).ToArray();

            NewService().BuildPages(99, learners).Should().HaveCount(1);
        }

        [Fact]
        public void BuildPages_NoLearners()
        {
            NewService().BuildPages(99, null).Should().HaveCount(0);
        }


        [Fact]
        public void BuildPages_MultiplePages()
        {
            var learners = BuildLearners(1600, 99).ToArray();

            NewService().BuildPages(99, learners).Should().HaveCount(4);
        }

        [Fact]
        public void BuildPages_FundModelMisMatch()
        {
            var learners = BuildLearners(10, 25).ToArray();

            NewService().BuildPages(99, learners).Should().HaveCount(0);
        }

        protected IEnumerable<MessageLearner> BuildLearners(int numberOfLearners, int fundModel)
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
                                FundModel = fundModel
                            }
                        }
                    });
            }

            return learners;
        }

        private AbstractLearnerPagingService NewService()
        {
            return new AbstractLearnerPagingService();
        }
    }
}
