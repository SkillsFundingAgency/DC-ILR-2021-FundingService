using Autofac;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Input;
using ESFA.DC.ILR.FundingService.FM25.Service.Output;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Service;
using ESFA.DC.ILR.FundingService.Service.Interfaces;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;

namespace ESFA.DC.ILR.FundingService.Modules.FundingModules
{
    public class FM25Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new RulebaseProvider("Rulebase")).As<IRulebaseProvider>();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<FM25LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<FM25Global>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<FM25LearnerDto, FM25Global>>().As<IFundingService<FM25LearnerDto, FM25Global>>().InstancePerLifetimeScope();
        }
    }
}