using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Orchestrators.Output;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Tests.CondenserServiceTests
{
    public class FM35FundingOutputCondenserServiceTests
    {
        [Fact]
        public void Condense()
        {
            var learnerOne = new FM35Learner();
            var learnerTwo = new FM35Learner();
            var learnerThree = new FM35Learner();
            var learnerFour = new FM35Learner();
            var learnerFive = new FM35Learner();
            var learnerSix = new FM35Learner();

            var globalOne = new FM35Global()
            {
                Learners = new List<FM35Learner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new FM35Global()
            {
                Learners = new List<FM35Learner>
                {
                    learnerThree,
                    learnerFour,
                },
            };

            var globalThree = new FM35Global()
            {
                Learners = new List<FM35Learner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<FM35Global>()
            {
                globalOne,
                globalTwo,
                globalThree,
            };

            var fundingOutput = NewService().Condense(fundingOutputs);

            fundingOutput.Should().Be(globalOne);
            fundingOutput.Learners.Should().HaveCount(6);
            fundingOutput.Learners.Should().Contain(new[] { learnerOne, learnerTwo, learnerThree, learnerFour, learnerFive, learnerSix });
        }

        [Fact]
        public void Condense_Null()
        {
            Action action = () => NewService().Condense(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Condense_NullLearners()
        {
            var globalOne = new FM35Global()
            {
                Learners = null
            };

            var globalTwo = new FM35Global()
            {
                Learners = null
            };

            var globalThree = new FM35Global()
            {
                Learners = null
            };

            var fundingOutputs = new List<FM35Global>()
            {
                globalOne,
                globalTwo,
                globalThree,
            };

            var fundingOutput = NewService().Condense(fundingOutputs);

            fundingOutput.Should().Be(globalOne);
            fundingOutput.Learners.Should().BeEmpty();
        }

        [Fact]
        public void Condense_MixedNullLearners()
        {
            var learnerOne = new FM35Learner();
            var learnerTwo = new FM35Learner();
            var learnerFive = new FM35Learner();
            var learnerSix = new FM35Learner();

            var globalOne = new FM35Global()
            {
                Learners = new List<FM35Learner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new FM35Global()
            {
                Learners = null,
            };

            var globalThree = new FM35Global()
            {
                Learners = new List<FM35Learner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<FM35Global>()
            {
                globalOne,
                globalTwo,
                globalThree,
            };

            var fundingOutput = NewService().Condense(fundingOutputs);

            fundingOutput.Should().Be(globalOne);
            fundingOutput.Learners.Should().HaveCount(4);
            fundingOutput.Learners.Should().Contain(new[] { learnerOne, learnerTwo, learnerFive, learnerSix });
        }


        private FM35FundingOutputCondenserService NewService()
        {
            return new FM35FundingOutputCondenserService();
        }
    }
}
