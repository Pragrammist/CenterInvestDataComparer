
var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

var yandexMapCred = config.GetSection("YandexMapCenterInvestOrg")?.Get<YandexMapCenterInvestUrlDto>() 
    ?? throw new NullReferenceException("Configuration Exception. Yandex credentials is null");

var actualDataXmplPath = config.GetSection("ActualDataCenterInvestOrg")?.Get<ActualDataXmlPathDto>() 
    ?? throw new NullReferenceException("Configuration Exception. Actual data source not found");

var xmlWriteOptions = config.GetSection("CompareResultOutput")?.Get<XmlWriteOptions>() 
    ?? throw new NullReferenceException("Configuration Exception. Xml no not found");

ICompareWriter compareWriter = new CompareWriter(yandexMapCred, actualDataXmplPath, xmlWriteOptions);
//IDataComparerService comparerService =  new DataComparerService(yandexMapCred, actualDataXmplPath);



await compareWriter.CompareAndWrite();

