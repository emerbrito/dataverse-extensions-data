# EmBrito.Dataverse.Extensions.Data
Data utilities for Dateverse client applications and plugin development.

[![Nuget](https://img.shields.io/nuget/v/EmBrito.Dataverse.Extensions.Data)](https://www.nuget.org/packages/EmBrito.Dataverse.Extensions.Data)

## Entity Mapper

Use a convetion based approach to data mapping and transformation by annotating your data transfer object.
The mapper will automatically transform the data between between the column and property types while also giving access to Dataverse formatted values and string formatting.

``` csharp
// Sample class with the data annotation,
public class AccountDto : EntityMapper<AccountDto>
{
    [FromStringColumn("name")]
    public string Name { get; set; }

    [FromOptionSetColumn("industrycode", formattedValue: true)]
    public string IndustryCode { get; set; }

    [FromOptionSetColumn("createdon", format: "yyyy-MM-dd")]
    public string DateCreated { get; set; }    

    [FromCurrencyColumn("openrevenue")]
    public decimal? Revenue { get; set; }

    [FromOptionSetColumn("statecode")]
    public int State { get; set; }

    public AccountDto(Entity entity) : base(entity) {}
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
    "DateCreated": "2023-01-12",
    "Revenue": 3000000,
    "AccountState": 1
}
```

Visit the [project website][1] for detailed documentation.

[1]: https://github.com/emerbrito/dataverse-extensions-data