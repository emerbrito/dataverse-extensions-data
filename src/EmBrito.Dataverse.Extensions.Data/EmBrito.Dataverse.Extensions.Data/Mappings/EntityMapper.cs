using EmBrito.Dataverse.Extensions;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings
{
    public abstract class EntityMapper<TDerivedMappings> where TDerivedMappings : EntityMapper<TDerivedMappings>
    {

        #region Field Declarations
        private readonly EntityMapperOptions _options = new EntityMapperOptions();
        private AttributeValueConverter _valueConverter = new AttributeValueConverter();
        private FormattedValueConverter _formattedValueConverter = new FormattedValueConverter();
        private static Lazy<List<string>> _mappedAttributeNames = new Lazy<List<string>>(() =>
        {
            return _mappedProperties
                .Value
                .Select(m => m.MappingAttribute.LogicalName)
                .ToList();
        });
        private static Lazy<MappedPropertyCollection> _mappedProperties = new Lazy<MappedPropertyCollection>(() =>
        {
            var col = new MappedPropertyCollection();
            typeof(TDerivedMappings)
                .GetProperties()
                .Select(p => new { Prop = p, Att = p.GetCustomAttributes<ColumnMappingAttribute>().FirstOrDefault() })
                .Where(x => x.Att != null)
                .Select(x => new MappedProperty(x.Att, x.Prop))
                .ToList()
                .ForEach(x =>
                {
                    if (col.Contains(x.MappingAttribute.LogicalName))
                    {
                        throw new InvalidOperationException($"Error when mapping attributes. Attribute {x.MappingAttribute.LogicalName} was mapped to more than one property. Each entity attribute should be mapped to a single property.");
                    }
                    col.Add(x);
                });
            return col;
        });

        #endregion

        #region Public Constructors

        public EntityMapper(Entity entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            MapEntity(entity);
        }

        #endregion

        #region Public Mehtods

        /// <summary>
        /// List of mapped attributes.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> MappedAttributeNames() => _mappedAttributeNames.Value;

        #endregion

        #region Protected Methods

        protected virtual void Configure(EntityMapperOptions options)
        {
        }

        #endregion

        #region Internal Implementation

        private void MapEntity(Entity entity)
        {
            Configure(_options);

            foreach (var mappedProperty in _mappedProperties.Value)
            {
                MapAttribute(mappedProperty, entity);
            }
        }

        private void MapAttribute(MappedProperty mappedProperty, Entity entity)
        {
            if(mappedProperty.MappingAttribute.FormattedValue)
            {
                mappedProperty.PropertyInfo.SetValue(this, GetFormattedValue(mappedProperty, entity));
            }
            else
            {
                var value = GetAttributeValue(mappedProperty.MappingAttribute.LogicalName, entity);

                if (_options.CustomConverters.ContainsKey(mappedProperty.MappingAttribute.LogicalName))
                {
                    var customConverter = _options.CustomConverters[mappedProperty.MappingAttribute.LogicalName];                    
                    mappedProperty.PropertyInfo.SetValue(this, customConverter(mappedProperty, entity));
                }
                else
                {
                    mappedProperty.PropertyInfo.SetValue(this, _valueConverter.Convert(value, mappedProperty.PropertyInfo.PropertyType, mappedProperty.MappingAttribute.StringFormat));
                }                
            }
        }

        private object GetFormattedValue(MappedProperty mappedProperty, Entity entity)
        {
            if(mappedProperty.MappingAttribute.ColumnType == typeof(EntityReferenceCollection))
            {
                return GetFormattedValueFromEntityRefs(mappedProperty, entity);
            }

            if (mappedProperty.PropertyInfo.PropertyType == typeof(string))
            {
                return entity.GetFormattedValue(mappedProperty.MappingAttribute.LogicalName);
            }
            else if (mappedProperty.PropertyInfo.PropertyType == typeof(string[]))
            { 
                return _formattedValueConverter.GetFormattedValueAsArray(mappedProperty.MappingAttribute.LogicalName, entity);
            }
            else if (mappedProperty.PropertyInfo.PropertyType == typeof(List<string>))
            {
                var array = _formattedValueConverter.GetFormattedValueAsArray(mappedProperty.MappingAttribute.LogicalName, entity);

                if(array is null)
                {
                    return null;
                }
                else
                {
                    return array.ToList();
                }
            }
            else
            {
                throw new InvalidOperationException($"When convering a formatted value only string, string[] and List<string> are supported. Property name: {mappedProperty.PropertyInfo.Name}.");
            }
        }

        private object GetFormattedValueFromEntityRefs(MappedProperty mappedProperty, Entity entity)
        {
            var values = entity.GetAttributeValue<EntityReferenceCollection>(mappedProperty.MappingAttribute.LogicalName);
            string[] names = values != null && values.Count > 0 ? values.Select(e => e.Name ?? e.Id.ToString()).ToArray() : null;

            if (mappedProperty.PropertyInfo.PropertyType == typeof(string))
            {
                return names != null ? string.Join(", ", names) : null;
            }
            else if (mappedProperty.PropertyInfo.PropertyType == typeof(string[]))
            {
                return names;
            }
            else if (mappedProperty.PropertyInfo.PropertyType == typeof(List<string>))
            {
                return names != null ? names.ToList() : null;
            }
            else
            {
                throw new InvalidOperationException($"When convering an entity refeence collection to formatted value, only string, string[] and List<string> are supported. Property name: {mappedProperty.PropertyInfo.Name}.");
            }
        }

        private object GetAttributeValue(string logicalName, Entity entity)
        {

            if(!entity.Contains(logicalName))
            {
                return null;
            }

            var value = entity[logicalName];

            if(value is AliasedValue)
            {
                return ((AliasedValue)value).Value;
            }

            return entity[logicalName];
        }

        #endregion

    }
}
