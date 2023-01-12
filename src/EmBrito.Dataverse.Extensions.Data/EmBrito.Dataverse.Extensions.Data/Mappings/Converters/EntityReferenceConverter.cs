using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal class EntityReferenceConverter : ValueConverter
    {
        public override Type SourceType => typeof(EntityReference);

        protected override Type[] RestrictDestinationTypes => new Type[] { typeof(Guid), typeof(Guid?), typeof(string) };

        protected override object ConvertValue(object value, Type destinationType, string format = null)
        {
            if (destinationType.GetTypeInfo().IsGenericType && destinationType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return ConvertValue(value, Nullable.GetUnderlyingType(destinationType));
            }

            var eref = ((EntityReference)value);

            if (destinationType == typeof(Guid)) 
            {
                return eref.Id;
            }

            return eref.Id.ToString();
        }
    }
}
