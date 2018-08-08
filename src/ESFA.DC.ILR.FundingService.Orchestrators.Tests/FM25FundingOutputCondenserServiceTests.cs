using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Output;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Tests
{
    public class FundingOutputCondenserServiceTests
    {
        [Fact]
        public void Condense()
        {
            var learnerOne = new Learner();
            var learnerTwo = new Learner();
            var learnerThree = new Learner();
            var learnerFour = new Learner();
            var learnerFive = new Learner();
            var learnerSix = new Learner();

            var globalOne = new Global()
            {
                Learners = new List<Learner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new Global()
            {
                Learners = new List<Learner>
                {
                    learnerThree,
                    learnerFour,
                },
            };

            var globalThree = new Global()
            {
                Learners = new List<Learner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<Global>()
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
            var globalOne = new Global()
            {
                Learners = null
            };

            var globalTwo = new Global()
            {
                Learners = null
            };

            var globalThree = new Global()
            {
                Learners = null
            };

            var fundingOutputs = new List<Global>()
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
            var learnerOne = new Learner();
            var learnerTwo = new Learner();
            var learnerFive = new Learner();
            var learnerSix = new Learner();

            var globalOne = new Global()
            {
                Learners = new List<Learner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new Global()
            {
                Learners = null,
            };

            var globalThree = new Global()
            {
                Learners = new List<Learner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<Global>()
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

        private FM25FundingOutputCondenserService NewService()
        {
            return new FM25FundingOutputCondenserService();
        }
    }
}
