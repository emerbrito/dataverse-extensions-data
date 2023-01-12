using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings
{
    public class MappedPropertyCollection : KeyedCollection<string, MappedProperty>
    {
        protected override string GetKeyForItem(MappedProperty item)
        {
            return item.MappingAttribute.LogicalName;
        }
    }
}
