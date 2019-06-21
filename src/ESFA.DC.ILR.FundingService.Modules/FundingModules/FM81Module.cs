using Autofac;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM81.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM81.Service;
using ESFA.DC.ILR.FundingService.FM81.Service.Input;
using ESFA.DC.ILR.FundingService.FM81.Service.Rulebase;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Service;
using ESFA.DC.ILR.FundingService.Service.Interfaces;
using ESFA.DC.OPA.Service;
using ESFA.DC.OPA.Service.Builders;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Builders;
using ESFA.DC.OPA.Service.Interface.Rulebase;

namespace ESFA.DC.ILR.FundingService.Modules.FundingModules
{
    public class FM81Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FM81RulebaseProvider>().As<IRulebaseStreamProvider<FM81LearnerDto>>();
            builder.RegisterType<SessionFactory<FM81LearnerDto>>().As<ISessionFactory<FM81LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<OPAService<FM81LearnerDto>>().As<IOPAService<FM81LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<FM81LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<FM81Global>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<FM81LearnerDto, FM81Global>>().As<IFundingService<FM81LearnerDto, FM81Global>>().InstancePerLifetimeScope();
        }
    }
}