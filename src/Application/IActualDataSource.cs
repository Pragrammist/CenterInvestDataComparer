namespace Application;

public interface IActualDataSource
{
    Task<IEnumerable<CompanyDataDto>> GetActualData();
}


public class ActualDataSource : IActualDataSource
{
    ActualDataXmlPathDto _actualDataXmlPath;
    public ActualDataSource(ActualDataXmlPathDto actualDataXmlPath)
    {
        _actualDataXmlPath = actualDataXmlPath;
    }
    public async Task<IEnumerable<CompanyDataDto>> GetActualData()
    {
        var streamRead = File.OpenRead(_actualDataXmlPath.Path);
        
        XDocument xdoc = await XDocument.LoadAsync(streamRead, LoadOptions.None, default);
        var companies = xdoc.Element("companies")?.Elements() ?? throw new NullReferenceException("companies not found");
        var result = companies.Select(compEl => {
            var coordinatesEl = compEl.Element("coordinates") ?? throw new NullReferenceException("coordinates is null");
            
            var latStr = coordinatesEl.Element("lat")?.Value ?? throw new NullReferenceException("lat is null");
            var lonStr = coordinatesEl.Element("lon")?.Value ?? throw new NullReferenceException("lon is null");
            
            var coordinates = new Coordinates(float.Parse(latStr), float.Parse(lonStr));
            var address = compEl.Element("address")?.Value
                ?? throw new NullReferenceException("address is null");

            var workingTime = compEl.Element("working-time")?.Value
                ?? throw new NullReferenceException("working-time is null");
            
            var phones = compEl.Elements("phone")?.Select(
                phEl => phEl.Element("number")?.Value
                    ?? throw new NullReferenceException("number is null")
                ).NormolizePhones().ToArray() ?? throw new NullReferenceException("phones is null");
            
            return new CompanyDataDto(
                coordinates,
                address,
                workingTime,
                phones
            );
        });

        return result;
    }
}