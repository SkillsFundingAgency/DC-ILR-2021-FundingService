using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.ILR.FundingService.FM35.Modules;
using ESFA.DC.ILR.FundingService.FM35.TaskProvider.Interface;
using ESFA.DC.ILR.Model;
using ESFA.DC.Serialization.Interfaces;
using ESFA.DC.Serialization.Xml;

namespace ESFA.DC.ILR.FundingService.FM35.Console
{
    public class Program
    {
         private static string FileName = "ILR-10006341-1819-20180118-023456-01.xml";
        // private const string FileName = "ILR-10006341-1819-20180118-023456-02.xml";
        //private const string FileName = "ILR-10006341-1819-20180613-144516-03.xml";
        private static Stream stream;

        private static Message message;

        public static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();

            GetILRFile();

            System.Console.WriteLine("Executing Funding Service...");

            var container = BuildContainer();

            using (var scope = container.BeginLifetimeScope())
            {
                var fundingTasks = container.Resolve<ITaskProviderService>();

                stopwatch.Start();

                fundingTasks.ExecuteTasks(message);

                var fundingCreateTime = stopwatch.Elapsed;
                System.Console.WriteLine("Process completed in " + fundingCreateTime.ToString());
            }
        }

        public static void GetILRFile()
        {
            try
            {
                System.Console.WriteLine("Loading file..");

                stream = new FileStream(@"Files\" + FileName, FileMode.Open);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("File Load Error: Problem loading file... {0}", ex);
            }

            IXmlSerializationService serializationService = new XmlSerializationService();
            message = serializationService.Deserialize<Message>(stream);

            stream.Close();
        }

        private static IContainer BuildContainer()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<ConsoleFM35Module>();

            return containerBuilder.Build();
        }
    }
}
