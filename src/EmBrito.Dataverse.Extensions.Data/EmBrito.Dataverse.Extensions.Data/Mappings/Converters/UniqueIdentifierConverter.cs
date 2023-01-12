using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal class UniqueIdentifierConverter : ValueConverter
    {
        public override Type SourceType => typeof(Guid);

        protected override Type[] RestrictDestinationTypes => new[] { typeof(string) };

        protected override object ConvertValue(object value, Type destinationType, string format = null)
        {
            return value.ToString();
        }
    }
}
