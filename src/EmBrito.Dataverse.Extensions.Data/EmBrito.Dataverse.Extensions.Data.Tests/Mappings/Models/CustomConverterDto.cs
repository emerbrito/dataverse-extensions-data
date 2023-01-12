using EmBrito.Dataverse.Extensions.Data.Mappings;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings.Models
{
    public class CustomConverterDto : EntityMapper<CustomConverterDto>
    {
        public CustomConverterDto(Entity entity) : base(entity)
        {
        }

        [FromOptionSetColumn(Constants.ChoiceColumn)]
        public string Gender { get; set; }

        protected override void Configure(EntityMapperOptions options)
        {
            options.AddConverter(Constants.ChoiceColumn, ConvertGender);
        }

        object ConvertGender(MappedProperty mappingInfo, Entity entity)
        {
            var attributeName = mappingInfo.MappingAttribute.LogicalName;
            var choice = entity.GetAttributeValue<OptionSetValue>(attributeName);
            string value = "Not Specified";

            switch (choice.Value)
            {
                case 1:
                    value = "Custom Value 1";
                    break;  
                case 2:
                    value = "Custom Value 2";
                    break;
            }

            return value;
        }

    }
}
