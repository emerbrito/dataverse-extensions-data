using EmBrito.Dataverse.Extensions.Data.Mappings;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings.Models
{
    public class OptionSetCollectionToFormattedValueDto : EntityMapper<OptionSetCollectionToFormattedValueDto>
    {
        public OptionSetCollectionToFormattedValueDto(Entity entity) : base(entity)
        {
        }

        [FromOptionSetCollectionColumn(Constants.MultiChoicesColumn, formattedValue: true)]
        public string MultiChoices { get; set; }

    }
}
