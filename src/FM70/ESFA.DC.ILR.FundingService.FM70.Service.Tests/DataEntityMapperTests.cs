using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.FM70.Service.Input;
using ESFA.DC.ILR.FundingService.FM70.Service.Models;
using ESFA.DC.ILR.Tests.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.F705.Service.Tests
{
    public class DataEntityMapperTests
    {
        [Fact]
        public void BuildGlobal()
        {
            var ukprn = 1234;

            var fileDataServiceMock = new Mock<IFileDataService>();

            fileDataServiceMock.Setup(f => f.UKPRN()).Returns(ukprn);

            var global = NewService(
                fileDataService: fileDataServiceMock.Object)
                .BuildGlobal();

            global.UKPRN.Should().Be(ukprn);
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized()
        {
            var learningDeliveryFams = new List<TestLearningDeliveryFAM>()
            {
                new TestLearningDeliveryFAM() { LearnDelFAMType = "RES", LearnDelFAMCode = "1" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "2" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "3" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "4" },
                new TestLearningDeliveryFAM() { LearnDelFAMType = "LDM", LearnDelFAMCode = "5" },
            };

            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(learningDeliveryFams);

            learningDeliveryFAMDenormalized.RES.Should().Be("1");
            learningDeliveryFAMDenormalized.LDM1.Should().Be("2");
            learningDeliveryFAMDenormalized.LDM2.Should().Be("3");
            learningDeliveryFAMDenormalized.LDM3.Should().Be("4");
            learningDeliveryFAMDenormalized.LDM4.Should().Be("5");
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized_Null()
        {
            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(null);

            learningDeliveryFAMDenormalized.RES.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM1.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM2.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM3.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM4.Should().BeNull();
        }

        [Fact]
        public void BuildLearningDeliveryFAMDenormalized_NoMatches()
        {
            var learningDeliveryFams = new List<TestLearningDeliveryFAM>();

            var learningDeliveryFAMDenormalized = NewService().BuildLearningDeliveryFAMDenormalized(learningDeliveryFams);

            learningDeliveryFAMDenormalized.RES.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM1.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM2.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM3.Should().BeNull();
            learningDeliveryFAMDenormalized.LDM4.Should().BeNull();
        }

        private DataEntityMapper NewService(IFileDataService fileDataService = null)
        {
            return new DataEntityMapper(fileDataService);
        }
    }
}
