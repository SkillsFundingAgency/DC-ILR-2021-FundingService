using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Service.Interfaces;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Rulebase;

namespace ESFA.DC.ILR.FundingService.Service
{
    public class FundingService<TIn, TOut> : IFundingService<TIn, TOut>
    {
        private readonly IDataEntityMapper<TIn> _dataEntityMapper;
        private readonly IOPAService _opaService;
        private readonly IOutputService<TOut> _fundingOutputService;
        private readonly IRulebaseStreamProvider<TIn> _rulebaseStreamProvider;

        public FundingService(IDataEntityMapper<TIn> dataEntityMapper, IOPAService opaService, IOutputService<TOut> fundingOutputService, IRulebaseStreamProvider<TIn> rulebaseStreamProvider)
        {
            _dataEntityMapper = dataEntityMapper;
            _opaService = opaService;
            _fundingOutputService = fundingOutputService;
            _rulebaseStreamProvider = rulebaseStreamProvider;
        }

        public TOut ProcessFunding(int ukprn, IEnumerable<TIn> learnerList, CancellationToken cancellationToken)
        {
            IEnumerable<IDataEntity> inputDataEntities = _dataEntityMapper.MapTo(ukprn, learnerList);

            cancellationToken.ThrowIfCancellationRequested();

            IEnumerable<IDataEntity> outputDataEntities = inputDataEntities.Select(e => _opaService.ExecuteSession(e, _rulebaseStreamProvider.GetStream)).ToList();

            cancellationToken.ThrowIfCancellationRequested();

            return _fundingOutputService.ProcessFundingOutputs(outputDataEntities);
        }
    }
}