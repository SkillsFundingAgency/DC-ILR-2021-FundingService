using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using ESFA.DC.ILR.Constants;
using ESFA.DC.ILR.FundingService.ALB.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Desktop.Tasks;
using ESFA.DC.ILR.FundingService.Dto.Model;
using ESFA.DC.ILR.FundingService.FM25.Model.Output;
using ESFA.DC.ILR.FundingService.FM35.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.FM81.FundingOutput.Model.Output;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.FundingService.Modules;
using ESFA.DC.ILR.FundingService.Modules.FundingModules;
using ESFA.DC.ILR.FundingService.Orchestrators;
using ESFA.DC.ILR.FundingService.Orchestrators.Interfaces;

namespace ESFA.DC.ILR.FundingService.Desktop.Modules
{
    public class FundingServiceDesktopModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var taskName = "taskName";
            var outputKey = "outputKey";

            containerBuilder.RegisterModule<BaseModule>();
            containerBuilder.RegisterModule<CondenserModule>();
            containerBuilder.RegisterModule<DataCacheModule>();
            containerBuilder.RegisterModule<ProviderModule>();

            containerBuilder.RegisterType<FundingOrchestrationService>().As<IFundingOrchestrationService>();

            containerBuilder.RegisterModule<ALBModule>();
            containerBuilder.RegisterModule<FM25Module>();
            containerBuilder.RegisterModule<FM35Module>();
            containerBuilder.RegisterModule<FM36Module>();
            containerBuilder.RegisterModule<FM70Module>();
            containerBuilder.RegisterModule<FM81Module>();

            containerBuilder.RegisterType<FundingTask<ALBLearnerDto, ALBGlobal>>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskALB)
                .WithParameters(new List<Parameter>
                {
                   new NamedParameter(taskName, ILRContextKeys.FundingTaskALB),
                   new NamedParameter(outputKey, ILRContextKeys.FundingAlbOutput),
                }).InstancePerLifetimeScope();

            containerBuilder.RegisterType<FM25FundingTask<FM25LearnerDto, FM25Global, PeriodisationGlobal>>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM25)
                .WithParameters(new List<Parameter>
                {
                   new NamedParameter(taskName, ILRContextKeys.FundingTaskFM25),
                   new NamedParameter(outputKey, ILRContextKeys.FundingFm25Output),
                }).InstancePerLifetimeScope();

            containerBuilder.RegisterType<FundingTask<FM35LearnerDto, FM35Global>>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM35)
                .WithParameters(new List<Parameter>
                {
                   new NamedParameter(taskName, ILRContextKeys.FundingTaskFM35),
                   new NamedParameter(outputKey, ILRContextKeys.FundingFm35Output),
                }).InstancePerLifetimeScope();

            containerBuilder.RegisterType<FundingTask<FM36LearnerDto, FM36Global>>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM36)
                .WithParameters(new List<Parameter>
                {
                   new NamedParameter(taskName, ILRContextKeys.FundingTaskFM36),
                   new NamedParameter(outputKey, ILRContextKeys.FundingFm36Output),
                }).InstancePerLifetimeScope();

            containerBuilder.RegisterType<FM70FundingTask<FM70LearnerDto, FM70Global>>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM70)
                .WithParameters(new List<Parameter>
                {
                   new NamedParameter(taskName, ILRContextKeys.FundingTaskFM70),
                   new NamedParameter(outputKey, ILRContextKeys.FundingFm70Output),
                }).InstancePerLifetimeScope();

            containerBuilder.RegisterType<FundingTask<FM81LearnerDto, FM81Global>>().Keyed<IFundingTask>(ILRContextKeys.FundingTaskFM81)
                .WithParameters(new List<Parameter>
                {
                   new NamedParameter(taskName, ILRContextKeys.FundingTaskFM81),
                   new NamedParameter(outputKey, ILRContextKeys.FundingFm81Output),
                }).InstancePerLifetimeScope();
        }
    }
}