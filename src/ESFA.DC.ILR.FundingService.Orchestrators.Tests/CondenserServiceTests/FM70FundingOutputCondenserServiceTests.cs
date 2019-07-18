using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Orchestrators.Output;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Tests.CondenserServiceTests
{
    public class FM70FundingOutputCondenserServiceTests
    {
        [Fact]
        public void Condense()
        {
            var learnerOne = new FM70Learner();
            var learnerTwo = new FM70Learner();
            var learnerThree = new FM70Learner();
            var learnerFour = new FM70Learner();
            var learnerFive = new FM70Learner();
            var learnerSix = new FM70Learner();

            var globalOne = new FM70Global()
            {
                Learners = new List<FM70Learner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new FM70Global()
            {
                Learners = new List<FM70Learner>
                {
                    learnerThree,
                    learnerFour,
                },
            };

            var globalThree = new FM70Global()
            {
                Learners = new List<FM70Learner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<FM70Global>()
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
            var global = new FM70Global
            {
                UKPRN = 1
            };

            NewService().Condense(Enumerable.Empty<FM70Global>(), 1, "1920").Should().BeEquivalentTo(global);
        }

        [Fact]
        public void Condense_NullLearners()
        {
            var globalOne = new FM70Global()
            {
                Learners = null
            };

            var globalTwo = new FM70Global()
            {
                Learners = null
            };

            var globalThree = new FM70Global()
            {
                Learners = null
            };

            var fundingOutputs = new List<FM70Global>()
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
            var learnerOne = new FM70Learner();
            var learnerTwo = new FM70Learner();
            var learnerFive = new FM70Learner();
            var learnerSix = new FM70Learner();

            var globalOne = new FM70Global()
            {
                Learners = new List<FM70Learner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new FM70Global()
            {
                Learners = null,
            };

            var globalThree = new FM70Global()
            {
                Learners = new List<FM70Learner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<FM70Global>()
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


        private FM70FundingOutputCondenserService NewService()
        {
            return new FM70FundingOutputCondenserService();
        }
    }
}
