using ESFA.DC.ILR.FundingService.Config.Interfaces;

namespace ESFA.DC.ILR.FundingService.Config
{
    public class LoggerConfig : ILoggerConfig
    {
        public string LoggerConnectionstring { get; set; }
    }
}
