using Microsoft.Xrm.Sdk;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions
{

    /// <summary>
    /// These are copies from the extension methods project, until we figure a way to proper 
    /// share them between frameworks.
    /// </summary>
    internal static class EntityExtensionsInternal
    {

        /// <summary>
        /// Append attributes from another entity, without changing existing attribute values, if an attribute with the same name already exist it is ignored.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <param name="fromEntity">The entity to copy attributes from.</param>
        /// <returns>True is at least one attributed was added.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool AppendAttributes(this Entity entity, Entity fromEntity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var hasChange = false;

            if (fromEntity == null)
            {
                return hasChange;
            }

            foreach (var att in fromEntity.Attributes)
            {
                if (!entity.Contains(att.Key))
                {
                    hasChange = true;
                    entity.Attributes.Add(att.Key, att.Value);
                }
            }

            return hasChange;
        }

        /// <summary>
        /// Compare the the entity attribute too a value.
        /// When comparing reference types, their primitive values will be used. When comparing collections, the primitive
        /// type of the collection item will be used.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <param name="attributeLogicalName">Name of the attribute the value is based on.</param>
        /// <param name="attributeB">The value to compare.</param>
        /// <returns></returns>
        public static bool AttributeEquals(this Entity entity, string attributeLogicalName, Object otherAttribute)
        {

            object attributeX = null;

            if (entity.Contains(attributeLogicalName))
            {
                attributeX = entity.Attributes[attributeLogicalName];
            }

            return CompareValues(attributeX, otherAttribute);
        }

        /// <summary>
        /// Compare the the entity attribute too a value.
        /// When comparing reference types, their primitive values will be used. When comparing collections, the primitive
        /// type of the collection item will be used.
        /// </summary>
        /// <typeparam name="TEntity">The entity instance.</typeparam>
        /// <typeparam name="TReturn">The attribute value.</typeparam>
        /// <param name="entity"></param>
        /// <param name="attributeA">An expression that returns the attribute value from the entity instance.</param>
        /// <param name="attributeB">The value to compare.</param>
        /// <returns></returns>
        public static bool AttributeEquals<TEntity, TReturn>(this TEntity entity, Expression<Func<TEntity, TReturn>> attributeA, Object otherAttribute) where TEntity : Entity
        {
            Object attributeX = null;
            string logicalName = LogicalNameFromProperty(entity, attributeA);

            if (!string.IsNullOrEmpty(logicalName) && entity.Contains(logicalName))
            {
                attributeX = entity.Attributes[logicalName];
            }

            return CompareValues(attributeX, otherAttribute);
        }

        /// <summary>
        /// Copy all atributes from another entity replacing any attributes with the same name.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <param name="fromEntity">The entity to copy attributes from.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CopyAttributes(this Entity entity, Entity fromEntity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            if (fromEntity == null)
            {
                return;
            }

            foreach (var att in fromEntity.Attributes)
            {
                entity.Attributes[att.Key] = att.Value;
            }
        }

        /// <summary>
        /// Returns a deep clone of the entity and attributes. Read only properties may be ignored
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <returns></returns>
        public static Entity Clone(this Entity entity)
        {

            if (entity is null)
            {
                return null;
            }

            var clone = new Entity(entity.LogicalName, entity.Id);

            CopyReadWriteProperties(entity, clone, propertyExceptions: nameof(entity.Attributes));
            entity.FormattedValues
                .ToList()
                .ForEach(v => clone.FormattedValues.Add(v));

            foreach (var att in entity.Attributes)
            {
                clone.Attributes.Add(att.Key, CloneValue(entity, att.Key));
            }

            return clone;
        }

        /// <summary>
        /// Clones the attribute value if it is a reference type, otherwise returns the value type.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <param name="attributeLogicalName">Name of the attribute the value is based on.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>The cloned value.</returns>
        public static object CloneValue(this Entity entity, string attributeLogicalName)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            if (!entity.Contains(attributeLogicalName) || entity[attributeLogicalName] is null)
            {
                return default;
            }

            object value;

            if (entity[attributeLogicalName] is AliasedValue aliasedValue)
            {
                value = aliasedValue.Value;
            }
            else
            {
                aliasedValue = null;
                value = entity[attributeLogicalName];
            }

            switch (value)
            {
                case BooleanManagedProperty booleanManaged:
                    var booleanManagedNew = new BooleanManagedProperty(booleanManaged.Value);
                    CopyReadWriteProperties(booleanManaged, booleanManagedNew);
                    return booleanManagedNew;

                case EntityCollection entityCol:
                    return CloneEntityCollection(entityCol);

                case EntityReference entityRef:
                    var entityRefNew = new EntityReference(entityRef.LogicalName, entityRef.Id);
                    CopyReadWriteProperties(entityRef, entityRefNew);
                    return entityRefNew;

                case EntityReferenceCollection entityRefCol:
                    return CloneEntityReferenceCollection(entityRefCol);

                case Money money:
                    var moneyNew = new Money(money.Value);
                    CopyReadWriteProperties(money, moneyNew);
                    return moneyNew;

                case OptionSetValueCollection optionSetCol:
                    return CloneOptionSetCollection(optionSetCol);

                case OptionSetValue optionSet:
                    var optionSetNew = new OptionSetValue(optionSet.Value);
                    CopyReadWriteProperties(optionSet, optionSetNew);
                    return optionSetNew;

            }

            if (aliasedValue != null)
            {
                value = new AliasedValue(aliasedValue.EntityLogicalName, aliasedValue.AttributeLogicalName, value)
                {
                    ExtensionData = aliasedValue.ExtensionData
                };
            }

            return value;

        }

        /// <summary>
        /// Gets the value from an AliasedValue attribute or null of no attribute is found.
        /// </summary>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="entity">The entity instance.</param>
        /// <param name="entityLogicalName">Name of the entity that the attribute belongs to.</param>
        /// <param name="attributeLogicalName">Name of the attribute the value is based on.</param>
        /// <returns>Attribute value or null</returns>
        public static T GetAliasedValue<T>(this Entity entity, string entityLogicalName, string attributeLogicalName)
        {
            if (string.IsNullOrWhiteSpace(entityLogicalName)) throw new ArgumentNullException(nameof(entityLogicalName));
            if (string.IsNullOrWhiteSpace(attributeLogicalName)) throw new ArgumentNullException(nameof(attributeLogicalName));
            return GetAliasedValue<T>(entity, $"{entityLogicalName}.{attributeLogicalName}");
        }

        /// <summary>
        /// Gets the value from an AliasedValue attribute or null of no attribute is found.
        /// </summary>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="entity">The entity instance.</param>
        /// <param name="attributeAlias">Alias of the attribute the value is based on.</param>
        /// <returns>Attribute value or null</returns>
        public static T GetAliasedValue<T>(this Entity entity, string attributeAlias)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrWhiteSpace(attributeAlias)) throw new ArgumentNullException(nameof(attributeAlias));

            if (!entity.Contains(attributeAlias) || entity[attributeAlias] is null)
            {
                return default(T);
            }

            var attValue = entity[attributeAlias];

            if (attValue is AliasedValue)
            {
                return (T)((AliasedValue)attValue).Value;
            }

            return (T)attValue;
        }

        /// <summary>
        /// Returns the corresponding formatted value or null.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <param name="attributeLogicalName">Name of the attribute the value is based on.</param>
        /// <returns>Formatted value or null.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetFormattedValue(this Entity entity, string attributeLogicalName)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrWhiteSpace(attributeLogicalName)) throw new ArgumentNullException(nameof(attributeLogicalName));

            if (entity.FormattedValues != null)
            {
                if (entity.FormattedValues.ContainsKey(attributeLogicalName))
                {
                    return entity.FormattedValues[attributeLogicalName];
                }
            }

            return null;
        }

        /// <summary>
        /// Compare the entity logical name and attribute values. If an attribute is a reference type, 
        /// for example an option set or entity reference it will compare the option set value and the
        /// entity reference id.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityToCompare"></param>
        /// <returns></returns>
        public static bool IsEqualTo(this Entity entity, Entity entityToCompare)
        {

            if (entity == entityToCompare) return true;
            if (entity is null || entityToCompare is null) return false;
            if (entity.LogicalName != entityToCompare.LogicalName) return false;
            if (entity.Attributes.Count != entityToCompare.Attributes.Count) return false;

            foreach (var att in entity.Attributes)
            {
                if (!entityToCompare.Contains(att.Key))
                {
                    return false;
                }

                if (!CompareValues(entity.Attributes[att.Key], entityToCompare.Attributes[att.Key]))
                {
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// Wheter the attribute value is null, an empty string or white spaces. Also returns <c>true</c>
        /// if the attribute cannot not be found.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <param name="attributeLogicalName">Name of the attribute the value is based on.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsNullOrWhiteSpace(this Entity entity, string attributeLogicalName)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrEmpty(attributeLogicalName)) throw new ArgumentNullException(nameof(attributeLogicalName));

            if (!entity.Contains(attributeLogicalName)) return true;
            if (entity[attributeLogicalName] is null) return true;
            if (entity[attributeLogicalName] is string && string.IsNullOrWhiteSpace(entity.GetAttributeValue<string>(attributeLogicalName))) return true;

            return false;
        }

        /// <summary>
        /// Wheter the attribute value is null, an empty string or white spaces. Also returns <c>true</c>
        /// if the attribute cannot not be found.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <param name="attribute">Name of the attribute the value is based on.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsNullOrWhiteSpace<TEntity, TReturn>(this TEntity entity, Expression<Func<TEntity, TReturn>> attribute) where TEntity : Entity
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _ = attribute ?? throw new ArgumentNullException(nameof(attribute));

            var logicalName = LogicalNameFromProperty(entity, attribute);

            if (string.IsNullOrEmpty(logicalName)) return true;
            return IsNullOrWhiteSpace(entity, logicalName);
        }

        /// <summary>
        /// Returns the attribute value of primitive types (including strings) or the underlying value when attribute is a complex type, 
        /// for example: int for option sets, decimal for money, int[] for option set collection and Guid[] for entity and
        /// entity reference collection.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <param name="attributeLogicalName">Name of the attribute the value is based on.</param>
        /// <returns>The attribute value.</returns>
        public static object GetUnderlyingValue(this Entity entity, string attributeLogicalName)
        {

            if (!entity.Contains(attributeLogicalName) || entity[attributeLogicalName] is null)
            {
                return default;
            }

            var value = entity[attributeLogicalName] is AliasedValue aliasedValue
                ? aliasedValue.Value
                : entity[attributeLogicalName];

            return ToUnderlyingValue(value);
        }

        /// <summary>
        /// Merge atributes from another entity. Only overrides existing attributes with the same name if the attribute value in the target entity is null, an empty string or white space.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <param name="fromEntity">The entity to copy attributes from.</param>
        /// <returns>True is at least one attributed was added or modified.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool MergeAttributes(this Entity entity, Entity fromEntity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var hasChanges = false;

            if (fromEntity == null)
            {
                return hasChanges;
            }

            foreach (var att in fromEntity.Attributes)
            {

                if (entity.Contains(att.Key))
                {
                    var curValue = entity[att.Key];

                    if (curValue is null || (curValue is string && string.IsNullOrWhiteSpace((string)curValue)))
                    {
                        hasChanges = true;
                        entity.Attributes[att.Key] = att.Value;
                    }

                }
                else
                {
                    hasChanges = true;
                    entity.Attributes[att.Key] = att.Value;
                }

            }

            return hasChanges;
        }

        private static EntityCollection CloneEntityCollection(EntityCollection entityCollection)
        {
            var newCollection = new EntityCollection();

            CopyReadWriteProperties(entityCollection, newCollection, propertyExceptions: nameof(newCollection.Entities));

            foreach (var entity in entityCollection.Entities.ToList())
            {
                newCollection.Entities.Add(entity.Clone());
            }

            return newCollection;
        }

        private static EntityReferenceCollection CloneEntityReferenceCollection(EntityReferenceCollection entityRefCollection)
        {
            var newCollection = new EntityReferenceCollection();

            foreach (var item in entityRefCollection.ToList())
            {
                var eref = new EntityReference(item.LogicalName, item.Id);
                CopyReadWriteProperties(item, eref);
                newCollection.Add(item);
            }

            return newCollection;
        }

        private static OptionSetValueCollection CloneOptionSetCollection(OptionSetValueCollection optionSetCollection)
        {
            var newCollection = new OptionSetValueCollection();

            foreach (var item in optionSetCollection.ToList())
            {
                var value = new OptionSetValue(item.Value);
                CopyReadWriteProperties(item, value);
                newCollection.Add(item);
            }

            return newCollection;
        }

        private static bool CompareValues(object a, object b)
        {

            if (a == b) return true;
            if (a == null || b == null) return false;

            a = ToUnderlyingValue(a);
            b = ToUnderlyingValue(b);

            var atype = a.GetType();

            if (atype == b.GetType())
            {
                if (typeof(IEnumerable).IsAssignableFrom(atype))
                {
                    if (!CompareCollectionOfPrimitiveValuesValues(((IEnumerable)a).Cast<object>(), ((IEnumerable)b).Cast<object>()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!a.Equals(b)) return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private static bool CompareCollectionOfPrimitiveValuesValues(IEnumerable<object> acol, IEnumerable<object> bcol)
        {

            // return true if values are the same.
            return acol.Union(bcol).Except(acol.Intersect(bcol)).Count() == 0;
        }

        private static void CopyReadWriteProperties<T>(T source, T target, params string[] propertyExceptions)
        {
            var properties = typeof(T)
                .GetProperties()
                .Where(p => p.GetIndexParameters().Count() == 0)
                .ToList();

            foreach (var prop in properties)
            {
                if (propertyExceptions != null && propertyExceptions.Contains(prop.Name)) continue;
                if (!prop.CanRead || !prop.CanWrite) continue;

                var indexParams = prop.GetIndexParameters();
                prop.SetValue(target, prop.GetValue(source));
            }
        }

        private static string LogicalNameFromProperty<TEntity, TReturn>(TEntity entity, Expression<Func<TEntity, TReturn>> attribute) where TEntity : Entity
        {

            LambdaExpression lambda = attribute;
            MemberExpression memberExpression = lambda.Body is UnaryExpression expression
                ? (MemberExpression)expression.Operand
                : (MemberExpression)lambda.Body;

            PropertyInfo prop = (PropertyInfo)memberExpression.Member;
            AttributeLogicalNameAttribute att = prop.GetCustomAttribute<AttributeLogicalNameAttribute>();

            return att?.LogicalName;
        }

        public static object ToUnderlyingValue(object value)
        {

            if (value == null) return null;

            switch (value)
            {
                case BooleanManagedProperty boolPro:
                    return boolPro.Value;

                case EntityCollection entityCol:
                    return entityCol.Entities.Select(e => e.Id).ToArray();

                case EntityReference entityRef:
                    return entityRef.Id;

                case EntityReferenceCollection entityRefCol:
                    return entityRefCol.Select(e => e.Id).ToArray();

                case Money money:
                    return money.Value;

                case OptionSetValueCollection optionSetCol:
                    return optionSetCol.Select(o => o.Value).ToArray();

                case OptionSetValue optionSet:
                    return optionSet.Value;

            }

            return value;
        }
    }
}
