using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External;
using ESFA.DC.ILR.FundingService.Data.Population.External;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.CollectionDates;
using ESFA.DC.ILR.ReferenceDataService.Model.MetaData.ReferenceDataVersions;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.External
{
    public class MetaDataMapperServiceTests
    {
        [Fact]
        public void GetReferenceDataVersions()
        {
            var metaData = TestMetaData();

            var result = NewService().GetReferenceDataVersions(metaData);

            result.LarsVersion.Version.Should().Be("1");
            result.PostcodesVersion.Version.Should().Be("1");
            result.OrganisationsVersion.Version.Should().Be("1");
            result.Employers.Version.Should().Be("1");
            result.CoFVersion.Version.Should().Be("1");
            result.EasUploadDateTime.UploadDateTime.Should().Be(new DateTime(2019, 8, 1));
        }

        [Fact]
        public void BuildPeriods()
        {
            var metaData = TestMetaData();

            var periods = new Periods
            {
                Period1 = new DateTime(2019, 8, 1),
                Period2 = new DateTime(2019, 9, 1),
                Period3 = new DateTime(2019, 10, 1),
                Period4 = new DateTime(2019, 11, 1),
                Period5 = new DateTime(2019, 12, 1),
                Period6 = new DateTime(2020, 1, 1),
                Period7 = new DateTime(2020, 2, 1),
                Period8 = new DateTime(2020, 3, 1),
                Period9 = new DateTime(2020, 4, 1),
                Period10 = new DateTime(2020, 5, 1),
                Period11 = new DateTime(2020, 6, 1),
                Period12 = new DateTime(2020, 7, 1),
            };

            NewService().BuildPeriods(metaData).Should().BeEquivalentTo(periods);
        }

        private MetaData TestMetaData() => new MetaData
        {
            ReferenceDataVersions = new ReferenceDataVersion
            {
                LarsVersion = new LarsVersion { Version = "1" },
                PostcodesVersion = new PostcodesVersion { Version = "1" },
                OrganisationsVersion = new OrganisationsVersion { Version = "1" },
                Employers = new EmployersVersion { Version = "1" },
                CoFVersion = new CoFVersion { Version = "1" },
                EasUploadDateTime = new EasUploadDateTime { UploadDateTime = new DateTime(2019, 8, 1) },
            },
            CollectionDates = new IlrCollectionDates
            {
                CensusDates = new List<CensusDate>
                        {
                            new CensusDate { Period = 1, Start = new DateTime(2019, 8, 1) },
                            new CensusDate { Period = 2, Start = new DateTime(2019, 9, 1) },
                            new CensusDate { Period = 3, Start = new DateTime(2019, 10, 1) },
                            new CensusDate { Period = 4, Start = new DateTime(2019, 11, 1) },
                            new CensusDate { Period = 5, Start = new DateTime(2019, 12, 1) },
                            new CensusDate { Period = 6, Start = new DateTime(2020, 1, 1) },
                            new CensusDate { Period = 7, Start = new DateTime(2020, 2, 1) },
                            new CensusDate { Period = 8, Start = new DateTime(2020, 3, 1) },
                            new CensusDate { Period = 9, Start = new DateTime(2020, 4, 1) },
                            new CensusDate { Period = 10, Start = new DateTime(2020, 5, 1) },
                            new CensusDate { Period = 11, Start = new DateTime(2020, 6, 1) },
                            new CensusDate { Period = 12, Start = new DateTime(2020, 7, 1) },
                        }
            }
        };

        private MetaDataMapperService NewService()
        {
            return new MetaDataMapperService();
        }
    }
}
