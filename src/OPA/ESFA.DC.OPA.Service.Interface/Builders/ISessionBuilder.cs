using System.IO;
using ESFA.DC.OPA.Model.Interface;
using Oracle.Determinations.Engine;

namespace ESFA.DC.OPA.Service.Interface.Builders
{
    public interface ISessionBuilder
    {
        Session ProcessOPASession(Session session, IDataEntity globalEntity);
    }
}
