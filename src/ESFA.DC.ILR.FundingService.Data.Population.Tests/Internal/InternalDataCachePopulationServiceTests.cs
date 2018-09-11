using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.ILR.FundingService.Data.Interface;
using ESFA.DC.ILR.FundingService.Data.Internal;
using ESFA.DC.ILR.FundingService.Data.Population.Internal;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.Data.Population.Tests.Internal
{
    public class InternalDataCachePopulationServiceTests
    {
        [Fact]
        public void Populate()
        {
            IInternalDataCache internalDataCache = new InternalDataCache();

            NewService(internalDataCache).Populate();

            internalDataCache.Period1.Should().Be(new DateTime(2018, 8, 1));
            internalDataCache.Period2.Should().Be(new DateTime(2018, 9, 1));
            internalDataCache.Period3.Should().Be(new DateTime(2018, 10, 1));
            internalDataCache.Period4.Should().Be(new DateTime(2018, 11, 1));
            internalDataCache.Period5.Should().Be(new DateTime(2018, 12, 1));
            internalDataCache.Period6.Should().Be(new DateTime(2019, 1, 1));
            internalDataCache.Period7.Should().Be(new DateTime(2019, 2, 1));
            internalDataCache.Period8.Should().Be(new DateTime(2019, 3, 1));
            internalDataCache.Period9.Should().Be(new DateTime(2019, 4, 1));
            internalDataCache.Period10.Should().Be(new DateTime(2019, 5, 1));
            internalDataCache.Period11.Should().Be(new DateTime(2019, 6, 1));
            internalDataCache.Period12.Should().Be(new DateTime(2019, 7, 1));
        }

        private InternalDataCachePopulationService NewService(IInternalDataCache internalDataCache = null)
        {
            return new InternalDataCachePopulationService(internalDataCache);
        }
    }
}
