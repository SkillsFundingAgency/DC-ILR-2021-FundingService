using Autofac;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM81.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Orchestrators.Output;

namespace ESFA.DC.ILR.FundingService.Modules
{
    public class CondenserModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<FM81FundingOutputCondenserService>().As<IFundingOutputCondenserService<FM81Global>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM70FundingOutputCondenserService>().As<IFundingOutputCondenserService<FM70Global>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM35FundingOutputCondenserService>().As<IFundingOutputCondenserService<FM35Global>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM36FundingOutputCondenserService>().As<IFundingOutputCondenserService<FM36Global>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ALBFundingOutputCondenserService>().As<IFundingOutputCondenserService<ALBGlobal>>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FM25FundingOutputCondenserService>().As<IFundingOutputCondenserService<FM25Global>>().InstancePerLifetimeScope();
        }
    }
}
