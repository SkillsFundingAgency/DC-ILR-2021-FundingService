﻿using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.Orchestrators.Output;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Orchestrators.Tests.CondenserServiceTests
{
    public class FundingOutputCondenserServiceTests
    {
        [Fact]
        public void Condense()
        {
            var learnerOne = new FM25Learner();
            var learnerTwo = new FM25Learner();
            var learnerThree = new FM25Learner();
            var learnerFour = new FM25Learner();
            var learnerFive = new FM25Learner();
            var learnerSix = new FM25Learner();

            var globalOne = new FM25Global()
            {
                Learners = new List<FM25Learner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new FM25Global()
            {
                Learners = new List<FM25Learner>
                {
                    learnerThree,
                    learnerFour,
                },
            };

            var globalThree = new FM25Global()
            {
                Learners = new List<FM25Learner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<FM25Global>()
            {
                globalOne,
                globalTwo,
                globalThree,
            };

            var fundingOutput = NewService().Condense(fundingOutputs, 1, "2021");

            fundingOutput.Should().Be(globalOne);
            fundingOutput.Learners.Should().HaveCount(6);
            fundingOutput.Learners.Should().Contain(new[] { learnerOne, learnerTwo, learnerThree, learnerFour, learnerFive, learnerSix });
        }

        [Fact]
        public void Condense_Null()
        {
            Action action = () => NewService().Condense(null, 1, "2021");

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Condense_EmptyCollection()
        {
            var global = new FM25Global
            {
                UKPRN = 1
            };

            NewService().Condense(Enumerable.Empty<FM25Global>(), 1, "2021").Should().BeEquivalentTo(global);
        }

        [Fact]
        public void Condense_NullLearners()
        {
            var globalOne = new FM25Global()
            {
                Learners = null
            };

            var globalTwo = new FM25Global()
            {
                Learners = null
            };

            var globalThree = new FM25Global()
            {
                Learners = null
            };

            var fundingOutputs = new List<FM25Global>()
            {
                globalOne,
                globalTwo,
                globalThree,
            };

            var fundingOutput = NewService().Condense(fundingOutputs, 1, "2021");

            fundingOutput.Should().Be(globalOne);
            fundingOutput.Learners.Should().BeEmpty();
        }

        [Fact]
        public void Condense_MixedNullLearners()
        {
            var learnerOne = new FM25Learner();
            var learnerTwo = new FM25Learner();
            var learnerFive = new FM25Learner();
            var learnerSix = new FM25Learner();

            var globalOne = new FM25Global()
            {
                Learners = new List<FM25Learner>()
                {
                    learnerOne,
                    learnerTwo,
                },
            };

            var globalTwo = new FM25Global()
            {
                Learners = null,
            };

            var globalThree = new FM25Global()
            {
                Learners = new List<FM25Learner>
                {
                    learnerFive,
                    learnerSix,
                },
            };

            var fundingOutputs = new List<FM25Global>()
            {
                globalOne,
                globalTwo,
                globalThree,
            };

            var fundingOutput = NewService().Condense(fundingOutputs, 1, "2021");

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
