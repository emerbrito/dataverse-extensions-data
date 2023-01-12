using EmBrito.Dataverse.Extensions.Data.Tests.Mappings.Models;
using Microsoft.Xrm.Sdk;
using System.Text;

namespace EmBrito.Dataverse.Extensions.Data.Tests.Mappings
{
    public class EntityMappingTests
    {

        #region Fields Declaration

        Guid defaultId = Guid.Parse(Constants.DefaultId);
        Guid id1 = Guid.Parse(Constants.Id1);
        Guid id2 = Guid.Parse(Constants.Id2);
        Guid id3 = Guid.Parse(Constants.Id3);
        OptionSetValue defaultChoice = new OptionSetValue(1);
        DateTime defaultDate = new DateTime(2001, 01, 01);
        Decimal defaultDecimal = 100.15M;
        Money defaultMoney = new Money(200.32M);
        Int32 defaultInt = 15;
        double defaultDouble = 3.45;
        byte[] defaultImage = Encoding.UTF8.GetBytes("image");
        EntityReference defaultEntityRef = new(Constants.EntityLogicalName2, Guid.Parse(Constants.Id2))
        {
            Name = Constants.AccountName2
        };
        EntityCollection activityParyCol = new(new List<Entity>
        {
            new Entity("activityparty", Guid.Parse(Constants.Id1)),
            new Entity("activityparty", Guid.Parse(Constants.Id2)),
            new Entity("activityparty", Guid.Parse(Constants.Id3))
        });
        EntityReferenceCollection entityRefCollection = new EntityReferenceCollection(new List<EntityReference>
        {
            new EntityReference("account", Guid.Parse(Constants.Id1)) { Name = Constants.AccountName1 },
            new EntityReference("account", Guid.Parse(Constants.Id2)) { Name = Constants.AccountName2 },
            new EntityReference("account", Guid.Parse(Constants.Id3)) { Name = Constants.AccountName3 }
        });
        OptionSetValueCollection defaultMultiChoices = new(new List<OptionSetValue>
        {
            new OptionSetValue(1),
            new OptionSetValue(2),
            new OptionSetValue(3)
        });

        #endregion

        #region General Testing

        [Fact]
        public void Static_Mapped_Attributes_Count()
        {
            Assert.Equal(16, DefaultEntityDto.MappedAttributeNames().Count());
        }

        [Fact]
        public void DefaultMappings()
        {
            var entity = GetDefaultEntity();
            var dto = new DefaultEntityDto(entity);

            Assert.Equal(Constants.AccountName1, dto.AccountName);
            Assert.Equal(new Guid[] { id1, id2, id3 }, dto.ActivityParty);
            Assert.Equal("1001", dto.AutoNumber);
            Assert.Equal(defaultChoice.Value, dto.Choice);
            Assert.Equal(defaultMoney.Value, dto.Currency);
            Assert.Equal(defaultDate, dto.DateTime);
            Assert.Equal(defaultDecimal, dto.Decimal);
            Assert.Equal(defaultInt, dto.Duration);
            Assert.Equal(defaultId, dto.Id);
            Assert.Equal(defaultEntityRef.Id, dto.EnitityReference);
            Assert.Equal(defaultDouble, dto.Float);
            Assert.Equal(defaultImage, dto.Image);
            Assert.Equal(new int[] { 1, 2, 3 }, dto.MultiChoices);
            Assert.Equal(true, dto.TwoOptions);
            Assert.Equal(new Guid[] { id1, id2, id3 }, dto.EntityRefColl);
        }

        [Fact]
        public void All_Null_Values()
        {
            var entity = new Entity();
            var dto = new DefaultEntityDto(entity);

            Assert.Equal(null, dto.AccountName);
            Assert.Equal(null, dto.ActivityParty);
            Assert.Equal(null, dto.AutoNumber);
            Assert.Equal(null, dto.Choice);
            Assert.Equal(null, dto.Currency);
            Assert.Equal(null, dto.DateTime);
            Assert.Equal(null, dto.Decimal);
            Assert.Equal(null, dto.Duration);
            Assert.Equal(null, dto.Id);
            Assert.Equal(null, dto.EnitityReference);
            Assert.Equal(null, dto.Float);
            Assert.Equal(null, dto.Image); ;
            Assert.Equal(null, dto.MultiChoices);
            Assert.Equal(null, dto.TwoOptions);
            Assert.Equal(null, dto.WholeNumber);
            Assert.Equal(null, dto.EntityRefColl);
        }

