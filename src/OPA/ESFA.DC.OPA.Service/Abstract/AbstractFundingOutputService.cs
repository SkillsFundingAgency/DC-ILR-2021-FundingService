using System;
using System.Collections.Generic;
using System.Text;
using ESFA.DC.OPA.Model.Interface;

namespace ESFA.DC.OPA.Service.Abstract
{
    public abstract class AbstractFundingOutputService
    {
        private const string Uncertain = "uncertain";
        private const string True = "true";

        protected string GetAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null && attribute.ToString() != Uncertain)
            {
                return attribute.ToString();
            }

            return null;
        }

        protected int? GetIntAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null && attribute.ToString() != Uncertain)
            {
                return int.Parse(attribute.ToString());
            }

            return null;
        }

        protected bool? GetBoolAttributeValue(IDataEntity dataEntity, string attributeName)
        {
            var attribute = dataEntity.Attributes[attributeName].Value;

            if (attribute != null && attribute.ToString() != Uncertain)
            {
                return attribute.ToString() == True;
            }

            return null;
        }
    }
}
