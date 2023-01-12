using EmBrito.Dataverse.Extensions;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings
{
    public class FormattedValueConverter
    {

        public string[] GetFormattedValueAsArray(string attributeLogicalName, Entity entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrWhiteSpace(attributeLogicalName)) throw new  ArgumentNullException(nameof(attributeLogicalName));

            string value = entity.GetFormattedValue(attributeLogicalName);
            string[] result = null;

            if(value != null)
            {
                result = value
                    .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToArray();
            }

            return result;
        }

    }
}