        [Fact]
        public void All_Default_Value()
        {
            var entity = new Entity();
            var dto = new DefaultEntityNoNullablesDto(entity);

            Assert.Equal(null, dto.AccountName);
            Assert.Equal(null, dto.ActivityParty);
            Assert.Equal(null, dto.AutoNumber);
            Assert.Equal(0, dto.Choice);
            Assert.Equal(0M, dto.Currency);
            Assert.Equal(default(DateTime), dto.DateTime);
            Assert.Equal(0M, dto.Decimal);
            Assert.Equal(0, dto.Duration);
            Assert.Equal(Guid.Empty, dto.Id);
            Assert.Equal(Guid.Empty, dto.EnitityReference);
            Assert.Equal(0D, dto.Float);
            Assert.Equal(null, dto.Image); ;
            Assert.Equal(null, dto.MultiChoices);
            Assert.Equal(false, dto.TwoOptions);
            Assert.Equal(0, dto.WholeNumber);
            Assert.Equal(null, dto.EntityRefColl);
        }

        [Fact]
        public void String_Format_Attribute()
        {
            var entity = GetDefaultEntity();
            var dto = new DefaultEntityStringFormatDto(entity);

            Assert.Equal(defaultMoney.Value.ToString("C3"), dto.Currency);
            Assert.Equal(defaultDate.ToString("MM-dd-yy"), dto.DateTime);
        }

        [Fact]
        public void Custom_Data_Converter()
        {
            var entity = GetDefaultEntity();
            var dto = new CustomConverterDto(entity);

            Assert.Equal("Custom Value 1", dto.Gender);
        }

        #endregion

        #region Dates

        [Fact]
        public void Date_To_And_From_Text()
        {
            var entity = GetDefaultEntity();
            entity.Attributes.Add("dateastext", "2001-01-01");

            var dto = new DateToTextDto(entity);            

            Assert.Equal(defaultDate.ToString(), dto.DateTime);
            Assert.Equal(defaultDate, dto.DateFromText);
        }

        #endregion

        #region Entity Reference Collection

        [Fact]
        public void Map_EntityRefernceCollection_Value_NotFound()
        {
            var entity = new Entity();
            var dto = new EntityRefCollToGuidListDto(entity);

            Assert.Null(dto.EntityRefColl);
        }

        [Fact]
        public void Map_EntityRefernceCollection_To_Guid_Array()
        {
            var entity = GetDefaultEntity();
            var mapper = new EntityRefCollToGuidArrayDto(entity);

            var expectedChoices = new Guid[] { id1, id2, id3 };

            Assert.Equal(expectedChoices, mapper.EntityRefColl);
        }

        [Fact]
        public void Map_EntityRefernceCollection_To_Guid_List()
        {
            var entity = GetDefaultEntity();
            var mapper = new EntityRefCollToGuidListDto(entity);

            var expectedChoices = new List<Guid> { id1, id2, id3 };

            Assert.Equal(expectedChoices, mapper.EntityRefColl);
        }

        [Fact]
        public void Map_EntityRefernceCollection_To_String_Array()
        {
            var entity = GetDefaultEntity();
            var mapper = new EntityRefCollToStringArrayDto(entity);

            var expectedChoices = new string[] { id1.ToString(), id2.ToString(), id3.ToString() };

            Assert.Equal(expectedChoices, mapper.EntityRefColl);
        }

        [Fact]
        public void Map_EntityRefernceCollection_To_String_List()
        {
            var entity = GetDefaultEntity();
            var mapper = new EntityRefCollToStringListDto(entity);

            var expectedChoices = new List<string> { id1.ToString(), id2.ToString(), id3.ToString() };

            Assert.Equal(expectedChoices, mapper.EntityRefColl);
        }

