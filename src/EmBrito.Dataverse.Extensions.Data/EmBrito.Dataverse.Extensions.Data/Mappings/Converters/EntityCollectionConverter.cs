using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal class EntityCollectionConverter : ValueConverter
    {

        public override Type SourceType => typeof(EntityCollection);

        protected override Type[] RestrictDestinationTypes => new Type[] { typeof(List<Guid>), typeof(Guid[]), typeof(List<string>), typeof(string[]) };

        protected override object ConvertValue(object value, Type destinationType, string format = null)
        {
            var listConverter = new EntityCollectionToPrimitiveList();
            return listConverter.ConvertValue((EntityCollection)value, destinationType);
        }

    }
}
