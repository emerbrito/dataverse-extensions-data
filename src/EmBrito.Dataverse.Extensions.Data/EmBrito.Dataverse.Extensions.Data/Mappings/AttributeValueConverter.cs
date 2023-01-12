using EmBrito.Dataverse.Extensions.Data.Mappings.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings
{
    public class AttributeValueConverter
    {
        private static Lazy<Dictionary<Type, IValueConverter>> _convetersHolder = new Lazy<Dictionary<Type, IValueConverter>>(() => LoadConveters());
        private static Dictionary<Type, IValueConverter> _conveters = _convetersHolder.Value;

        private static Dictionary<Type, IValueConverter> LoadConveters()
        {

            var dict = new Dictionary<Type, IValueConverter>();
            var converterInterface = typeof(IValueConverter);
            var converterTypes = typeof(AttributeValueConverter)
                .Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && (converterInterface.IsAssignableFrom(t) || t.IsSubclassOf(typeof(ValueConverter))))
                .ToList();

            foreach (var type in converterTypes)
            {
                var instance = Activator.CreateInstance(type) as IValueConverter;
                dict[instance.SourceType] = instance;
            }

            return dict;
        }

        public object Convert(object value, Type destinationType, string format = null)
        {
            _ = destinationType ?? throw new ArgumentNullException(nameof(destinationType));

            // if value is null we return null for nullable properties of or the default for type.
            if (value is null)
            {
                return destinationType.IsValueType ? Activator.CreateInstance(destinationType) : null;
            }

            // error if no converter is available;
            if (!_conveters.TryGetValue(value.GetType(), out var converter))
            {
                throw new InvalidOperationException($"A value converter was not found for value type: {value.GetType().Name} and destination type: {destinationType.Name}.");
            }

            // convert value
            return converter.Convert(value, destinationType, format);
        }
    }
}
