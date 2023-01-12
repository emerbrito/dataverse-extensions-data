using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal class EntityReferenceCollectionConverter : ValueConverter
    {

        public override Type SourceType => typeof(EntityReferenceCollection);

        protected override Type[] RestrictDestinationTypes => new Type[] { typeof(List<Guid>), typeof(Guid[]), typeof(List<string>), typeof(string[]) };

        protected override object ConvertValue(object value, Type destinationType, string format = null)
        {
            var listConverter = new EntityReferenceCollectionToPrimitiveList();
            return listConverter.ConvertValue((EntityReferenceCollection)value, destinationType);
        }

    }
}
