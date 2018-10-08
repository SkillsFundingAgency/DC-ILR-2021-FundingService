using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.Tests.Model;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class FCSDataRetrievalServiceTests
    {
        [Fact]
        public void Contractors()
        {
            var fcsMock = new Mock<IFcsContext>();

            var fcs = NewService(fcsMock.Object).Contractors;

            fcsMock.VerifyGet(l => l.Contractors);
        }

        [Fact]
        public void ElibibilityRules()
        {
            var fcsMock = new Mock<IFcsContext>();

            var fcs = NewService(fcsMock.Object).EsfEligibilityRules;

            fcsMock.VerifyGet(l => l.EsfEligibilityRules);
        }

        [Fact]
        public void UniqueConRefNumbers()
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
                                ConRefNumber = "1",
                            },
                            new TestLearningDelivery()
                            {
                                ConRefNumber = "2",
                            }
                        }
                    },
                    new TestLearner()
                }
            };

            var employerIds = NewService().UniqueConRefNumbers(message).ToList();

            employerIds.Should().HaveCount(2);
            employerIds.Should().Contain(new List<string>() { "1", "2" });
        }

        [Fact]
        public void LargeEmployersForEmployerIds()
        {
            var contractors = new List<Contractor>()
            {
                new Contractor()
                {
                    Ukprn = 1,
                    Contracts = new List<Contract>
                    {
                        new Contract
                        {
                            ContractAllocations = new List<ContractAllocation>
                            {
                                new ContractAllocation
                                {
                                    ContractAllocationNumber = "1",
                                    TenderSpecReference = "T1",
                                    LotReference = "L1"
                                }
                            }
                        }
                    }
                },
                new Contractor()
                {
                    Ukprn = 1,
                    Contracts = new List<Contract>
                    {
                        new Contract
                        {
                            ContractAllocations = new List<ContractAllocation>
                            {
                                new ContractAllocation
                                {
                                    ContractAllocationNumber = "2",
                                    TenderSpecReference = "T2",
                                    LotReference = "L2"
                                }
                            }
                        }
                    }
                }
            }.AsQueryable();

            var eligibilityRules = new List<EsfEligibilityRule>()
            {
                new EsfEligibilityRule()
                {
                    TenderSpecReference = "T1",
                    LotReference = "L1",
                    CalcMethod = 1
                },
                new EsfEligibilityRule()
                {
                    TenderSpecReference = "T2",
                    LotReference = "L2",
                    CalcMethod = 2
                }
            }.AsQueryable();

            var fcsDataRetrievalServiceMock = NewMock();

            fcsDataRetrievalServiceMock.SetupGet(l => l.Contractors).Returns(contractors);
            fcsDataRetrievalServiceMock.SetupGet(l => l.EsfEligibilityRules).Returns(eligibilityRules);

            var conRefNumbers = new List<string>() { "1", "2" };

            var fcs = fcsDataRetrievalServiceMock.Object.FCSContractAllocationsForUKPRN(1, conRefNumbers);

            fcs.Should().HaveCount(2);

            fcs.Should().ContainKeys("1", "2");

            fcs["1"].Select(f => f.CalcMethod).Should().BeEquivalentTo(1);
            fcs["2"].Select(f => f.CalcMethod).Should().BeEquivalentTo(2);
        }

        [Fact]
        public void FCSContractAllocationFromEntity()
        {
            var contractAllocation = new ContractAllocation()
            {
                ContractAllocationNumber = "1",
                FundingStreamCode = "2",
                Period = "3",
                PeriodTypeCode = "4",
                FundingStreamPeriodCode = "5",
                StartDate = new DateTime(2018, 8, 1),
                EndDate = new DateTime(2018, 8, 1),
                UoPCode = "6",
                TenderSpecReference = "T1",
                LotReference = "L1",
            };

            var eligibilityRules = new List<EsfEligibilityRule>()
            {
                new EsfEligibilityRule()
                {
                    TenderSpecReference = "T1",
                    LotReference = "L1",
                    CalcMethod = 1
                },
                new EsfEligibilityRule()
                {
                    TenderSpecReference = "T2",
                    LotReference = "L2",
                    CalcMethod = 2
                }
            }.AsQueryable();

            var fcsDataRetrievalServiceMock = NewMock();

            fcsDataRetrievalServiceMock.SetupGet(l => l.EsfEligibilityRules).Returns(eligibilityRules);

            var fcs = fcsDataRetrievalServiceMock.Object.FCSContractAllocationFromEntity(contractAllocation);

            fcs.ContractAllocationNumber.Should().Be(contractAllocation.ContractAllocationNumber);
            fcs.TenderSpecReference.Should().Be(contractAllocation.TenderSpecReference);
            fcs.LotReference.Should().Be(contractAllocation.LotReference);
            fcs.CalcMethod.Should().Be(1);
        }

        private FCSDataRetrievalService NewService(IFcsContext fcs = null)
        {
            return new FCSDataRetrievalService(fcs);
        }

        private Mock<FCSDataRetrievalService> NewMock()
        {
            return new Mock<FCSDataRetrievalService>()
            {
                CallBase = true
            };
        }
    }
}
