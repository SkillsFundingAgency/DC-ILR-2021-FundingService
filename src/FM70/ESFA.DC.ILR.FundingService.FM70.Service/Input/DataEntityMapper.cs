using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.FM70.Service.Constants;
using ESFA.DC.ILR.FundingService.FM70.Service.Models;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.ILR.FundingService.FM70.Service.Input
{
    public class DataEntityMapper
    {
        private readonly IFileDataService _fileDataService;

        public DataEntityMapper(IFileDataService fileDataService)
        {
            _fileDataService = fileDataService;
        }

        public Global BuildGlobal()
        {
            return new Global()
            {
                UKPRN = _fileDataService.UKPRN()
            };
        }

        public LearningDeliveryFAMDenormalized BuildLearningDeliveryFAMDenormalized(IEnumerable<ILearningDeliveryFAM> learningDeliveryFams)
        {
            var learningDeliveryFam = new LearningDeliveryFAMDenormalized();

            if (learningDeliveryFams != null)
            {
                learningDeliveryFams = learningDeliveryFams.ToList();

                var ldmArray = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.LearningDeliveryFAMTypeLDM).Select(f => f.LearnDelFAMCode).ToArray();

                Array.Resize(ref ldmArray, 4);

                learningDeliveryFam.RES = learningDeliveryFams.Where(f => f.LearnDelFAMType == Attributes.LearningDeliveryFAMTypeRES).Select(f => f.LearnDelFAMCode).FirstOrDefault();
                learningDeliveryFam.LDM1 = ldmArray[0];
                learningDeliveryFam.LDM2 = ldmArray[1];
                learningDeliveryFam.LDM3 = ldmArray[2];
                learningDeliveryFam.LDM4 = ldmArray[3];
            }

            return learningDeliveryFam;
        }
    }
}