        [Fact]
        public void Map_EntityRefernceCollection_To_FormattedValues_List()
        {
            var entity = GetDefaultEntity();
            var mapper = new EntityRefCollToFormattedValueListDto(entity);

            var expected = new List<string> 
            { 
                Constants.AccountName1, 
                Constants.AccountName2, 
                Constants.AccountName3 
            };

            Assert.Equal(expected, mapper.EntityRefColl);
        }

        [Fact]
        public void Map_EntityRefernceCollection_To_FormattedValues_Array()
        {
            var entity = GetDefaultEntity();
            var mapper = new EntityRefCollToFormattedValueArrayDto(entity);

            var expected = new string[]
            {
                Constants.AccountName1,
                Constants.AccountName2,
                Constants.AccountName3
            };

            Assert.Equal(expected, mapper.EntityRefColl);
        }

        [Fact]
        public void Map_EntityRefernceCollection_To_FormattedValues_String()
        {
            var entity = GetDefaultEntity();
            var mapper = new EntityRefCollToFormattedValueStringDto(entity);

            var expected = $"{Constants.AccountName1}, {Constants.AccountName2}, {Constants.AccountName3}";

            Assert.Equal(expected, mapper.EntityRefColl);
        }

        #endregion

        #region Byte Array (Images)

        [Fact]
        public void ByteArray_To_Base64_String()
        {
            var content = "somecontent";
            var bytes = Encoding.UTF8.GetBytes(content);
            var entity = GetDefaultEntity();
            entity.Attributes[Constants.ImageColumn] = bytes;

            var dto = new ImageToBase64StringDto(entity);
            var expected = System.Convert.ToBase64String(bytes);

            Assert.Equal(expected, dto.Image);
        }

        #endregion

        #region Entity Collection

        [Fact]
        public void Map_EntityCollection_Value_NotFound()
        {
            var entity = GetDefaultEntity();
            var mapper = new EntityCollectionToListUnknowAttDto(entity);

            Assert.Null(mapper.ActivityParty);
        }

        [Fact]
        public void Map_EntityCollection_To_Guid_Array()
        {
            var entity = GetDefaultEntity();
            var mapper = new EntityCollectionToGuidArrayDto(entity);

            var expectedChoices = new Guid[] { id1, id2, id3 };

            Assert.Equal(expectedChoices, mapper.ActivityParty);
        }

        [Fact]
        public void Map_EntityCollection_To_Guid_List()
        {
            var entity = GetDefaultEntity();
            var mapper = new EntityCollectionToGuidListDto(entity);

            var expectedChoices = new List<Guid> { id1, id2, id3 };

            Assert.Equal(expectedChoices, mapper.ActivityParty);
        }

        [Fact]
        public void Map_EntityCollection_To_String_Array()
        {
            var entity = GetDefaultEntity();
            var mapper = new EntityCollectionToStringArrayDto(entity);

            var expectedChoices = new string[] { id1.ToString(), id2.ToString(), id3.ToString() };

            Assert.Equal(expectedChoices, mapper.ActivityParty);
        }

        [Fact]
        public void Map_EntityCollection_To_String_List()
        {
            var entity = GetDefaultEntity();
            var mapper = new EntityCollectionToStringListDto(entity);

            var expectedChoices = new List<string> { id1.ToString(), id2.ToString(), id3.ToString() };

            Assert.Equal(expectedChoices, mapper.ActivityParty);
        }

        #endregion

        #region Multi Select Option Set

        [Fact]
        public void Map_MultiSelectChoice_Formatted_Value_NotFound()
        {
            var entity = GetDefaultEntity();
            var mapper = new OptionSetCollectionToListUnknownAttDto(entity);

            Assert.Null(mapper.MultiChoices);
        }

        [Fact]
        public void Map_MultiSelectChoice_Formatted_Value_To_String_Array()
        {
            var entity = GetDefaultEntity();
            var mapper = new OptionSetCollectionToFormattedStringArrayDto(entity);

            var expectedChoices = new string[] { "Email", "SMS", "Regular Mail" };

            Assert.Equal(expectedChoices, mapper.MultiChoices);
        }

