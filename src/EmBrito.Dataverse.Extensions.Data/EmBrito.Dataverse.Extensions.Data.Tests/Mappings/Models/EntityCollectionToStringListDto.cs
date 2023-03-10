using EmBrito.Dataverse.Extensions.Data.Mappings;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings.Models
{
    public class EntityCollectionToStringListDto : EntityMapper<EntityCollectionToStringListDto>
    {
        public EntityCollectionToStringListDto(Entity entity) : base(entity)
        {
        }

        [FromEntityCollectionColumn(Constants.ActivityPartyColumn)]
        public List<string> ActivityParty { get; set; }

    }
}
