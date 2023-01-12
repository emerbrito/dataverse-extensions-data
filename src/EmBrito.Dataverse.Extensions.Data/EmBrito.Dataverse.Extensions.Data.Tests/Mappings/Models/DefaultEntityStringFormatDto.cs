using EmBrito.Dataverse.Extensions.Data.Mappings;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings.Models
{
    public class DefaultEntityStringFormatDto : EntityMapper<DefaultEntityStringFormatDto>
    {
        public DefaultEntityStringFormatDto(Entity entity) : base(entity)
        {
        }

        [FromCurrencyColumn(Constants.CurrencyColumn, format: "C3")]
        public string Currency { get; set; }

        [FromDateTimeColumn(Constants.DateTimeColumn, format: "MM-dd-yy")]
        public string DateTime { get; set; }

    }
}
