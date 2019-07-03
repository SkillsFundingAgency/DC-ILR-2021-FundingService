using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.FCS.Model;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.ReferenceDataService.Model.FCS;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class FCSMapperServiceTests
    {
        [Fact]
        public void MapFCSContractAllocations()
        {
            var expectedContractAllocations = ExpectedContractAllocations();

            var contractAllocations = new List<FcsContractAllocation>
            {
                new FcsContractAllocation
                {
                    ContractAllocationNumber = "Contract1",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = new DateTime(2019, 7, 31),
                    FundingStreamPeriodCode = "FundingStreamPeriodCode1",
                    LearningRatePremiumFactor = 1.0m,
                    TenderSpecReference = "TenderSpecReference1",
                    LotReference = "LotReference1",
                    EsfEligibilityRule = new EsfEligibilityRule
                    {
                        CalcMethod = 1
                    },
                    FCSContractDeliverables = new List<FcsContractDeliverable>
                    {
                        new FcsContractDeliverable
                        {
                            DeliverableCode = 1,
                            DeliverableDescription = "DeliverableDescription1",
                            PlannedValue = 1.0m,
                            PlannedVolume = 1,
                            UnitCost = 1.0m,
                            ExternalDeliverableCode = "ExternalDeliverableCode1"
                        },
                        new FcsContractDeliverable
                        {
                            DeliverableCode = 11,
                            DeliverableDescription = "DeliverableDescription11",
                            PlannedValue = 11m,
                            PlannedVolume = 11,
                            UnitCost = 11m,
                            ExternalDeliverableCode = "ExternalDeliverableCode11"
                        }
                    }
                },
                new FcsContractAllocation
                {
                    ContractAllocationNumber = "Contract2",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = new DateTime(2019, 7, 31),
                    FundingStreamPeriodCode = "FundingStreamPeriodCode2",
                    LearningRatePremiumFactor = 2.0m,
                    TenderSpecReference = "TenderSpecReference2",
                    LotReference = "LotReference2",
                    EsfEligibilityRule = new EsfEligibilityRule
                    {
                        CalcMethod = 2
                    },
                    FCSContractDeliverables = new List<FcsContractDeliverable>
                    {
                        new FcsContractDeliverable
                        {
                            DeliverableCode = 2,
                            DeliverableDescription = "DeliverableDescription2",
                            PlannedValue = 2.0m,
                            PlannedVolume = 2,
                            UnitCost = 2.0m,
                            ExternalDeliverableCode = "ExternalDeliverableCode2"
                        },
                        new FcsContractDeliverable
                        {
                            DeliverableCode = 22,
                            DeliverableDescription = "DeliverableDescription22",
                            PlannedValue = 22m,
                            PlannedVolume = 22,
                            UnitCost = 22m,
                            ExternalDeliverableCode = "ExternalDeliverableCode22"
                        }
                    }
                },
                new FcsContractAllocation
                {
                    ContractAllocationNumber = "Contract3",
                    StartDate = new DateTime(2018, 8, 1),
                    EndDate = new DateTime(2019, 7, 31),
                    FundingStreamPeriodCode = "FundingStreamPeriodCode3",
                    LearningRatePremiumFactor = 3.0m,
                    TenderSpecReference = "TenderSpecReference3",
                    LotReference = "LotReference3",
                    EsfEligibilityRule = new EsfEligibilityRule
                    {
                        CalcMethod = 3
                    }
                }
            };

            var result = NewService().MapFCSContractAllocations(contractAllocations);

            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(expectedContractAllocations);
        }

        [Fact]
        public void MapFCSContractAllocations_Null()
        {
            NewService().MapFCSContractAllocations(null).Should().BeNull();
        }

        private IReadOnlyCollection<FCSContractAllocation> ExpectedContractAllocations()
        {
            return new List<FCSContractAllocation>
            {
                new FCSContractAllocation
                {
                    ContractAllocationNumber = "Contract1",
                    ContractStartDate = new DateTime(2018, 8, 1),
                    ContractEndDate = new DateTime(2019, 7, 31),
                    FundingStreamPeriodCode = "FundingStreamPeriodCode1",
                    LearningRatePremiumFactor = 1.0m,
                    TenderSpecReference = "TenderSpecReference1",
                    LotReference = "LotReference1",
                    CalcMethod = 1,
                    FCSContractDeliverables = new List<FCSContractDeliverable>
                    {
                        new FCSContractDeliverable
                        {
                            DeliverableCode = 1,
                            DeliverableDescription = "DeliverableDescription1",
                            PlannedValue = 1.0m,
                            PlannedVolume = 1,
                            UnitCost = 1.0m,
                            ExternalDeliverableCode = "ExternalDeliverableCode1"
                        },
                        new FCSContractDeliverable
                        {
                            DeliverableCode = 11,
                            DeliverableDescription = "DeliverableDescription11",
                            PlannedValue = 11m,
                            PlannedVolume = 11,
                            UnitCost = 11m,
                            ExternalDeliverableCode = "ExternalDeliverableCode11"
                        }
                    }
                },
                new FCSContractAllocation
                {
                    ContractAllocationNumber = "Contract2",
                    ContractStartDate = new DateTime(2018, 8, 1),
                    ContractEndDate = new DateTime(2019, 7, 31),
                    FundingStreamPeriodCode = "FundingStreamPeriodCode2",
                    LearningRatePremiumFactor = 2.0m,
                    TenderSpecReference = "TenderSpecReference2",
                    LotReference = "LotReference2",
                    CalcMethod = 2,
                    FCSContractDeliverables = new List<FCSContractDeliverable>
                    {
                        new FCSContractDeliverable
                        {
                            DeliverableCode = 2,
                            DeliverableDescription = "DeliverableDescription2",
                            PlannedValue = 2.0m,
                            PlannedVolume = 2,
                            UnitCost = 2.0m,
                            ExternalDeliverableCode = "ExternalDeliverableCode2"
                        },
                        new FCSContractDeliverable
                        {
                            DeliverableCode = 22,
                            DeliverableDescription = "DeliverableDescription22",
                            PlannedValue = 22m,
                            PlannedVolume = 22,
                            UnitCost = 22m,
                            ExternalDeliverableCode = "ExternalDeliverableCode22"
                        }
                    }
                },
                new FCSContractAllocation
                {
                    ContractAllocationNumber = "Contract3",
                    ContractStartDate = new DateTime(2018, 8, 1),
                    ContractEndDate = new DateTime(2019, 7, 31),
                    FundingStreamPeriodCode = "FundingStreamPeriodCode3",
                    LearningRatePremiumFactor = 3.0m,
                    TenderSpecReference = "TenderSpecReference3",
                    LotReference = "LotReference3",
                    CalcMethod = 3,
                }
            };
        }

        private FCSMapperService NewService()
        {
            return new FCSMapperService();
        }
    }
}
