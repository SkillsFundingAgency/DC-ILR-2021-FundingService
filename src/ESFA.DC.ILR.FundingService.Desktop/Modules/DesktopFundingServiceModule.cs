using Autofac;
using ESFA.DC.ILR.Constants;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Modules;

namespace ESFA.DC.ILR.FundingService.Desktop.Modules
{
    public class DesktopFundingServiceModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<BaseModule>();
            containerBuilder.RegisterType<DesktopFundingTaskProvider>().As<IFundingTaskProvider>();

            //containerBuilder.RegisterType<DesktopALBActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskALB).InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DesktopFM25ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM25).InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DesktopFM35ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM35).InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DesktopFM36ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM36).InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DesktopFM70ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM70).InstancePerLifetimeScope();
            //containerBuilder.RegisterType<DesktopFM81ActorTask>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM81).InstancePerLifetimeScope();
        }
    }
}