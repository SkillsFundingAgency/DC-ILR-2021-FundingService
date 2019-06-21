using Autofac;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM70.Service;
using ESFA.DC.ILR.FundingService.FM70.Service.Input;
using ESFA.DC.ILR.FundingService.FM70.Service.Rulebase;
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
    public class FM70Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FM70RulebaseProvider>().As<IRulebaseStreamProvider<FM70LearnerDto>>();
            builder.RegisterType<SessionFactory<FM70LearnerDto>>().As<ISessionFactory<FM70LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<OPAService<FM70LearnerDto>>().As<IOPAService<FM70LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<DataEntityMapper>().As<IDataEntityMapper<FM70LearnerDto>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingOutputService>().As<IOutputService<FM70Global>>().InstancePerLifetimeScope();
            builder.RegisterType<FundingService<FM70LearnerDto, FM70Global>>().As<IFundingService<FM70LearnerDto, FM70Global>>().InstancePerLifetimeScope();
        }
    }
}