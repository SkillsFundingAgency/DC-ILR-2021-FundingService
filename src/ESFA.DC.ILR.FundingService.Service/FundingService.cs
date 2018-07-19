using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.OPA.Service.Interface;

namespace ESFA.DC.ILR.FundingService.Service
{
    public class FundingService<TIn, TOut> : IFundingService<TIn, TOut>
    {
        private readonly IDataEntityMapper<TIn> _dataEntityMapper;
        private readonly IOPAService _opaService;
        private readonly IOutputService<TOut> _fundingOutputService;

        public FundingService(IDataEntityMapper<TIn> dataEntityMapper, IOPAService opaService, IOutputService<TOut> fundingOutputService)
        {
            _dataEntityMapper = dataEntityMapper;
            _opaService = opaService;
            _fundingOutputService = fundingOutputService;
        }

        public TOut ProcessFunding(IEnumerable<TIn> learnerList)
        {
            var inputDataEntities = _dataEntityMapper.MapTo(learnerList);

            var outputDataEntities = inputDataEntities.Select(e => _opaService.ExecuteSession(e)).ToList();

            return _fundingOutputService.ProcessFundingOutputs(outputDataEntities);
        }
    }
}