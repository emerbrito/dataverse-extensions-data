using EmBrito.Dataverse.Extensions.Data.Mappings;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings.Models
{
    public class EntityCollectionToListUnknowAttDto : EntityMapper<EntityCollectionToListUnknowAttDto>
    {
        public EntityCollectionToListUnknowAttDto(Entity entity) : base(entity)
        {
        }

        [FromEntityCollectionColumn("_not_to_be_found_")]
        public List<string> ActivityParty { get; set; }

    }
}
