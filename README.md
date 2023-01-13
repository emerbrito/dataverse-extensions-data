# EmBrito.Dataverse.Extensions.Data

[![Nuget](https://img.shields.io/nuget/v/EmBrito.Dataverse.Extensions.Data)](https://www.nuget.org/packages/EmBrito.Dataverse.Extensions.Data)
![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/emerbrito/dataverse-extensions-data/dotnet-build.yml)

Data utilities for Dateverse client applications and plugin development.

# Entity Mappings

Performs data transformation between an entity (table) and a business object (such as a DTO). A common use case is when you need to map an entity to a dto, serialize and send it to an external API. In this document:

- [Basic Usage](#basic-usage)
- [Using Formatted Values](#using-formatted-values)
- [Applying String Format](#applying-string-format)
- [Mapping Multiple Choice Columns](#mapping-multiple-choice-columns)
- [Mapping Entity Collection Columns](#mapping-entity-collection-columns)
- [Mapping Entity Reference Columns](#mapping-entity-reference-columns)
- [Mapping Entity Reference Collection Columns](#mappinge-entity-reference-collection-columns)
- [Custom Converters](#custom-converters)
- [Data Mapping Attributes](#data-mapping-attributes)
- [Dataverse Column to .net Types](#dataverse-column-to-net-types)

## Basic Usage

To perform data mapping and transformation your DTO must extend the `EntityMapper` class.
Mapping is done by decorating each mapped property with a corresponding attribute.

A simple example:

``` csharp
public class AccountDto : EntityMapper<AccountDto>
{
    [FromStringColumn("name")]
    public string Name { get; set; }

    [FromOptionSetColumn("industrycode", formattedValue: true)]
    public string IndustryCode { get; set; }

    [FromCurrencyColumn("openrevenue")]
    public decimal? Revenue { get; set; }

    [FromOptionSetColumn("statecode")]
    public int State { get; set; }

    public AccountDto(Entity entity) : base(entity)
    {
    }
}
```

In this example an instance of the DTO is created from an account entity and then serialized to JSON:

``` csharp
Entity account = crmServiceClient.Retrieve("account", accountId, new ColumnSet(true));
AccountDto dto = new AccountDto(account);

// serializing the mapped object
string jsonString = JsonSerializer.Serialize(dto);
```
The JSON should look like this:

``` json
{
    "Name": "Contoso",
    "IndustryCode": "Business Services",
    "Revenue": 3000000,
    "AccountState": 1
}
```

See the following sections for additional details on data transformation.

## Using Columns Formatted Value

Some dataverse columns such as numbers, dates and currency have built in support for formatted values. The formatted value of an option set (choice) is its label.

The use the formatted value, map to a string property and set the `formattedValue` parameter to `true`.

``` csharp
// returns the option set label
[FromOptionSetColumn("statecode", formattedValue: true)]
public string Status { get; set; }

// returns the formatted date
[FromDateTimeColumn("modifiedon", formattedValue: true)]
public string DateModified { get; set; }

// returns the formatted currency value
[FromCurrencyColumn("annualrevenue", formattedValue: true)]
public string Revenue { get; set; }
```

## Applying String Format

Some attributes will support a string format parameter.
When provided, this format will be applied to the output string.
More information on supported formats can be found here: [How to format numbers, dates, enums, and other types in .NET][1]

``` csharp
// sample output: 2023-10-25
[FromDateTimeColumn("modifiedon",format: "yyyy-MM-dd")]
public string DateModified { get; set; }

// sample output: 
[FromCurrencyColumn("annualrevenue", format: "C3")]
public string Revenue { get; set; }
```

## Mapping Multiple Choice Columns

The output of a multiple choices columns is controlled by the attribute paramaters and the property type.

``` csharp

// The examples using a List<T> (int or string) could also be replaced with arrays of the equivalent type.

// Returns a list of numbers with the value from each selection.
[FromOptionSetCollectionColumn("new_subscription")]
public List<int> Subscriptions { get; set; }

// Returns a list of strings with the numeric value from each selection.
[FromOptionSetCollectionColumn("new_subscription")]
public List<string> Subscriptions { get; set; }

// Returns a list of strings with the label from each selection.
[FromOptionSetCollectionColumn("new_subscription", formattedValue: true)]
public List<string> Subscriptions { get; set; }

// Returns a comma separated string with the numeric value of all selected items.
[FromOptionSetCollectionColumn("new_subscription")]
public string Subscriptions { get; set; }

// Returns a comma separated string with the label from all selected items.
[FromOptionSetCollectionColumn("new_subscription", formattedValue: true)]
public string Subscriptions { get; set; }

```

## Mapping Entity Collection Columns

Some columns such as Activity Party returns an entity collection. The output will always be the ID property of each entity which could be represented by the following types:

- Guid[]
- List<Guid>
- string[] (Guid as string)
- List<string> (Guid as string)

```csharp
[FromEntityCollectionColumn("to")]
public List<Guid> EmailTo { get; set; }
```

## Mapping Entity Reference Columns
Entity reference columns can output their id as Guid or string and their formatted value (the primary name column of the corresponding entity).

``` csharp
// Outputs entity reference id
[FromEntityReferenceColumn("parentaccountid")]
public Guid? ParentAccount { get; set; }

// Outputs entity reference id as string
[FromEntityReferenceColumn("parentaccountid")]
public string ParentAccount { get; set; }

// Outputs entity reference formatted value (the primary name column)
[FromEntityReferenceColumn("parentaccountid", formattedValue: true)]
public string ParentAccount { get; set; }
```

## Mapping Entity Reference Collection Columns

Entity reference collection columns can output their id as Guid or string and their formatted value (the primary name column of the corresponding entity).

- Guid[]
- List<Guid>
- string[] (Guid as strings)
- List<string> (Guid as strings)
- List<string> (primary name when formattedValue is true)
- string (comma separate list of primary name when formattedValue is true)

## Custom Converters

You can apply custom data transformation to handle specific business scenarions. Form example, you may need to return different labels for an option set than what you get from formatted values.

``` csharp
public class AccountDto : EntityMapper<AccountDto>
{
    public AccountDto(Entity entity) : base(entity)
    {
    }
    
    [FromOptionSetColumn("accounttype")]
    public string AccountType { get; set; }

    // Ovveride the Configure method to specify custom converter for specific fields
    protected override void Configure(EntityMapperOptions options)
    {
        options.AddConverter("accounttype", AccountTypeConverter);
    }

    object AccountTypeConverter(MappedProperty mappingInfo, Entity entity)
    {
        var choice = entity.GetAttributeValue<OptionSetValue>("accounttype");        
        return choice.Value == 2 ? "Premium" : "Standard"        
    }

}
```

## Data Mapping Attributes

The attribute name is self explanatory and should match the column type you are mapping from.
The property type will determine the desired output, or the type you are mapping to.

Below is a list of all available attributes the the supported output type.

> Columns mapped to a string property may output  their primitive values, the column formatted value from the dataverse or a formatted string when a format parameter is provided. Support for each option is based on the attribute type. A `formattedValue` paramter will be available in the attribute when supported.

### FromBooleanColumn

- Boolean
- Integer
- String (will display true, false or the option set label)

### FromCurrencyColumn

- Decimal
- String

### FromDateTimeColumn

- DateTime
- String

### FromDecimalColumn

Conversion to other numeric types or boolean will be performed based on the value.
If the value cannot be converted an exception will be thrown.

- Bool
- Decimal
- Double
- Float
- Int
- String

### FromDoubleColumn

Conversion to other numeric types or boolean will be performed based on the value.
If the value cannot be converted an exception will be thrown.

- Bool
- Decimal
- Double
- Float
- Int
- String

### FromEntityCollectionColumn

- Array<Guid>
- Array<string> (Guid will be converted to string)
- List<Guid>
- List<string> (Guid will be converted to string)

### FromEntityReferenceCollectionColumn

- Array<Guid>
- Array<string> (Guid will be converted to string)
- List<Guid>
- List<string> (Guid will be converted to string)

### FromEntityReferenceColumn

- Guid
- String

### FromImageColumn

- byte[] (the original byte array returned by as the column value).
- String (a base64 string representing the byte[])

### FromIntegerColumn
Conversion to other numeric types or boolean will be performed based on the value.
If the value cannot be converted an exception will be thrown.

- Bool
- Decimal
- Double
- Float
- Int
- String

### FromOptionSetCollectionColumn

- Array<int>
- Array<string> (With numeric values or option set labels)
- List<int>
- List<string> (With numeric values or option set labels)

### FromOptionSetColumn

Conversion to other numeric types besides decimal, or boolean will be performed based on the value. If the value cannot be converted an exception will be thrown.

- Bool
- Decimal
- Double
- Float
- Int
- String (numeric value or option set label)

### FromStringColumn
Conversion to numeric types, boolean or DateTime will be performed based on the value.
If the value cannot be converted an exception will be thrown.

- Bool
- Decimal
- DateTime
- Double
- Float
- Int
- String

### FromUniqueIdentifierColumn

- Guid
- String (Guid converted to a string)
## Dataverse Column Type to .net Types

The following table is meant for reference only.
It indicates how the dataverse columns are mapped to .net types by the PowerPlatform SDK.
It also indicates the column types that have native support for formatted values.

| Column Type | .net type | Formatted Value Support<sup>1</sup> |
|-------------|-----------|-------------------------|
| Activity Party | EntityCollection<sup>2</sup> | |
| Auto Number | string | |
| Currency | Money | Yes |
| Customer | EntityReference | Yes |
| Date and Time | DateTime | Yes |
| Decimal | decimal | Yes |
| Duration | Int32 | Yes |
| File | Guid | |
| Float | Double | Yes |
| Image | byte[] | |
| Language Code | Int32 | Yes |
| Lookup | EntityReference | Yes |
| Multiple Lines of Text | string | |
| MultiSelect Option Set (Choice) | OptionSetValueCollection | Yes |
| Option Set (Choice) | OptionSetValue | Yes |
| Owner | EntityReference | Yes |
| Single Line of Text | string | |
| Status | OptionSetValue | Yes |
| Status Reason | OptionSetValue | Yes |
| Time Zone | Int32 | Yes |
| Ticker Symbol | string | |
| Two Options | bool | Yes |
| Unique Identifier | Guid | |
| Whole Number | Int32 | Yes |

<sup>1</sup> Whether a string representing a formatted value is also returned by query expressions.
<sup>2</sup> An Activity Party column will return an entity collection of `actvityparty`. The `partyid` column of each returned entity will contain an `EntityReference` to the actual record.

[1]: https://learn.microsoft.com/en-us/dotnet/standard/base-types/formatting-types
