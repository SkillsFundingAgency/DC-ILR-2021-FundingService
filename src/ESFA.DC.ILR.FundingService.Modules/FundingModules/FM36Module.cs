using Autofac;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM36.Service;
using ESFA.DC.ILR.FundingService.FM36.Service.Input;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Service;
using ESFA.DC.ILR.FundingService.Service.Interfaces;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using ESFA.DC.OPA.Service.Rulebase;

namespace ESFA.DC.ILR.FundingService.Modules.FundingModules
{
    public class FM36Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new RulebaseProvider("Apprenticeships Earnings Calc 18_19")).As<IRulebaseProvider>();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<FM36LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<FM36Global>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<FM36LearnerDto, FM36Global>>().As<IFundingService<FM36LearnerDto, FM36Global>>().InstancePerLifetimeScope();
        }
    }
}