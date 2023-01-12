using EmBrito.Dataverse.Extensions.Data.Mappings.Converters;
using EmBrito.Dataverse.Extensions.Data.Tests.Mappings.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings
{
    public class AttributeConverterTests
    {

        EntityCollection entityCollection = new EntityCollection
        {
            Entities =
            {
                new Entity("account", Guid.Parse(Constants.Id1)) { Attributes = { new KeyValuePair<string, object>("name", Constants.AccountName1) }  },
                new Entity("account", Guid.Parse(Constants.Id2)) { Attributes = { new KeyValuePair<string, object>("name", Constants.AccountName2) }  },
                new Entity("account", Guid.Parse(Constants.Id3)) { Attributes = { new KeyValuePair<string, object>("name", Constants.AccountName3) }  }
            }
        };

        EntityReferenceCollection entityReferenceCollection = new EntityReferenceCollection(new List<EntityReference>
        {
            new EntityReference("account", Guid.Parse(Constants.Id1)) { Name = Constants.AccountName1 },
            new EntityReference("account", Guid.Parse(Constants.Id2)) { Name = Constants.AccountName2 },
            new EntityReference("account", Guid.Parse(Constants.Id3)) { Name = Constants.AccountName3 }
        });

        OptionSetValueCollection optionSetCollection = new OptionSetValueCollection
        { 
            new OptionSetValue(1),
            new OptionSetValue(2),
            new OptionSetValue(3)
        };


        #region General Validation

        [Fact]
        public void Prevent_Wront_Input_Type()
        {
            var converter = new UniqueIdentifierConverter();
            Assert.Throws<InvalidOperationException>(() => converter.Convert(123, typeof(string)));
        }

        [Fact]
        public void Prevent_Wront_Output_Type()
        {
            var converter = new UniqueIdentifierConverter();
            Assert.Throws<InvalidOperationException>(() => converter.Convert(Guid.Empty, typeof(int)));
        }

        [Fact]
        public void Bypass_Conversion_If_Same_Type()
        {
            var converter = new UniqueIdentifierConverter();
            Assert.Equal(Guid.Empty, converter.Convert(Guid.Empty, typeof(Guid)));
        }

        [Fact]
        public void Nullable_Input_NonNullable_Output()
        {
            var converter = new UniqueIdentifierConverter();
            Guid? id = Guid.Empty;
            Assert.Equal(Guid.Empty, converter.Convert(id, typeof(Guid)));
        }

        [Fact]
        public void NonNullable_Input_Nullable_Output()
        {
            var converter = new UniqueIdentifierConverter();
            Assert.Equal(Guid.Empty, converter.Convert(Guid.Empty, typeof(Guid?)));
        }

        [Fact]
        public void Nullable_Value_To_Nullable_Output()
        {
            var converter = new UniqueIdentifierConverter();
            Assert.Null(converter.Convert(null!, typeof(Guid?)));
        }

        [Fact]
        public void Nullable_Value_To_NonNullable_Output()
        {
            var converter = new UniqueIdentifierConverter();
            Assert.Equal(Guid.Empty, converter.Convert(null!, typeof(Guid)));
        }


        #endregion

        #region Byte[]

        [Fact]
        public void ByteArrayConverter_From_Null_To_Null()
        {
            var converter = new ByteArrayConverter();
            Assert.Null(converter.Convert(null, typeof(byte[])));
        }

        [Fact]
        public void ByteArrayConverter_To_ByteArray()
        {
            var value = "content";
            var bytes = Encoding.UTF8.GetBytes(value);
            var converter = new ByteArrayConverter();
            Assert.Equal(bytes, converter.Convert(bytes, typeof(byte[])));
        }

        [Fact]
        public void ByteArrayConverter_To_String()
        {
            var value = "content";
            var bytes = Encoding.UTF8.GetBytes(value);
            var base64 = System.Convert.ToBase64String(bytes);
            var converter = new ByteArrayConverter();
            Assert.Equal(base64, converter.Convert(bytes, typeof(string)));
        }

        #endregion

        #region Entity Collection

        [Fact]
        public void EntityCollectionConverter_To_EntityCollection()
        {
            var converter = new EntityCollectionConverter();
            Assert.Equal(entityCollection, converter.Convert(entityCollection, typeof(EntityCollection)));
        }

        [Fact]
        public void EntityCollectionConverter_To_Guid_List()
        {
            var converter = new EntityCollectionConverter();
            var expected = new List<Guid> 
            {
                Guid.Parse(Constants.Id1),
                Guid.Parse(Constants.Id2),
                Guid.Parse(Constants.Id3)
            };
            Assert.Equal(expected, converter.Convert(entityCollection, typeof(List<Guid>)));
        }

        [Fact]
        public void EntityCollectionConverter_To_Guid_Array()
        {
            var converter = new EntityCollectionConverter();
            var expected = new Guid[]
            {
                Guid.Parse(Constants.Id1),
                Guid.Parse(Constants.Id2),
                Guid.Parse(Constants.Id3)
            };
            Assert.Equal(expected, converter.Convert(entityCollection, typeof(Guid[])));
        }

        [Fact]
        public void EntityCollectionConverter_To_String_List()
        {
            var converter = new EntityCollectionConverter();
            var expected = new List<string>
            {
                Guid.Parse(Constants.Id1).ToString(),
                Guid.Parse(Constants.Id2).ToString(),
                Guid.Parse(Constants.Id3).ToString()
            };
            Assert.Equal(expected, converter.Convert(entityCollection, typeof(List<string>)));
        }

        [Fact]
        public void EntityCollectionConverter_To_String_Array()
        {
            var converter = new EntityCollectionConverter();
            var expected = new string[]
            {
                Guid.Parse(Constants.Id1).ToString(),
                Guid.Parse(Constants.Id2).ToString(),
                Guid.Parse(Constants.Id3).ToString()
            };
            Assert.Equal(expected, converter.Convert(entityCollection, typeof(string[])));
        }

        [Fact]
        public void EntityCollectionConverter_To_String_List_Null()
        {
            var converter = new EntityCollectionConverter();
            Assert.Null(converter.Convert(null!, typeof(List<string>)));
        }

        [Fact]
        public void EntityCollectionConverter_To_String_Array_Null()
        {
            var converter = new EntityCollectionConverter();
            Assert.Null(converter.Convert(null!, typeof(string[])));
        }

        #endregion

        #region Entity Reference Collection

        [Fact]
        public void EntityReferenceCollectionConverter_To_EntityReferenceCollection()
        {
            var converter = new EntityReferenceCollectionConverter();
            Assert.Equal(entityReferenceCollection, converter.Convert(entityReferenceCollection, typeof(EntityReferenceCollection)));
        }

        [Fact]
        public void EntityReferenceCollectionConverter_To_Guid_List()
        {
            var converter = new EntityReferenceCollectionConverter();
            var expected = new List<Guid>
            {
                Guid.Parse(Constants.Id1),
                Guid.Parse(Constants.Id2),
                Guid.Parse(Constants.Id3)
            };
            Assert.Equal(expected, converter.Convert(entityReferenceCollection, typeof(List<Guid>)));
        }

        [Fact]
        public void EntityReferenceCollectionConverter_To_Guid_Array()
        {
            var converter = new EntityReferenceCollectionConverter();
            var expected = new Guid[]
            {
                Guid.Parse(Constants.Id1),
                Guid.Parse(Constants.Id2),
                Guid.Parse(Constants.Id3)
            };
            Assert.Equal(expected, converter.Convert(entityReferenceCollection, typeof(Guid[])));
        }

        [Fact]
        public void EntityReferenceCollectionConverter_To_String_List()
        {
            var converter = new EntityReferenceCollectionConverter();
            var expected = new List<string>
            {
                Guid.Parse(Constants.Id1).ToString(),
                Guid.Parse(Constants.Id2).ToString(),
                Guid.Parse(Constants.Id3).ToString()
            };
            Assert.Equal(expected, converter.Convert(entityReferenceCollection, typeof(List<string>)));
        }

        [Fact]
        public void EntityReferenceCollectionConverter_To_String_Array()
        {
            var converter = new EntityReferenceCollectionConverter();
            var expected = new string[]
            {
                Guid.Parse(Constants.Id1).ToString(),
                Guid.Parse(Constants.Id2).ToString(),
                Guid.Parse(Constants.Id3).ToString()
            };
            Assert.Equal(expected, converter.Convert(entityReferenceCollection, typeof(string[])));
        }

        [Fact]
        public void EntityReferenceCollectionConverter_To_String_List_Null()
        {
            var converter = new EntityReferenceCollectionConverter();
            Assert.Null(converter.Convert(null!, typeof(List<string>)));
        }

        [Fact]
        public void EntityReferenceCollectionConverter_To_String_Array_Null()
        {
            var converter = new EntityReferenceCollectionConverter();
            Assert.Null(converter.Convert(null!, typeof(string[])));
        }

        #endregion

        #region OptionSet Collection

        [Fact]
        public void OptionSetCollectionConverter_To_OptionSetCollection()
        {
            var converter = new OptionSetCollectionConverter();
            Assert.Equal(optionSetCollection, converter.Convert(optionSetCollection, typeof(OptionSetValueCollection)));
        }

        [Fact]
        public void OptionSetCollectionConverter_To_Int_List()
        {
            var converter = new OptionSetCollectionConverter();
            var expected = new List<int> { 1, 2, 3 };
            Assert.Equal(expected, converter.Convert(optionSetCollection, typeof(List<int>)));
        }

        [Fact]
        public void OptionSetCollectionConverter_To_Int_Array()
        {
            var converter = new OptionSetCollectionConverter();
            var expected = new int[] { 1, 2, 3 };
            Assert.Equal(expected, converter.Convert(optionSetCollection, typeof(int[])));
        }

        [Fact]
        public void OptionSetCollectionConverter_To_String_List()
        {
            var converter = new OptionSetCollectionConverter();
            var expected = new List<string> { "1", "2", "3" };
            Assert.Equal(expected, converter.Convert(optionSetCollection, typeof(List<string>)));
        }

        [Fact]
        public void OptionSetCollectionConverter_To_String_Array()
        {
            var converter = new OptionSetCollectionConverter();
            var expected = new string[] { "1", "2", "3" };
            Assert.Equal(expected, converter.Convert(optionSetCollection, typeof(string[])));
        }


        #endregion

        #region Primitives

        [Theory]
        [InlineData(null, false, typeof(bool))]
        [InlineData(null, null, typeof(bool?))]
        [InlineData(true, 1, typeof(int))]
        [InlineData(true, 1, typeof(int?))]
        [InlineData(true, "True", typeof(string))]
        public void BooleanConveter(bool? value, object? expected, Type destinationType)
        {
            var converter = new BooleanConverter();
            expected = ConvertToType(expected, destinationType);
            Assert.Equal(expected, converter.Convert(value, destinationType));
        }

        [Theory]
        [InlineData(null, 0, typeof(decimal))]
        [InlineData(null, null, typeof(decimal?))]
        [InlineData(1.32, 1.32, typeof(decimal))]
        [InlineData(1.32, "1.32", typeof(string))]
        [InlineData(1.32, 1.32, typeof(double))]
        [InlineData(1.32, 1.32, typeof(float))]
        [InlineData(1.32, 1.32, typeof(int))]
        [InlineData(1, true, typeof(bool))]
        [InlineData(0, false, typeof(bool))]
        public void DecimalConveter(object? value, object? expected, Type destinationType)
        {
            var converter = new DecimalConverter();
            value = ConvertToType(value, typeof(decimal));
            expected = ConvertToType(expected, destinationType);
            Assert.Equal(expected, converter.Convert(value, destinationType));
        }

        [Theory]
        [InlineData(null, 0, typeof(double))]
        [InlineData(null, null, typeof(double?))]
        [InlineData(1.32, 1.32, typeof(double))]
        [InlineData(1.32, "1.32", typeof(string))]
        [InlineData(1.32, 1.32, typeof(decimal))]
        [InlineData(1.32, 1.32, typeof(float))]
        [InlineData(1.32, 1.32, typeof(int))]
        [InlineData(1, true, typeof(bool))]
        [InlineData(0, false, typeof(bool))]
        public void DoubleConveter(object? value, object? expected, Type destinationType)
        {
            var converter = new DoubleConverter();
            value = ConvertToType(value, typeof(double));
            expected = ConvertToType(expected, destinationType);
            Assert.Equal(expected, converter.Convert(value, destinationType));
        }

        [Theory]
        [InlineData(null, 0, typeof(int))]
        [InlineData(null, null, typeof(int?))]
        [InlineData(1, 1, typeof(int))]
        [InlineData(1, "1", typeof(string))]
        [InlineData(1, 1, typeof(double))]
        [InlineData(1, 1, typeof(float))]
        [InlineData(1, 1, typeof(decimal))]
        [InlineData(1, true, typeof(bool))]
        [InlineData(0, false, typeof(bool))]
        public void IntegerConveter(object? value, object? expected, Type destinationType)
        {
            var converter = new IntegerConverter();
            value = ConvertToType(value, typeof(int));
            expected = ConvertToType(expected, destinationType);
            Assert.Equal(expected, converter.Convert(value, destinationType));
        }

        [Theory]
        [InlineData(null, null, typeof(string))]
        [InlineData("value", "value", typeof(string))]
        [InlineData("1", 1, typeof(int))]
        [InlineData("1", 1, typeof(int?))]
        [InlineData("25.32", 25.32, typeof(decimal))]
        [InlineData("25.32", 25.32, typeof(double))]
        [InlineData("true", true, typeof(bool))]
        public void StringConveter(string? value, object? expected, Type destinationType)
        {
            var converter = new StringConveter();
            expected = ConvertToType(expected, destinationType);
            Assert.Equal(expected, converter.Convert(value, destinationType));
        }

        #endregion

        #region Money

        [Fact]
        public void MoneyConverter_To_Money()
        {
            var converter = new MoneyConverter();
            var expected = new Money(1000);
            Assert.Equal(expected, converter.Convert(expected, typeof(Money)));
        }

        [Fact]
        public void MoneyConverter_To_Decimal()
        {
            var converter = new MoneyConverter();
            Assert.Equal(1000M, converter.Convert(new Money(1000), typeof(decimal)));
        }

        [Fact]
        public void MoneyConverter_To_String()
        {
            var converter = new MoneyConverter();
            Assert.Equal("1000", converter.Convert(new Money(1000), typeof(string)));
        }

        #endregion

        #region Option Set

        [Theory]
        [InlineData(null, 0, typeof(int))]
        [InlineData(null, null, typeof(int?))]
        [InlineData(1, 1, typeof(int))]
        [InlineData(1, true, typeof(bool))]
        [InlineData(0, false, typeof(bool))]
        [InlineData(1, "1", typeof(string))]
        [InlineData(1, 1, typeof(decimal))]
        [InlineData(1, 1, typeof(double))]
        [InlineData(1, 1, typeof(float))]
        public void OptionSetConverter(int? value, object? expected, Type destinationType)
        {
            var optionset = value.HasValue ? new OptionSetValue(value.Value) : null;
            var converter = new OptionSetConverter();            
            expected = ConvertToType(expected, destinationType);

            Assert.Equal(expected, converter.Convert(optionset, destinationType));
        }

        #endregion

        #region DateTime

        [Fact]
        public void DateTimeConverter_To_DateTime()
        {
            var converter = new DateTimeConverter();
            var expected = new DateTime(2001, 01, 01);
            Assert.Equal(expected, converter.Convert(expected, typeof(DateTime)));
        }

        [Fact]
        public void DateTimeConverter_To_String()
        {
            var converter = new DateTimeConverter();
            var expected = new DateTime(2001, 01, 01);
            Assert.Equal(expected.ToString(), converter.Convert(expected, typeof(string)));
        }

        #endregion

        #region Entity Reference

        [Fact]
        public void EntityReferenceConverter_To_EntityReference()
        {
            var converter = new EntityReferenceConverter();
            var expected = new EntityReference("", Guid.Parse(Constants.DefaultId));

            Assert.Equal(expected, converter.Convert(expected, typeof(EntityReference)));
        }

        [Fact]
        public void EntityReferenceConverter_To_Guid()
        {
            var converter = new EntityReferenceConverter();
            var expected = Guid.Parse(Constants.DefaultId);

            Assert.Equal(expected, converter.Convert(new EntityReference("", expected), typeof(Guid)));
        }

        [Fact]
        public void EntityReferenceConverter_To_String()
        {
            var converter = new EntityReferenceConverter();
            var expected = Guid.Parse(Constants.DefaultId);

            Assert.Equal(expected.ToString(), converter.Convert(new EntityReference("", expected), typeof(string)));
        }

        #endregion

        #region Unique Identifier

        [Fact]
        public void UniqueIdentifierConverter_To_Guid()
        {
            var converter = new UniqueIdentifierConverter();
            Assert.Equal(Guid.Empty, converter.Convert(Guid.Empty, typeof(Guid?)));
        }

        [Fact]
        public void UniqueIdentifierConverter_To_String()
        {
            var converter = new UniqueIdentifierConverter();
            Assert.Equal(Guid.Empty.ToString(), converter.Convert(Guid.Empty, typeof(string)));
        }


        #endregion

        #region Helpers

        object? ConvertToType(object? value, Type destinationType)
        {
            // Some types cannot be passed as arguments to the InlineData attribute.
            // For example a decimal value as 23.15M won't compile.
            // This will ensure the expected value is actually of the expected type.

            if (value != null)
            {
                Type toType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;
                value = Convert.ChangeType(value, toType);
            }

            return value;
        }

        #endregion

    }
}
