using EmBrito.Dataverse.Extensions.Data.Mappings;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings.Models
{
    public class DefaultEntityNoNullablesDto : EntityMapper<DefaultEntityNoNullablesDto>
    {
        public DefaultEntityNoNullablesDto(Entity entity) : base(entity)
        {
        }

        [FromUniqueIdentifierColumn(Constants.EntityIdColumn1)]
        public Guid Id { get; set; }

        [FromStringColumn(Constants.AccountNameColumn)]
        public string AccountName { get; set; }

        [FromEntityCollectionColumn(Constants.ActivityPartyColumn)]
        public Guid[] ActivityParty { get; set; }

        [FromStringColumn(Constants.AutoNumberColumn)]
        public string AutoNumber { get; set; }

        [FromOptionSetColumn(Constants.ChoiceColumn)]
        public int Choice { get; set; }

        [FromCurrencyColumn(Constants.CurrencyColumn)]
        public decimal Currency { get; set; }

        [FromDateTimeColumn(Constants.DateTimeColumn)]
        public DateTime DateTime { get; set; }

        [FromDecimalColumn(Constants.DecimalColumn)]
        public decimal Decimal { get; set; }

        [FromIntegerColumn(Constants.DurationColumn)]
        public int Duration { get; set; }

        [FromEntityReferenceColumn(Constants.EntityReferenceColumn)]
        public Guid EnitityReference { get; set; }

        [FromDoubleColumn(Constants.FloatColumn)]
        public double Float { get; set; }

        [FromImageColumn(Constants.ImageColumn)]
        public byte[] Image { get; set; }

        [FromOptionSetCollectionColumn(Constants.MultiChoicesColumn)]
        public int[] MultiChoices { get; set; }

        [FromBooleanColumn(Constants.TwoOptionsColumn)]
        public bool TwoOptions { get; set; }

        [FromIntegerColumn(Constants.WholeNumberColumn)]
        public int WholeNumber { get; set; }

        [FromEntityReferenceCollectionColumn(Constants.EntityReferenceCollectionColumn)]
        public Guid[] EntityRefColl { get; set; }

    }
}
