using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Orchestrators.Output;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Tests.CondenserServiceTests
{
    public class ALBFundingOutputCondenserServiceTests
    {
        [Fact]
        public void Condense()
        {
            var learnerOne = new ALBLearner();
            var learnerTwo = new ALBLearner();
            var learnerThree = new ALBLearner();
            var learnerFour = new ALBLearner();
            var learnerFive = new ALBLearner();
            var learnerSix = new ALBLearner();

            var globalOne = new ALBGlobal()
            {
                Learners = new List<ALBLearner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new ALBGlobal()
            {
                Learners = new List<ALBLearner>
                {
                    learnerThree,
                    learnerFour,
                },
            };

            var globalThree = new ALBGlobal()
            {
                Learners = new List<ALBLearner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<ALBGlobal>()
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
            var globalOne = new ALBGlobal()
            {
                Learners = null
            };

            var globalTwo = new ALBGlobal()
            {
                Learners = null
            };

            var globalThree = new ALBGlobal()
            {
                Learners = null
            };

            var fundingOutputs = new List<ALBGlobal>()
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
            var learnerOne = new ALBLearner();
            var learnerTwo = new ALBLearner();
            var learnerFive = new ALBLearner();
            var learnerSix = new ALBLearner();

            var globalOne = new ALBGlobal()
            {
                Learners = new List<ALBLearner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new ALBGlobal()
            {
                Learners = null,
            };

            var globalThree = new ALBGlobal()
            {
                Learners = new List<ALBLearner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<ALBGlobal>()
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

        private ALBFundingOutputCondenserService NewService()
        {
            return new ALBFundingOutputCondenserService();
        }
    }
}
