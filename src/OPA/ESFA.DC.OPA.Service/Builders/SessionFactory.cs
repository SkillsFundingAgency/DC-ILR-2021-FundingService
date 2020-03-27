using ESFA.DC.OPA.Service.Interface.Builders;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using Oracle.Determinations.Engine;
using Oracle.Determinations.Masquerade.IO;

namespace ESFA.DC.OPA.Service.Builders
{
    public class SessionFactory<T> : ISessionFactory<T>
    {
        private readonly IRulebaseStreamProvider<T> _rulebaseStreamProvider;

        public SessionFactory(IRulebaseStreamProvider<T> rulebaseStreamProvider)
        {
            _rulebaseStreamProvider = rulebaseStreamProvider;
        }

        private Rulebase Rulebase { get; set; } = null;

        public Session CreateSession()
        {
            if (Rulebase == null)
            {
                using (var stream = _rulebaseStreamProvider.GetStream())
                {
                    using (var rulebaseStream = new InputStreamAdapter(stream))
                    {
                        Rulebase = Engine.INSTANCE.GetRulebase(rulebaseStream);
                    }
                }
            }

            return Engine.INSTANCE.CreateSession(Rulebase);
        }
    }
}
