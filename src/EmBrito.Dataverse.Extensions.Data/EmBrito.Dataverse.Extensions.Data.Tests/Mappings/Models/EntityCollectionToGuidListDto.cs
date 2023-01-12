using EmBrito.Dataverse.Extensions.Data.Mappings;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings.Models
{
    public class EntityCollectionToGuidListDto : EntityMapper<EntityCollectionToGuidListDto>
    {
        public EntityCollectionToGuidListDto(Entity entity) : base(entity)
        {
        }

        [FromEntityCollectionColumn(Constants.ActivityPartyColumn)]
        public List<Guid> ActivityParty { get; set; }

    }
}
