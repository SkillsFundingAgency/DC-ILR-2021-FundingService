using Autofac;
using ESFA.DC.ILR.FundingService.FundingActor.Modules;
using ESFA.DC.ILR.FundingService.Modules;
using ESFA.DC.ILR.FundingService.Modules.FundingModules;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Json;

namespace ESFA.DC.ILR.FundingService.ALBActor.Modules
{
    public class ActorFundingALBModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<BaseModule>();
            builder.RegisterModule<LoggerModule>();
            builder.RegisterModule<ALBModule>();
        }
    }
}