using Autofac;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM81.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM81.Service;
using ESFA.DC.ILR.FundingService.FM81.Service.Input;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Service;
using ESFA.DC.ILR.FundingService.Service.Interfaces;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;

namespace ESFA.DC.ILR.FundingService.Modules.FundingModules
{
    public class FM81Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new RulebaseProvider("Trailblazer Funding Calc 18_19")).As<IRulebaseProvider>();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<FM81LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<FM81Global>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<FM81LearnerDto, FM81Global>>().As<IFundingService<FM81LearnerDto, FM81Global>>().InstancePerLifetimeScope();
        }
    }
}