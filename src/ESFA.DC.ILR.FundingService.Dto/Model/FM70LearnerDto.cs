﻿using System;
using System.Collections.Generic;
using ESFA.DC.ILR.Model;

namespace ESFA.DC.ILR.FundingService.Dto.Model
{
    public class FM70LearnerDto
    {
        public string LearnRefNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public List<MessageLearnerDestinationandProgressionDPOutcome> DPOutcomes { get; set; }

        public List<LearnerEmploymentStatus> LearnerEmploymentStatuses { get; set; }

        public List<LearningDelivery> LearningDeliveries { get; set; }
    }
}
