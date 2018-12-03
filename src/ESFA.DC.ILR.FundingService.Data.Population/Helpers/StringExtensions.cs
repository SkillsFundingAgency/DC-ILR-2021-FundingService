using System;
using System.Collections.Generic;
using System.Linq;

namespace ESFA.DC.ILR.FundingService.Data.Population.Helpers
{
    public static class StringExtensions
    {
        public static bool CaseInsensitiveEquals(this string source, string data)
        {
            return source.Equals(data, StringComparison.OrdinalIgnoreCase);
        }
    }
}