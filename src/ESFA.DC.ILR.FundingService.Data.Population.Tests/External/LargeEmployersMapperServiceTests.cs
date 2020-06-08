using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LargeEmployer.Model;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.ReferenceDataService.Model.Employers;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class LargeEmployersMapperServiceTests
    {
        [Fact]
        public void MapLargeEmployers()
        {
            var expectedLargeEmployerDictionary = ExpectedLargeEmployerDictionary();

            var employers = new List<Employer>
            {
                new Employer
                {
                    ERN = 1,
                    LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>
                    {
                        new LargeEmployerEffectiveDates
                        {
                           EffectiveFrom = new DateTime(2018, 8, 1),
                           EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new LargeEmployerEffectiveDates
                        {
                           EffectiveFrom = new DateTime(2018, 9, 2)
                        },
                    },
                },
                new Employer
                {
                    ERN = 2,
                    LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>
                    {
                        new LargeEmployerEffectiveDates
                        {
                           EffectiveFrom = new DateTime(2018, 8, 1)
                        }
                    },
                },
                new Employer
                {
                    ERN = 3,
                    LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>(),
                },
                new Employer
                {
                    ERN = 4,
                    LargeEmployerEffectiveDates = new List<LargeEmployerEffectiveDates>(),
                },
            };

            var result = NewService().MapLargeEmployers(employers);

            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedLargeEmployerDictionary);
        }

        [Fact]
        public void MapLargeEmployers_Null()
        {
            NewService().MapLargeEmployers(null).Should().BeEquivalentTo(new Dictionary<int, IReadOnlyCollection<LargeEmployers>>());
        }

        [Fact]
        public void MapLargeEmployers_Empty()
        {
            NewService().MapLargeEmployers(new List<Employer>()).Should().BeEquivalentTo(new Dictionary<int, IReadOnlyCollection<LargeEmployers>>());
        }

        private IDictionary<int, IReadOnlyCollection<LargeEmployers>> ExpectedLargeEmployerDictionary()
        {
            return new Dictionary<int, IReadOnlyCollection<LargeEmployers>>
            {
                {
                    1,
                    new List<LargeEmployers>
                    {
                        new LargeEmployers
                        {
                            ERN = 1,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                            EffectiveTo = new DateTime(2018, 9, 1)
                        },
                        new LargeEmployers
                        {
                            ERN = 1,
                            EffectiveFrom = new DateTime(2018, 9, 2)
                        }
                    }
                },
                {
                    2,
                    new List<LargeEmployers>
                    {
                        new LargeEmployers
                        {
                            ERN = 2,
                            EffectiveFrom = new DateTime(2018, 8, 1),
                        }
                    }
                }
            };
        }

        private LargeEmployersMapperService NewService()
        {
            return new LargeEmployersMapperService();
        }
    }
}
