using Autofac;
using ESFA.DC.OPA.Service;
using ESFA.DC.OPA.Service.Builders;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Builders;

namespace ESFA.DC.ILR.FundingService.Modules
{
    public class OpaModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DataEntityAttributeService>().As<IDataEntityAttributeService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SessionBuilder>().As<ISessionBuilder>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<OPADataEntityBuilder>().As<IOPADataEntityBuilder>().WithParameter("yearStartDate", new System.DateTime(2019, 8, 1)).InstancePerLifetimeScope();
        }
    }
}