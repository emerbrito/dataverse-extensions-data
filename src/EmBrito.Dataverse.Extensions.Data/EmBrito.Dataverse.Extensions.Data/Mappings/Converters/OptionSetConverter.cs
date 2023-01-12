using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal class OptionSetConverter : ValueConverter
    {
        public override Type SourceType => typeof(OptionSetValue);

        protected override Type[] RestrictDestinationTypes => new Type[] { };

        protected override object GetComplexAttributeValue(object value)
        {
            return ((OptionSetValue)value).Value;
        }

    }
}
