using System.Collections.Concurrent;
using System.Collections.Generic;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.ALB.Service
{
    public class FundingService : IFundingService<FundingOutputs>
    {
        private readonly IDataEntityMapper<ILearner, FundingOutputs> _dataEntityBuilder;
        private readonly IOPAService _opaService;
        private readonly IOutputService<FundingOutputs> _outputService;

        public FundingService(IDataEntityMapper<ILearner, FundingOutputs> dataEntityBuilder, IOPAService opaService, IOutputService<FundingOutputs> fundingOutputService)
        {
            _dataEntityBuilder = dataEntityBuilder;
            _opaService = opaService;
            _outputService = fundingOutputService;
        }

        public FundingOutputs ProcessFunding(IEnumerable<ILearner> learnerList)
        {
            // Generate Funding Inputs
            var inputDataEntities = _dataEntityBuilder.MapTo(learnerList);

            // Execute OPA
            var outputDataEntities = ExecuteSessions(inputDataEntities);

            // Transform to FundingOutput Model and return
            return _outputService.ProcessFundingOutputs(outputDataEntities);
        }

        protected internal ConcurrentBag<IDataEntity> ExecuteSessions(IEnumerable<IDataEntity> inputDataEntities)
        {
            var outputDataEntities = new ConcurrentBag<IDataEntity>();

            foreach (var globalEntity in inputDataEntities)
            {
                IDataEntity sessionEntity = _opaService.ExecuteSession(globalEntity);

                outputDataEntities.Add(sessionEntity);
            }

            return outputDataEntities;
        }
    }
}
