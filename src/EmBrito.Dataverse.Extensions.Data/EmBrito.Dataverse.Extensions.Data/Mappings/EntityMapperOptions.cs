using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings
{
    public class EntityMapperOptions
    {

        private readonly Dictionary<string, Func<MappedProperty, Entity, object>> _customConverters;

        internal Dictionary<string, Func<MappedProperty, Entity, object>> CustomConverters { get => _customConverters; }

        public EntityMapperOptions()
        {
            _customConverters = new Dictionary<string, Func<MappedProperty, Entity, object>>();
        }

        public void AddConverter(string attributeLogicalName, Func<MappedProperty, Entity, object> converter)
        {
            if(string.IsNullOrWhiteSpace(attributeLogicalName)) throw new ArgumentNullException(nameof(attributeLogicalName));
            _ = converter ?? throw new ArgumentNullException(nameof(converter));

            _customConverters[attributeLogicalName] = converter;
        }


    }
}
