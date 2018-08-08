using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Attribute;
using ESFA.DC.ILR.FundingService.Orchestrators.Output;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Tests
{
    public class FM35FundingOutputCondenserServiceTests
    {
        [Fact]
        public void Condense()
        {
            var learnerOne = new LearnerAttribute();
            var learnerTwo = new LearnerAttribute();
            var learnerThree = new LearnerAttribute();
            var learnerFour = new LearnerAttribute();
            var learnerFive = new LearnerAttribute();
            var learnerSix = new LearnerAttribute();

            var globalOne = new GlobalAttribute();
            var globalTwo = new GlobalAttribute();
            var globalThree = new GlobalAttribute();

            var fundingOutputs = new List<FM35FundingOutputs>()
            {
                new FM35FundingOutputs()
                {
                    Global = globalOne,
                    Learners = new LearnerAttribute[]
                    {
                        learnerOne,
                        learnerTwo,
                    },
                },
                new FM35FundingOutputs()
                {
                    Global = globalTwo,
                    Learners = new LearnerAttribute[]
                    {
                        learnerThree,
                        learnerFour,
                    },
                },
                new FM35FundingOutputs()
                {
                    Global = globalThree,
                    Learners = new LearnerAttribute[]
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
            var learnerOne = new LearnerAttribute();
            var learnerTwo = new LearnerAttribute();
            var learnerThree = new LearnerAttribute();
            var learnerFour = new LearnerAttribute();
            var learnerFive = new LearnerAttribute();
            var learnerSix = new LearnerAttribute();

            var fundingOutputs = new List<FM35FundingOutputs>()
            {
                new FM35FundingOutputs()
                {
                    Global = null,
                    Learners = new LearnerAttribute[]
                    {
                        learnerOne,
                        learnerTwo,
                    },
                },
                new FM35FundingOutputs()
                {
                    Global = null,
                    Learners = new LearnerAttribute[]
                    {
                        learnerThree,
                        learnerFour,
                    },
                },
                new FM35FundingOutputs()
                {
                    Global = null,
                    Learners = new LearnerAttribute[]
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
            var globalOne = new GlobalAttribute();
            var globalTwo = new GlobalAttribute();
            var globalThree = new GlobalAttribute();

            var fundingOutputs = new List<FM35FundingOutputs>()
            {
                new FM35FundingOutputs()
                {
                    Global = globalOne,
                    Learners = null
                },
                new FM35FundingOutputs()
                {
                    Global = globalTwo,
                    Learners = null
                },
                new FM35FundingOutputs()
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
            var learnerOne = new LearnerAttribute();
            var learnerTwo = new LearnerAttribute();
            var learnerThree = new LearnerAttribute();
            var learnerFour = new LearnerAttribute();
            var learnerFive = new LearnerAttribute();
            var learnerSix = new LearnerAttribute();

            var globalOne = new GlobalAttribute();
            var globalTwo = new GlobalAttribute();
            var globalThree = new GlobalAttribute();

            var fundingOutputs = new List<FM35FundingOutputs>()
            {
                new FM35FundingOutputs()
                {
                    Global = globalOne,
                    Learners = new LearnerAttribute[]
                    {
                        learnerOne,
                        learnerTwo,
                    },
                },
                new FM35FundingOutputs()
                {
                    Global = globalTwo,
                    Learners = null,
                },
                new FM35FundingOutputs()
                {
                    Global = globalThree,
                    Learners = new LearnerAttribute[]
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

        private FM35FundingOutputCondenserService NewService()
        {
            return new FM35FundingOutputCondenserService();
        }
    }
}
