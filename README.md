![image](http://www.ark-energy.eu/wp-content/uploads/ark-dark.png)
# Artesian.SDK

This Library provides read access to the Artesian API

## Getting Started
### Installation
This library is provided in NuGet.

Support for .NET Framework 4.6.1, .NET Standard 2.0.

In the Package Manager Console -
```
Install-Package Artesian.SDK
```

or download directly from NuGet.
* [NuGet](https://www.nuget.org/packages/Artesian.SDK/)

## How to use
The Artesian.SDK instance can be configured using either Client credentials or API-Key authentication 
```csharp
//API-Key
 ArtesianServiceConfig _cfg = new ArtesianServiceConfig(
		new Uri("https://fake-artesian-env/"),
		"5418B0DB - 7AB9 - 4875 - 81BA - 6EE609E073B6"
		);

//Client credentials
 ArtesianServiceConfig _cfg = new ArtesianServiceConfig(
		new Uri("https://fake-artesian-env/"),
		"audience",
		"domain",
		"client_id",
		"client_secret"
		);
```

## QueryService

Using the ArtesianServiceConfig we create an instance of the QueryService which is used to create Actual, Versioned and Market Assessment time series queries

### Actual Time Series
```csharp
var queryservice = new QueryService(_cfg);
var actualTimeSeries = await qs.CreateActual()
                .ForMarketData(new int[] { 100000001, 100000002, 100000003 })
                .InGranularity(Granularity.Day)
                .InAbsoluteDateRange(new LocalDate(2018,08,01),new LocalDate(2018,08,10))
                .ExecuteAsync();
```
To construct an Actual Time Series the following must be provided.
<table>
  <tr><th>Actual Query</th><th>Description</th></tr>
  <tr><td>Market Data ID</td><td>Provide a market data id or set of market data id's to query</td></tr>
  <tr><td>Time Granularity</td><td>Specify the granularity type</td></tr>
  <tr><td>Time Extraction Window</td><td>An extraction time window for data to be queried</td></tr>
</table>

[Go to Time Extraction window section](#artesian-sdk-extraction-windows)

### Market Assessment Time Series
```csharp
var queryservice = new QueryService(_cfg);
var marketAssesmentSeries = await qs.CreateMarketAssessment()
                       .ForMarketData(new int[] { 100000001 })
                       .ForProducts(new string[] { "M+1", "GY+1" })
                       .InRelativeInterval(RelativeInterval.RollingMonth)
                       .ExecuteAsync();
```
To construct a Market Assessment Time Series the following must be provided.
<table>
  <tr><th>Actual Query</th><th>Description</th></tr>
  <tr><td>Market Data ID</td><td>Provide a market data id or set of market data id's to query</td></tr>
  <tr><td>Product</td><td>Provide a product or set of products</td></tr>
  <tr><td>Time Extraction Window</td><td>An extraction time window for data to be queried </td></tr>
</table>

[Go to Time Extraction window section](#artesian-sdk-extraction-windows)

### Versioned Time Series
```csharp
 var versionedSeries = await qs.CreateVersioned()
		.ForMarketData(new int[] { 100000001 })
		.InGranularity(Granularity.Day)
		.ForLastOfMonths(Period.FromMonths(-4))
		.InRelativeInterval(RelativeInterval.RollingMonth)
		.ExecuteAsync();
```
To construct a Versioned Time Series the following must be provided.
<table>
  <tr><th>Versioned Query</th><th>Description</th></tr>
  <tr><td>Market Data ID</td><td>Provide a market data id or set of market data id's to query</td></tr>
  <tr><td>Time Granularity</td><td>Specify the granularity type</td></tr>
  <tr><td>Versioned Time Extraction Window</td><td>Versioned extraction time window</td></tr>
  <tr><td>Time Extraction Window</td><td>An extraction time window for data to be queried</td></tr>
</table>

[Go to Time Extraction window section](#artesian-sdk-extraction-windows)

### Artesian SDK Extraction Windows
Extraction window types  for queries.

Date Range
```csharp
 .InAbsoluteDateRange(new LocalDate(2018,08,01),new LocalDate(2018,08,10)
```
Relative Interval
```csharp
 .InRelativeInterval(RelativeInterval.RollingMonth)
```
Period
```csharp
 .InRelativePeriod(Period.FromDays(5))
```
Period Range
```csharp
 .InRelativePeriodRange(Period.FromWeeks(2), Period.FromDays(20))
```

## MetadataService

The MetadataService service is used to access the Artesian api market data metadata and related entitys.

```csharp
 var metadataservice = new MetadataService(_cfg);
 var metadataquery = await metadataservice.ReadMarketDataRegistryAsync(
		new MarketDataIdentifier("TestProvider", "TestCurveName"));
```
## MarketData Service

The MarketData Service is used to retrieve and edit MarketData references.

Retrieving Market Data by an Identifier or Id and verify if it is registerd
```csharp
 var md = mds.GetMarketDataReference(new MarketDataIdentifier(marketDataEntity.ProviderName, marketDataEntity.MarketDataName)); // Id or identifier

    var isRegistered = await md.IsRegistered(); // if exists return true, otherwise false
    
    if (!isRegistered)
        await md.Register(marketDataEntity);
```

Changing a value to verify an update
```csharp
    md.Metadata.AggregationRule = AggregationRule.Undefined;
    md.Metadata.Transform = SystemTimeTransforms.GASDAY66;

    await md.Update();

    await md.Load();
```

Using Write mode for Actual
```csharp
    if (md.Metadata.Type == MarketDataType.ActualTimeSerie)
    {
        var wmd = md.EditActual();

        wmd.AddData(new LocalDate(2018, 10, 03), 10);
        wmd.AddData(new LocalDate(2018, 10, 04), 15);

        await wmd.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()));
    }
```

 Versioned
 ```csharp
    else if (md.Metadata.Type == MarketDataType.VersionedTimeSerie)
    {
        var wmd = md.EditVersioned(new LocalDateTime(2018, 10, 18, 00, 00));

        wmd.AddData(new LocalDate(2018, 10, 03), 10);
        wmd.AddData(new LocalDate(2018, 10, 04), 15);

        await wmd.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()));
    }
```

Market Assessment
```csharp
    else if (md.Metadata.Type == MarketDataType.MarketAssessment)
    {
        var wmd = md.EditMarketAssessment();

        var mktAssValue = new MarketAssessmentValue()
        {
            High = 47,
            Close = 20,
            Low = 18,
            Open = 33
        };

        wmd.AddData(new LocalDate(2018, 11, 28), "Dec-18", mktAssValue);

        await wmd.Save(Instant.FromDateTimeUtc(DateTime.Now.ToUniversalTime()));
    }
```

## Links
* [Nuget](https://www.nuget.org/packages/Artesian.SDK/)
* [Github](https://github.com/ARKlab/Artesian.SDK)
* [Ark Energy](http://www.ark-energy.eu/)

## Acknowledgments
* [Flurl](https://flurl.io/docs/fluent-url/)
