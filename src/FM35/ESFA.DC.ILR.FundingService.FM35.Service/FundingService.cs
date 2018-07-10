using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.Service
{
    public class FundingService : IFundingService<FM35FundingOutputs>
    {
        private readonly IDataEntityMapper<ILearner, FM35FundingOutputs> _dataEntityMapper;
        private readonly IOPAService _opaService;
        private readonly IOutputService<FM35FundingOutputs> _fundingOutputService;

        public FundingService(IDataEntityMapper<ILearner, FM35FundingOutputs> dataEntityMapper, IOPAService opaService, IOutputService<FM35FundingOutputs> fundingOutputService)
        {
            _dataEntityMapper = dataEntityMapper;
            _opaService = opaService;
            _fundingOutputService = fundingOutputService;
        }

        public FM35FundingOutputs ProcessFunding(IEnumerable<ILearner> learnerList)
        {
            // Generate Funding Inputs
            var inputDataEntities = _dataEntityMapper.MapTo(learnerList).AsParallel().ToList();

            // Execute OPA
            var outputDataEntities = ExecuteSessions(inputDataEntities);

            // Transform to FundingOutput Model and return
            return _fundingOutputService.ProcessFundingOutputs(outputDataEntities);
        }

        protected internal ConcurrentBag<IDataEntity> ExecuteSessions(IEnumerable<IDataEntity> inputDataEntities)
        {
            var outputDataEntities = new ConcurrentBag<IDataEntity>();

            //Parallel.ForEach(inputDataEntities, e =>
            //{
            //    IDataEntity sessionEntity = _opaService.ExecuteSession(e);

            //    outputDataEntities.Add(sessionEntity);
            //});

            foreach (var globalEntity in inputDataEntities)
            {
                IDataEntity sessionEntity = _opaService.ExecuteSession(globalEntity);

                outputDataEntities.Add(sessionEntity);
            }

            return outputDataEntities;
        }
    }
}