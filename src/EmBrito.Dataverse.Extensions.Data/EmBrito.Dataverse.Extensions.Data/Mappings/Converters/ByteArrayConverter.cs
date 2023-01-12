using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal class ByteArrayConverter : ValueConverter
    {
        public override Type SourceType => typeof(byte[]);

        protected override Type[] RestrictDestinationTypes => new Type[] { typeof(string) };

        protected override object ConvertValue(object value, Type destinationType, string format = null)
        {
            return System.Convert.ToBase64String((byte[])value);
        }
    }
}
