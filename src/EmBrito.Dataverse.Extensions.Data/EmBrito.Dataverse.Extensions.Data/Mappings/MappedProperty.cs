using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings
{
    public class MappedProperty
    {

        public PropertyInfo PropertyInfo { get; private set; }
        public ColumnMappingAttribute MappingAttribute { get; private set; }

        internal MappedProperty(ColumnMappingAttribute mapingAttribute, PropertyInfo propertyInfo)
        {
            _ = mapingAttribute ?? throw new ArgumentNullException(nameof(mapingAttribute));
            _ =propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
            PropertyInfo = propertyInfo;
            MappingAttribute = mapingAttribute;
        }

    }
}
