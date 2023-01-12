using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal class OptionSetCollectionToPrimitiveList : ValueCollectionToListConveter<OptionSetValueCollection, int>
    {
        protected override int GetCount(OptionSetValueCollection collection)
        {
            return collection.Count;
        }

        protected override int GetValue(OptionSetValueCollection collection, int index)
        {
            return collection[index].Value;
        }
    }
}
