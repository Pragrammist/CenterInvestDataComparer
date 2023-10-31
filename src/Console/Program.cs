using System.Xml.Serialization;
XmlSerializer serializer = new XmlSerializer(typeof(Companies));
var currentDir = Directory.GetCurrentDirectory();
Companies companies;
using (StringReader reader = new StringReader(await File.ReadAllTextAsync(Path.Combine(currentDir, "CenterInvestList.xml"))))
{
    var deserializedData = serializer.Deserialize(reader) ?? throw new NullReferenceException("Configuration Exception. Current data is null");;;
    companies = (Companies)deserializedData;
}

var builder = new ConfigurationBuilder()
                .SetBasePath(currentDir)
                .AddJsonFile("appsettings.json", optional: false)
                .AddXmlFile("CenterInvestList.xml", optional: false);

IConfiguration config = builder.Build();

var yandexMapCred = config.GetSection("Urls")?.GetSection("YandexMapCenterInvestOrg")?.Get<YandexMapCenterInvestUrl>() 
    ?? throw new NullReferenceException("Configuration Exception. Yandex credentials is null");

DataComparerService d =  new DataComparerService(yandexMapCred);



var result = await d.CompareAsync(companies);
Console.WriteLine(yandexMapCred.UrlWithApiKey);