using System.Collections.Generic;
using Autofac;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Input;
using ESFA.DC.ILR.FundingService.FM25.Service.Output;
using ESFA.DC.ILR.FundingService.FM25.Service.Rulebase;
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
    public class FM25Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<AbstractFundingModule>();
            builder.RegisterType<FM25RulebaseProvider>().As<IRulebaseStreamProvider<FM25LearnerDto>>();
            builder.RegisterType<FM25PeriodisationRulebaseProvider>().As<IRulebaseStreamProvider<FM25LearnerDto>>();
            builder.RegisterType<SessionFactory<FM25LearnerDto>>().As<ISessionFactory<FM25LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<OPAService<FM25LearnerDto>>().As<IOPAService<FM25LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<FM25LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<FM25Global>>().InstancePerLifetimeScope();
            builder.RegisterType<PeriodisationOutputService>().As<IOutputService<IEnumerable<PeriodisationGlobal>>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<FM25LearnerDto, FM25Global>>().As<IFundingService<FM25LearnerDto, FM25Global>>().InstancePerLifetimeScope();
            builder.RegisterType<FM25FundingOutputCondenserService>().As<IFundingOutputCondenserService<FM25Global>>().InstancePerLifetimeScope();
        }
    }
}