        [Fact]
        public void Map_MultiSelectChoice_Formatted_Value_To_String_List()
        {
            var entity = GetDefaultEntity();
            var mapper = new OptionSetCollectionToFormattedStringArrayDto(entity);

            var expectedChoices = new List<string> { "Email", "SMS", "Regular Mail" };

            Assert.Equal(expectedChoices, mapper.MultiChoices);
        }

        [Fact]
        public void Map_MultiSelectChoice_Formatted_Value_To_String()
        {
            var entity = GetDefaultEntity();
            var mapper = new OptionSetCollectionToFormattedValueDto(entity);

            Assert.Equal("Email, SMS, Regular Mail", mapper.MultiChoices);
        }

        [Fact]
        public void Map_MultiSelectChoice_Value_To_String_Array()
        {
            var entity = GetDefaultEntity();
            var mapper = new OptionSetCollectionToStringArrayDto(entity);

            var expectedChoices = new string[] { "1", "2", "3" };

            Assert.Equal(expectedChoices, mapper.MultiChoices);
        }

        [Fact]
        public void Map_MultiSelectChoice_Value_To_String_List()
        {
            var entity = GetDefaultEntity();
            var mapper = new OptionSetCollectionToStringListDto(entity);

            var expectedChoices = new List<string> { "1", "2", "3" };

            Assert.Equal(expectedChoices, mapper.MultiChoices);
        }

        [Fact]
        public void Map_MultiSelectChoice_Value_To_Int_Array()
        {
            var entity = GetDefaultEntity();
            var mapper = new OptionSetCollectionToIntArrayDto(entity);

            var expectedChoices = new int[] { 1, 2, 3 };

            Assert.Equal(expectedChoices, mapper.MultiChoices);
        }

        [Fact]
        public void Map_MultiSelectChoice_Value_To_Int_List()
        {
            var entity = GetDefaultEntity();
            var mapper = new OptionSetCollectionToIntListDto(entity);

            var expectedChoices = new List<int> { 1, 2, 3 };

            Assert.Equal(expectedChoices, mapper.MultiChoices);
        }

        #endregion

        #region Helpers

        Entity GetDefaultEntity()
        {
            var entity = new Entity(Constants.EntityLogicalName1, Guid.Parse(Constants.DefaultId));

            entity.Attributes.Add(Constants.AccountNameColumn, new AliasedValue("account", "name", Constants.AccountName1));
            entity.Attributes.Add(Constants.ActivityPartyColumn, activityParyCol);
            entity.Attributes.Add(Constants.AutoNumberColumn, "1001");
            entity.Attributes.Add(Constants.ChoiceColumn, defaultChoice);
            entity.Attributes.Add(Constants.CurrencyColumn, defaultMoney);
            entity.Attributes.Add(Constants.DateTimeColumn, defaultDate);
            entity.Attributes.Add(Constants.DecimalColumn, defaultDecimal);
            entity.Attributes.Add(Constants.DurationColumn, defaultInt);
            entity.Attributes.Add(Constants.EntityIdColumn1, defaultId);
            entity.Attributes.Add(Constants.EntityReferenceColumn, defaultEntityRef);
            entity.Attributes.Add(Constants.FloatColumn, defaultDouble);
            entity.Attributes.Add(Constants.ImageColumn, defaultImage);
            entity.Attributes.Add(Constants.MultiChoicesColumn, defaultMultiChoices);
            entity.Attributes.Add(Constants.TwoOptionsColumn, true);
            entity.Attributes.Add(Constants.WholeNumberColumn, defaultInt);
            entity.Attributes.Add(Constants.EntityReferenceCollectionColumn, entityRefCollection);

            entity.FormattedValues.Add(Constants.CurrencyColumn, defaultMoney.Value.ToString("C"));
            entity.FormattedValues.Add(Constants.EntityReferenceColumn, defaultEntityRef.Name);
            entity.FormattedValues.Add(Constants.ChoiceColumn, "Active");
            entity.FormattedValues.Add(Constants.MultiChoicesColumn, "Email, SMS, Regular Mail");
            entity.FormattedValues.Add(Constants.TwoOptionsColumn, "Yes");

            return entity;
        }

        #endregion

    }
}