using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Autofac;
using ESFA.DC.ILR.FundingService.Console.Modules;
using ESFA.DC.ILR.FundingService.Interfaces;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Xml;
using ConsoleALBModule = ESFA.DC.ILR.FundingService.Console.Modules.ConsoleALBModule;

namespace ESFA.DC.ILR.FundingService.Console
{
    public class Program
    {
        private const string ALB = "ALB";
        private const string FM35 = "FM35";
        
        // Arg 0 - FileName
        // Args 1+ - Fund Models
        public static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();

            var message = GetMessageFromFile(args[0]);

            System.Console.WriteLine("Executing Funding Service...");

            foreach (var fundModel in args.Skip(1))
            {
                var container = BuildContainerForFundModel(fundModel);

                using (var scope = container.BeginLifetimeScope())
                {
                    var fundingTasks = container.Resolve<ITaskProviderService>();

                    stopwatch.Start();

                    fundingTasks.ExecuteTasks(message);

                    var fundingCreateTime = stopwatch.Elapsed;
                    System.Console.WriteLine("Process completed in " + fundingCreateTime);
                }
            }
        }

        private static IMessage GetMessageFromFile(string fileName)
        {
            System.Console.WriteLine("Loading file..");

            using (var stream = new FileStream(@"Files\" + fileName, FileMode.Open))
            {
                try
                {
                    IXmlSerializationService serializationService = new XmlSerializationService();

                    return serializationService.Deserialize<Message>(stream);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("File Load Error: Problem loading file... {0}", ex);

                    throw;
                }
            }
        }

        private static IContainer BuildContainerForFundModel(string fundModel)
        {
            var containerBuilder = new ContainerBuilder();

            switch (fundModel)
            {
                case ALB:
                    containerBuilder.RegisterModule<ConsoleALBModule>();
                    break;
                case FM35:
                    containerBuilder.RegisterModule<ConsoleFM35Module>();
                    break;
            }
            
            return containerBuilder.Build();
        }
    }
}
