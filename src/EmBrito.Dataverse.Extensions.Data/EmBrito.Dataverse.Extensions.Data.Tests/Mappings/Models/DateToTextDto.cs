using EmBrito.Dataverse.Extensions.Data.Mappings;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings.Models
{
    public class DateToTextDto : EntityMapper<DateToTextDto>
    {
        public DateToTextDto(Entity entity) : base(entity)
        {
        }

        [FromDateTimeColumn(Constants.DateTimeColumn)]
        public string DateTime { get; set; }

        [FromStringColumn("dateastext")]
        public DateTime? DateFromText { get; set; }

    }
}
