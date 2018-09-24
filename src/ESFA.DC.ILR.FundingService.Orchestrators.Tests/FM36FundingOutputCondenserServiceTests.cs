using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.Orchestrators.Output;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Tests
{
    public class FM36FundingOutputCondenserServiceTests
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

            var globalOne = new FM36Global();
            var globalTwo = new FM36Global();
            var globalThree = new FM36Global();

            var fundingOutputs = new List<FM36FundingOutputs>()
            {
                new FM36FundingOutputs()
                {
                    Global = globalOne,
                    Learners = new Learner[]
                    {
                        learnerOne,
                        learnerTwo,
                    },
                },
                new FM36FundingOutputs()
                {
                    Global = globalTwo,
                    Learners = new Learner[]
                    {
                        learnerThree,
                        learnerFour,
                    },
                },
                new FM36FundingOutputs()
                {
                    Global = globalThree,
                    Learners = new Learner[]
                    {
                        learnerFive,
                        learnerSix,
                    },
                },
            };

            var fundingOutput = NewService().Condense(fundingOutputs);

            fundingOutput.Global.Should().Be(globalOne);
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
        public void Condense_NullGlobal()
        {
            var learnerOne = new Learner();
            var learnerTwo = new Learner();
            var learnerThree = new Learner();
            var learnerFour = new Learner();
            var learnerFive = new Learner();
            var learnerSix = new Learner();

            var fundingOutputs = new List<FM36FundingOutputs>()
            {
                new FM36FundingOutputs()
                {
                    Global = null,
                    Learners = new Learner[]
                    {
                        learnerOne,
                        learnerTwo,
                    },
                },
                new FM36FundingOutputs()
                {
                    Global = null,
                    Learners = new Learner[]
                    {
                        learnerThree,
                        learnerFour,
                    },
                },
                new FM36FundingOutputs()
                {
                    Global = null,
                    Learners = new Learner[]
                    {
                        learnerFive,
                        learnerSix,
                    },
                },
            };

            var fundingOutput = NewService().Condense(fundingOutputs);

            fundingOutput.Global.Should().BeNull();
            fundingOutput.Learners.Should().HaveCount(6);
            fundingOutput.Learners.Should().Contain(new[] { learnerOne, learnerTwo, learnerThree, learnerFour, learnerFive, learnerSix });
        }

        [Fact]
        public void Condense_NullLearners()
        {
            var globalOne = new FM36Global();
            var globalTwo = new FM36Global();
            var globalThree = new FM36Global();

            var fundingOutputs = new List<FM36FundingOutputs>()
            {
                new FM36FundingOutputs()
                {
                    Global = globalOne,
                    Learners = null
                },
                new FM36FundingOutputs()
                {
                    Global = globalTwo,
                    Learners = null
                },
                new FM36FundingOutputs()
                {
                    Global = globalThree,
                    Learners = null
                },
            };

            var fundingOutput = NewService().Condense(fundingOutputs);

            fundingOutput.Global.Should().Be(globalOne);
            fundingOutput.Learners.Should().BeEmpty();
        }

        [Fact]
        public void Condense_MixedNullLearners()
        {
            var learnerOne = new Learner();
            var learnerTwo = new Learner();
            var learnerThree = new Learner();
            var learnerFour = new Learner();
            var learnerFive = new Learner();
            var learnerSix = new Learner();

            var globalOne = new FM36Global();
            var globalTwo = new FM36Global();
            var globalThree = new FM36Global();

            var fundingOutputs = new List<FM36FundingOutputs>()
            {
                new FM36FundingOutputs()
                {
                    Global = globalOne,
                    Learners = new Learner[]
                    {
                        learnerOne,
                        learnerTwo,
                    },
                },
                new FM36FundingOutputs()
                {
                    Global = globalTwo,
                    Learners = null,
                },
                new FM36FundingOutputs()
                {
                    Global = globalThree,
                    Learners = new Learner[]
                    {
                        learnerFive,
                        learnerSix,
                    },
                },
            };

            var fundingOutput = NewService().Condense(fundingOutputs);

            fundingOutput.Global.Should().Be(globalOne);
            fundingOutput.Learners.Should().HaveCount(4);
            fundingOutput.Learners.Should().Contain(new[] { learnerOne, learnerTwo, learnerFive, learnerSix });
        }

        private FM36FundingOutputCondenserService NewService()
        {
            return new FM36FundingOutputCondenserService();
        }
    }
}
