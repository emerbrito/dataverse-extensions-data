using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal class EntityCollectionToPrimitiveList : ValueCollectionToListConveter<EntityCollection, Guid>
    {
        protected override int GetCount(EntityCollection collection)
        {
            return collection.Entities.Count;
        }

        protected override Guid GetValue(EntityCollection collection, int index)
        {
            return collection.Entities[index].Id;
        }
    }
}
