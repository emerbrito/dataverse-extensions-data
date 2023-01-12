using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal abstract class ValueConverter : IValueConverter
    {

        public abstract Type SourceType { get; }

        protected abstract Type[] RestrictDestinationTypes { get; }

        public object Convert(object value, Type destinationType, string format = null)
        {
            _ = destinationType ?? throw new  ArgumentNullException(nameof(destinationType));

            // if value is null we return null for nullable properties of or default value types.

            if (value is null)
            {
                return destinationType.IsValueType ? Activator.CreateInstance(destinationType) : null;
            }

            var valueType = value.GetType();

            // validate input type in a way that Nullable<T> and T are considered the same.

            if (!(valueType.IsAssignableFrom(SourceType) || SourceType.IsAssignableFrom(valueType)))
            {
                throw new InvalidOperationException($"Attribute converter {GetType().Name} was expecting a valur of type {SourceType.Name} but {valueType} was received instead.");
            }

            // bypass convertion if input and output are of the same type,
            // considers Nullable<T> and T to be the same

            if (valueType.IsAssignableFrom(destinationType) || destinationType.IsAssignableFrom(valueType))
            {
                return value;
            }

            // validate output type

            if (RestrictDestinationTypes != null && RestrictDestinationTypes.Any() && !RestrictDestinationTypes.Contains(destinationType))
            {
                throw new InvalidOperationException($"Attribute converter {GetType().Name} cannot convert from {SourceType.Name} to {destinationType.Name}. The supported destination types are {string.Join(", ", RestrictDestinationTypes.Select(t => t.Name).ToArray())}.");
            }


            return ConvertValue(value, destinationType, format);
        }

        /// <summary>
        /// Convert value. Here is safe to assume types were already validated, value is not null and value type and destination types are different.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        protected virtual object ConvertValue(object value, Type destinationType, string format = null)
        {

            if (destinationType.GetTypeInfo().IsGenericType && destinationType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return ConvertValue(value, Nullable.GetUnderlyingType(destinationType));
            }

            // when overriden, extracts primitive value from complex attributes.
            value = GetComplexAttributeValue(value);

            var valueType = value.GetType();

            if(valueType != typeof(string) && destinationType == typeof(string) && !string.IsNullOrWhiteSpace(format))
            {
                return ApplyStringFormat(value, format);
            }

            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
            object result;

            if (converter.CanConvertFrom(valueType))
            {
                try
                {
                    result = converter.ConvertFrom(value);
                }
                catch (Exception innerException)
                {
                    throw new InvalidOperationException($"Converter {this.GetType().Name} is unable to convert value to expected type: {destinationType.FullName}. Value: {value}.", innerException);
                }
            }
            else
            {
                try
                {
                    result = System.Convert.ChangeType(value, destinationType);
                }
                catch (Exception innerException)
                {
                    throw new InvalidOperationException($"{nameof(System.Convert.ChangeType)}() failed on converter {this.GetType().Name} when attempting to convert a value to: {destinationType.FullName}. Value: {value}.", innerException);
                }
            }

            return result;
        }

        protected virtual object GetComplexAttributeValue(object value)
        {
            return value;
        }

        private object ApplyStringFormat(object value, string format)
        {
            var valueType = value.GetType();
            var method = valueType.GetMethod("ToString", new Type[] { typeof(string) });

            if(method != null)
            {
                return method.Invoke(value, new object[] { format });
            }

            method = valueType.GetMethod("ToString");
            return method.Invoke(value, null);
        }
    }
}
