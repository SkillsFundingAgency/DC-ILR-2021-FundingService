using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class LARSDataRetrievalServiceTests
    {
        [Fact]
        public void LarsFundings()
        {
            var larsMock = new Mock<ILARS>();

            var larsFundings = NewService(larsMock.Object).LARSFundings;

            larsMock.VerifyGet(p => p.LARS_Funding);
        }

        [Fact]
        public void LarsLearningDeliveries()
        {
            var larsMock = new Mock<ILARS>();

            var larsLearningDeliveries = NewService(larsMock.Object).LARSLearningDeliveries;

            larsMock.VerifyGet(l => l.LARS_LearningDelivery);
        }

        [Fact]
        public void LarsVersions()
        {
            var larsMock = new Mock<ILARS>();

            var larsVersions = NewService(larsMock.Object).LARSVersions;

            larsMock.VerifyGet(l => l.LARS_Version);
        }

        [Fact]
        public void UniqueLearnAimRefs()
        {
            var message = new TestMessage()
            {
                Learners = new List<TestLearner>()
                {
                    new TestLearner()
                    {
                        LearningDeliveries = new List<TestLearningDelivery>()
                        {
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "one",
                            },
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "two",
                            },
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "one",
                            }
                        },

                    },
                    new TestLearner()
                    {
                        LearningDeliveries = new List<TestLearningDelivery>()
                        {
                            new TestLearningDelivery()
                            {
                                LearnAimRef = "one",
                            }
                        }
                    },
                    new TestLearner(),
                }
            };

            var uniqueLearnAimRefs = NewService().UniqueLearnAimRefs(message).ToList();

            uniqueLearnAimRefs.Should().HaveCount(2);
            uniqueLearnAimRefs.Should().Contain(new List<string>() { "one", "two" });
        }

        [Fact]
        public void LARSFundingsForLearnAimRefs()
        {
            var lars_Fundings = new List<LARS_Funding>()
            {
                new LARS_Funding()
                {
                    LearnAimRef = "123",
                },
                new LARS_Funding()
                {
                    LearnAimRef = "456",
                },
                new LARS_Funding()
                {
                    LearnAimRef = "123",
                },
                new LARS_Funding()
                {
                    LearnAimRef = "789",
                },
                new LARS_Funding(),
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSFundings).Returns(lars_Fundings);

            var learnAimRefs = new List<string>() { "123", "456", "234" };

            var larsFundings = larsDataRetrievalServiceMock.Object.LARSFundingsForLearnAimRefs(learnAimRefs);

            larsFundings.Should().HaveCount(2);
            larsFundings.Should().ContainKeys("123", "456");
            larsFundings["123"].Should().HaveCount(2);
            larsFundings["456"].Should().HaveCount(1);
        }

        [Fact]
        public void LARSFundingFromEntity()
        {
            var lars_Funding = new LARS_Funding()
            {
                EffectiveFrom = new DateTime(2017, 1, 1),
                EffectiveTo = new DateTime(2018, 1, 1),
                FundingCategory = "FC",
                LearnAimRef = "LearnAimRef",
                RateUnWeighted = 1.2m,
                RateWeighted = 1.3m,
                WeightingFactor = "WF",
            };

            var larsFunding = NewService().LARSFundingFromEntity(lars_Funding);

            larsFunding.EffectiveFrom.Should().Be(lars_Funding.EffectiveFrom);
            larsFunding.EffectiveTo.Should().Be(lars_Funding.EffectiveTo);
            larsFunding.FundingCategory.Should().Be(lars_Funding.FundingCategory);
            larsFunding.LearnAimRef.Should().Be(lars_Funding.LearnAimRef);
            larsFunding.RateUnWeighted.Should().Be(lars_Funding.RateUnWeighted);
            larsFunding.RateWeighted.Should().Be(lars_Funding.RateWeighted);
            larsFunding.WeightingFactor.Should().Be(lars_Funding.WeightingFactor);
        }

        [Fact]
        public void LARSLearningDeliveryForLearnAimRefs()
        {
            var lars_LearningDeliveries = new List<LARS_LearningDelivery>()
            {
                new LARS_LearningDelivery()
                {
                    LearnAimRef = "123",
                },
                new LARS_LearningDelivery()
                {
                    LearnAimRef = "456",
                },
                new LARS_LearningDelivery()
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSLearningDeliveries).Returns(lars_LearningDeliveries);

            var learnAimRefs = new List<string>() { "123", "456" };

            var larsLearningDeliveries = larsDataRetrievalServiceMock.Object.LARSLearningDeliveriesForLearnAimRefs(learnAimRefs);

            larsLearningDeliveries.Should().HaveCount(2);
            larsLearningDeliveries.Should().ContainKeys("123", "456");
        }

        [Fact]
        public void LarsLearningDeliveryFromEntity()
        {
            var lars_LearningDelivery = new LARS_LearningDelivery()
            {
                EnglPrscID = 1,
                EnglandFEHEStatus = "englandFEHEStatus",
                FrameworkCommonComponent = 2,
                LearnAimRef = "learnAimRef",
                LearnAimRefType = "learnAimRefType",
                NotionalNVQLevelv2 = "notionalNVQLevelv2",
                RegulatedCreditValue = 3,
            };

            var larsLearningDelivery = NewService().LARSLearningDeliveryFromEntity(lars_LearningDelivery);

            larsLearningDelivery.EnglPrscID.Should().Be(lars_LearningDelivery.EnglPrscID);
            larsLearningDelivery.EnglandFEHEStatus.Should().Be(lars_LearningDelivery.EnglandFEHEStatus);
            larsLearningDelivery.FrameworkCommonComponent.Should().Be(lars_LearningDelivery.FrameworkCommonComponent);
            larsLearningDelivery.LearnAimRef.Should().Be(lars_LearningDelivery.LearnAimRef);
            larsLearningDelivery.LearnAimRefType.Should().Be(lars_LearningDelivery.LearnAimRefType);
            larsLearningDelivery.NotionalNVQLevelv2.Should().Be(lars_LearningDelivery.NotionalNVQLevelv2);
            larsLearningDelivery.RegulatedCreditValue.Should().Be(lars_LearningDelivery.RegulatedCreditValue);
        }

        [Fact]
        public void CurrentVersion()
        {
            var versions = new List<LARS_Version>()
            {
                new LARS_Version() { MainDataSchemaName = "001" },
                new LARS_Version() { MainDataSchemaName = "002" },
                new LARS_Version() { MainDataSchemaName = "003" }
            }.AsQueryable();

            var larsDataRetrievalServiceMock = NewMock();

            larsDataRetrievalServiceMock.SetupGet(l => l.LARSVersions).Returns(versions);

            larsDataRetrievalServiceMock.Object.CurrentVersion().Should().Be("003");
        }

        private LARSDataRetrievalService NewService(ILARS lars = null)
        {
            return  new LARSDataRetrievalService(lars);
        }

        private Mock<LARSDataRetrievalService> NewMock()
        {
            return new Mock<LARSDataRetrievalService>();
        }
    }
}
