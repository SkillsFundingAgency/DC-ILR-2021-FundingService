using System.IO;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Builders;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using Oracle.Determinations.Engine;

namespace ESFA.DC.OPA.Service
{
    public class OPAService : IOPAService
    {
        private readonly ISessionBuilder _sessionBuilder;
        private readonly IOPADataEntityBuilder _dataEntityBuilder;
        private readonly IRulebaseProvider _rulebaseProvider;

        public OPAService(ISessionBuilder sessionBuilder, IOPADataEntityBuilder dataEntityBuilder, IRulebaseProvider rulebaseProvider)
        {
            _sessionBuilder = sessionBuilder;
            _dataEntityBuilder = dataEntityBuilder;
            _rulebaseProvider = rulebaseProvider;
        }

        public IDataEntity ExecuteSession(IDataEntity globalEntity)
        {
            Session session;

            using (Stream stream = _rulebaseProvider.GetStream())
            {
                session = _sessionBuilder.CreateOPASession(stream, globalEntity);
            }

            session.Think();

            var outputGlobalInstance = session.GetGlobalEntityInstance();
            var outputEntity = _dataEntityBuilder.CreateOPADataEntity(outputGlobalInstance, null);

            return outputEntity;
        }
    }
}
