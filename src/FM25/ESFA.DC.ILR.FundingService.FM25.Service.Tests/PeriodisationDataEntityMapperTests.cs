using System;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Input;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM25.Service.Tests
{
    public class PeriodisationDataEntityMapperTests
    {
        [Fact]
        public void BuildGlobal()
        {
            var ukprn = 1234;

            var global = new FM25Global()
            {
                UKPRN = ukprn
            };

            var dataEntity = NewService().BuildGlobalDataEntity(global);

            dataEntity.EntityName.Should().Be("global");
            dataEntity.Attributes["UKPRN"].Value.Should().Be(ukprn);
        }

        [Fact]
        public void BuildLearner()
        {
            var acadMonthPayment = 1;
            var fundLine = "FundLine";
            var learnerActEndDate = new DateTime(2017, 1, 1);
            var learnerPlanEndDate = new DateTime(2018, 1, 1);
            var learnerStartDate = new DateTime(2019, 1, 1);
            var learnRefNumber = "LearnRefNumber";
            var onProgPayment = 1.0m;

            var learner = new FM25Learner()
            {
                AcadMonthPayment = acadMonthPayment,
                FundLine = fundLine,
                LearnerActEndDate = learnerActEndDate,
                LearnerPlanEndDate = learnerPlanEndDate,
                LearnerStartDate = learnerStartDate,
                LearnRefNumber = learnRefNumber,
                OnProgPayment = onProgPayment,
            };

            var dataEntity = NewService().BuildLearnerDataEntity(learner);

            dataEntity.EntityName.Should().Be("Learner");

            dataEntity.Attributes["AcadMonthPayment"].Value.Should().Be(acadMonthPayment);
            dataEntity.Attributes["FundLine"].Value.Should().Be(fundLine);
            dataEntity.Attributes["LearnerActEndDate"].Value.Should().Be(learnerActEndDate);
            dataEntity.Attributes["LearnerPlanEndDate"].Value.Should().Be(learnerPlanEndDate);
            dataEntity.Attributes["LearnerStartDate"].Value.Should().Be(learnerStartDate);
            dataEntity.Attributes["LearnRefNumber"].Value.Should().Be(learnRefNumber);
            dataEntity.Attributes["OnProgPayment"].Value.Should().Be(onProgPayment);

            dataEntity.Children.Should().BeEmpty();
        }

        private PeriodisationDataEntityMapper NewService()
        {
            return new PeriodisationDataEntityMapper();
        }
    }
}
