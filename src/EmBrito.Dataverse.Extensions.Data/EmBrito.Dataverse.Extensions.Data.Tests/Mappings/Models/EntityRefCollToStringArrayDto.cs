using EmBrito.Dataverse.Extensions.Data.Mappings;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings.Models
{
    public class EntityRefCollToStringArrayDto : EntityMapper<EntityRefCollToStringArrayDto>
    {
        public EntityRefCollToStringArrayDto(Entity entity) : base(entity)
        {
        }

        [FromEntityReferenceCollectionColumn(Constants.EntityReferenceCollectionColumn)]
        public string[] EntityRefColl { get; set; }

    }
}
