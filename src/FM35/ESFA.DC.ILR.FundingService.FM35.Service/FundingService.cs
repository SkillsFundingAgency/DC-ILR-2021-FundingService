using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Interface;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.FM35.Service
{
    public class FundingService : IFundingService<IFM35FundingOutputs>
    {
        private readonly IDataEntityMapper<ILearner, IFM35FundingOutputs> _dataEntityMapper;
        private readonly IOPAService _opaService;
        private readonly IOutputService<IFM35FundingOutputs> _fundingOutputService;

        public FundingService(IDataEntityMapper<ILearner, IFM35FundingOutputs> dataEntityMapper, IOPAService opaService, IOutputService<IFM35FundingOutputs> fundingOutputService)
        {
            _dataEntityMapper = dataEntityMapper;
            _opaService = opaService;
            _fundingOutputService = fundingOutputService;
        }

        public IFM35FundingOutputs ProcessFunding(int ukprn, IList<ILearner> learnerList)
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