using EmBrito.Dataverse.Extensions.Data.Mappings;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings.Models
{
    public class EntityRefCollToFormattedValueStringDto : EntityMapper<EntityRefCollToFormattedValueStringDto>
    {
        public EntityRefCollToFormattedValueStringDto(Entity entity) : base(entity)
        {
        }

        [FromEntityReferenceCollectionColumn(Constants.EntityReferenceCollectionColumn, formattedValue: true)]
        public string EntityRefColl { get; set; }

    }
}
