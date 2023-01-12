using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal class OptionSetCollectionConverter : ValueConverter
    {

        public override Type SourceType => typeof(OptionSetValueCollection);

        protected override Type[] RestrictDestinationTypes => new Type[] { typeof(List<int>), typeof(int[]), typeof(List<string>), typeof(string[]) };

        protected override object ConvertValue(object value, Type destinationType, string format = null)
        {
            var listConverter = new OptionSetCollectionToPrimitiveList();
            return listConverter.ConvertValue((OptionSetValueCollection)value, destinationType);
        }

    }
}
