using Autofac;
using ESFA.DC.ILR.Constants;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Providers.Dtos;
using ESFA.DC.ILR.FundingService.Providers.LearnerPaging;

namespace ESFA.DC.ILR.FundingService.Modules
{
    public class ProviderModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ALBDtoProvider>().Keyed<IFundingDtoProvider>(ILRContextKeys.FundingTaskALB).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM25DtoProvider>().Keyed<IFundingDtoProvider>(ILRContextKeys.FundingTaskFM25).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM35DtoProvider>().Keyed<IFundingDtoProvider>(ILRContextKeys.FundingTaskFM35).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM36DtoProvider>().Keyed<IFundingDtoProvider>(ILRContextKeys.FundingTaskFM36).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM70DtoProvider>().Keyed<IFundingDtoProvider>(ILRContextKeys.FundingTaskFM70).InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM81DtoProvider>().Keyed<IFundingDtoProvider>(ILRContextKeys.FundingTaskFM81).InstancePerLifetimeScope();

            containerBuilder.RegisterType<ALBLearnerPagingService>().As<ILearnerPagingService<ALBLearnerDto>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM25LearnerPagingService>().As<ILearnerPagingService<FM25LearnerDto>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM35LearnerPagingService>().As<ILearnerPagingService<FM35LearnerDto>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM36LearnerPagingService>().As<ILearnerPagingService<FM36LearnerDto>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM70LearnerPagingService>().As<ILearnerPagingService<FM70LearnerDto>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM81LearnerPagingService>().As<ILearnerPagingService<FM81LearnerDto>>().InstancePerLifetimeScope();
        }
    }
}
