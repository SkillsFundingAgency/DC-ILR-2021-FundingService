using System;
using System.Globalization;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using Oracle.Determinations.Masquerade.Util;

namespace ESFA.DC.OPA.Service
{
    public class DataEntityAttributeService : IDataEntityAttributeService
    {
        private const string Uncertain = "uncertain";
        private const string True = "true";

        public object GetAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null)
            {
                var attributeString = attribute.ToString();

                if (attributeString != Uncertain)
                {
                    return attribute;
                }
            }

            return null;
        }

        public string GetStringAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null)
            {
                var attributeString = attribute.ToString();

                if (attributeString != Uncertain)
                {
                    return attributeString;
                }
            }

            return null;
        }

        public int? GetIntAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null)
            {
                var attributeString = attribute.ToString();

                if (attributeString != Uncertain)
                {
                    return int.Parse(attributeString, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);
                }
            }

            return null;
        }

        public long? GetLongAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null)
            {
                var attributeString = attribute.ToString();

                if (attributeString != Uncertain)
                {
                    return long.Parse(attributeString, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);
                }
            }

            return null;
        }

        public decimal? GetDecimalAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null)
            {
                var attributeString = attribute.ToString();

                if (attributeString != Uncertain)
                {
                    return decimal.Parse(attributeString);
                }
            }

            return null;
        }

        public bool? GetBoolAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null)
            {
                var attributeString = attribute.ToString();

                if (attributeString != Uncertain)
                {
                    return attributeString == True;
                }
            }

            return null;
        }

        public DateTime? GetDateTimeAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null && attribute.ToString() != Uncertain)
            {
                return ((Date)attribute).GetDateTime();
            }

            return null;
        }
    }
}
