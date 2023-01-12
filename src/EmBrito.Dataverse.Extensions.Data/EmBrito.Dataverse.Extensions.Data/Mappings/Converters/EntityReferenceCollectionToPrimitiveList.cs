using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal class EntityReferenceCollectionToPrimitiveList : ValueCollectionToListConveter<EntityReferenceCollection, Guid>
    {
        protected override int GetCount(EntityReferenceCollection collection)
        {
            return collection.Count;
        }

        protected override Guid GetValue(EntityReferenceCollection collection, int index)
        {
            return collection[index].Id;
        }
    }
}
