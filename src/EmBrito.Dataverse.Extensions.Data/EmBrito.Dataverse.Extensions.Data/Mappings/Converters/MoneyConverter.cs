using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal class MoneyConverter : ValueConverter
    {
        public override Type SourceType => typeof(Money);

        protected override Type[] RestrictDestinationTypes => new Type[] { };

        protected override object GetComplexAttributeValue(object value)
        {
            return ((Money)value)?.Value;
        }

    }
}
