namespace Application;
public interface ICompareWriter
{
    Task CompareAndWrite();
}

public class CompareWriter : ICompareWriter
{
    IDataComparerService _comparerService;
    XmlWriteOptions _xmlWriteOptions;
    public CompareWriter (YandexMapCenterInvestUrlDto yandexMapCred, ActualDataXmlPathDto actualDataXmplPath, XmlWriteOptions xmlWriteOptions)
    {
        _comparerService =  new DataComparerService(yandexMapCred, actualDataXmplPath);
        _xmlWriteOptions = xmlWriteOptions;
    }
    public async Task CompareAndWrite()
    {
        var resultCompare = await _comparerService.CompareAsync();
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(CompanyDataDifferenceDto));
 
    
        using var fs = new FileStream(_xmlWriteOptions.Path, FileMode.OpenOrCreate);
        xmlSerializer.Serialize(fs, resultCompare);
    }
}