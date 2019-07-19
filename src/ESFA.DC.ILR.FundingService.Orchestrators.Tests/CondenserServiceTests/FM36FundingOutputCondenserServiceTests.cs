using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Orchestrators.Output;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Tests.CondenserServiceTests
{
    public class FM36FundingOutputCondenserServiceTests
    {
        [Fact]
        public void Condense()
        {
            var learnerOne = new FM36Learner();
            var learnerTwo = new FM36Learner();
            var learnerThree = new FM36Learner();
            var learnerFour = new FM36Learner();
            var learnerFive = new FM36Learner();
            var learnerSix = new FM36Learner();

            var globalOne = new FM36Global()
            {
                Learners = new List<FM36Learner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new FM36Global()
            {
                Learners = new List<FM36Learner>
                {
                    learnerThree,
                    learnerFour,
                },
            };

            var globalThree = new FM36Global()
            {
                Learners = new List<FM36Learner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<FM36Global>()
            {
                globalOne,
                globalTwo,
                globalThree,
            };

            var fundingOutput = NewService().Condense(fundingOutputs, 1, "1920");

            fundingOutput.Should().Be(globalOne);
            fundingOutput.Learners.Should().HaveCount(6);
            fundingOutput.Learners.Should().Contain(new[] { learnerOne, learnerTwo, learnerThree, learnerFour, learnerFive, learnerSix });
        }

        [Fact]
        public void Condense_Null()
        {
            Action action = () => NewService().Condense(null, 1, "1920");

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Condense_EmptyCollection()
        {
            var global = new FM36Global
            {
                UKPRN = 1,
                Year = "1920"
            };

            NewService().Condense(Enumerable.Empty<FM36Global>(), 1, "1920").Should().BeEquivalentTo(global);
        }

        [Fact]
        public void Condense_NullLearners()
        {
            var globalOne = new FM36Global()
            {
                Learners = null
            };

            var globalTwo = new FM36Global()
            {
                Learners = null
            };

            var globalThree = new FM36Global()
            {
                Learners = null
            };

            var fundingOutputs = new List<FM36Global>()
            {
                globalOne,
                globalTwo,
                globalThree,
            };

            var fundingOutput = NewService().Condense(fundingOutputs, 1, "1920");

            fundingOutput.Should().Be(globalOne);
            fundingOutput.Learners.Should().BeEmpty();
        }

        [Fact]
        public void Condense_MixedNullLearners()
        {
            var learnerOne = new FM36Learner();
            var learnerTwo = new FM36Learner();
            var learnerFive = new FM36Learner();
            var learnerSix = new FM36Learner();

            var globalOne = new FM36Global()
            {
                Learners = new List<FM36Learner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new FM36Global()
            {
                Learners = null,
            };

            var globalThree = new FM36Global()
            {
                Learners = new List<FM36Learner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<FM36Global>()
            {
                globalOne,
                globalTwo,
                globalThree,
            };

            var fundingOutput = NewService().Condense(fundingOutputs, 1, "1920");

            fundingOutput.Should().Be(globalOne);
            fundingOutput.Learners.Should().HaveCount(4);
            fundingOutput.Learners.Should().Contain(new[] { learnerOne, learnerTwo, learnerFive, learnerSix });
        }


        private FM36FundingOutputCondenserService NewService()
        {
            return new FM36FundingOutputCondenserService();
        }
    }
}
