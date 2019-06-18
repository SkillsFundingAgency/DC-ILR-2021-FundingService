﻿using Autofac;
using ESFA.DC.ILR.FundingService.FundingActor.Modules;
using ESFA.DC.ILR.FundingService.Modules;
using ESFA.DC.ILR.FundingService.Modules.FundingModules;

namespace ESFA.DC.ILR.FundingService.FM81Actor.Modules
{
    public class ActorFundingFM81Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<BaseModule>();
            builder.RegisterModule<ActorModule>();
            builder.RegisterModule<FM81Module>();
        }
    }
}