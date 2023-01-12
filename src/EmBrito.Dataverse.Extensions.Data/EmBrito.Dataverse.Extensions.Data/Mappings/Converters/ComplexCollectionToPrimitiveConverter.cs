using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Mappings.Converters
{
    internal abstract class ValueCollectionToListConveter<TAttributeCollection, TValue> where TAttributeCollection : class
    {

        public object ConvertValue(TAttributeCollection collection, Type destinationType)
        {
            if (destinationType.GetTypeInfo().IsGenericType && destinationType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return ConvertValue(collection, Nullable.GetUnderlyingType(destinationType));
            }

            var underlyingType = destinationType.IsArray ? destinationType.GetElementType() : destinationType.GetGenericArguments()[0];

            Array array = Array.CreateInstance(underlyingType, GetCount(collection));

            for (int i = 0; i < GetCount(collection); i++)
            {
                var entryValue = GetValue(collection, i);
                
                if(underlyingType == typeof(TValue))
                {
                    array.SetValue(entryValue, i);
                }
                else
                {
                    array.SetValue(entryValue.ToString(), i);
                }
            }

            if (!destinationType.IsArray)
            {
                Type genericListType = typeof(List<>);
                Type concreteListType = genericListType.MakeGenericType(underlyingType);

                return Activator.CreateInstance(concreteListType, array);
            }

            return array;
        }

        protected abstract int GetCount(TAttributeCollection collection);

        protected abstract TValue GetValue(TAttributeCollection collection, int index);

    }
}
