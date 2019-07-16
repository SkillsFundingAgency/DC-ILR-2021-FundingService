using Autofac;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM36.Service;
using ESFA.DC.ILR.FundingService.FM36.Service.Input;
using ESFA.DC.ILR.FundingService.FM36.Service.Rulebase;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Output;
using ESFA.DC.ILR.FundingService.Service;
using ESFA.DC.ILR.FundingService.Service.Interfaces;
using ESFA.DC.OPA.Service;
using ESFA.DC.OPA.Service.Builders;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Builders;
using ESFA.DC.OPA.Service.Interface.Rulebase;

namespace ESFA.DC.ILR.FundingService.Modules.FundingModules
{
    public class FM36Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<AbstractFundingModule>();
            builder.RegisterType<FM36RulebaseProvider>().As<IRulebaseStreamProvider<FM36LearnerDto>>();
            builder.RegisterType<SessionFactory<FM36LearnerDto>>().As<ISessionFactory<FM36LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<OPAService<FM36LearnerDto>>().As<IOPAService<FM36LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<FM36LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<FM36Global>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<FM36LearnerDto, FM36Global>>().As<IFundingService<FM36LearnerDto, FM36Global>>().InstancePerLifetimeScope();
            builder.RegisterType<FM36FundingOutputCondenserService>().As<IFundingOutputCondenserService<FM36Global>>().InstancePerLifetimeScope();
        }
    }
}