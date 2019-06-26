using System.IO;
using ESFA.DC.OPA.Model.Interface;
using Oracle.Determinations.Engine;

namespace ESFA.DC.OPA.Service.Interface.Builders
{
    public interface ISessionFactory<T>
    {
        Session CreateSession();
    }
}
