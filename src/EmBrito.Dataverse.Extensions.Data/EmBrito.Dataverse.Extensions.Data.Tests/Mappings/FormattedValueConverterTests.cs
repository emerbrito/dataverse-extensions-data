using EmBrito.Dataverse.Extensions.Data.Mappings;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings
{
    public class FormattedValueConverterTests
    {

        [Fact]
        public void Value_Not_Found_Returns_Null()
        {
            var converter = new FormattedValueConverter();
            var entity = new Entity();

            Assert.Null(converter.GetFormattedValueAsArray("subscriptions", entity));
        }

        [Fact]
        public void No_Separator_Returns_Single_Value_Array()
        {
            var converter = new FormattedValueConverter();
            var entity = new Entity();

            entity.FormattedValues.Add("subscriptions", "Email");

            Assert.Single(converter.GetFormattedValueAsArray("subscriptions", entity)!);
        }

        [Fact]
        public void Comma_Separator_Returns_Multiple_Value_Array()
        {
            var converter = new FormattedValueConverter();
            var entity = new Entity();

            entity.FormattedValues.Add("subscriptions", "Email, Phone Number");

            Assert.Equal(2, converter.GetFormattedValueAsArray("subscriptions", entity)!.Length);
        }

    }
}
