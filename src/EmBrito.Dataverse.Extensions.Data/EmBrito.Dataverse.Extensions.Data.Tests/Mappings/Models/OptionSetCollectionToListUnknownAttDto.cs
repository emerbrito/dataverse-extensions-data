using EmBrito.Dataverse.Extensions.Data.Mappings;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings.Models
{
    public class OptionSetCollectionToListUnknownAttDto : EntityMapper<OptionSetCollectionToListUnknownAttDto>
    {
        public OptionSetCollectionToListUnknownAttDto(Entity entity) : base(entity)
        {
        }

        [FromOptionSetCollectionColumn("_not_to_be_found_")]
        public List<int> MultiChoices { get; set; }

    }
}
