namespace Application;

public interface IDataComparerService 
{
    Task<CompanyDataDifferenceDto> CompareAsync();
}


public class DataComparerService : IDataComparerService
{
    IMapService _mapService;
    IActualDataSource _actualDataService;
    public DataComparerService(YandexMapCenterInvestUrlDto yandexMapCredentials, ActualDataXmlPathDto actualDataXmlPath)
    {
        _actualDataService = new ActualDataSource(actualDataXmlPath);
        _mapService = new MapService(yandexMapCredentials);
    }

    public DataComparerService(IMapService mapService, IActualDataSource actualDataService)
    {
        _mapService = mapService;
        _actualDataService = actualDataService;
    }
    public async Task<CompanyDataDifferenceDto> CompareAsync()
    {
        var dataFromApi = await _mapService.GetDataFromYandexApi();
        var actualData = await _actualDataService.GetActualData();

        var differenceElements = new List<DifferenceElementDto>();
        var newCompanies = new List<CompanyDataDto>();

        foreach(var actualCompData in actualData)
        {
            
            var oldCompData = dataFromApi.FirstOrDefault(
                oldCompData => 
                    actualCompData.Coordinates.Lat == oldCompData.Coordinates.Lat &&
                        actualCompData.Coordinates.Lon== oldCompData.Coordinates.Lon
            );

                    
            if(oldCompData is not null)
                differenceElements.Add(CompareOneElement(oldCompData, actualCompData));
            else
                newCompanies.Add(actualCompData);
            

        }

        var notActualCompanies = new List<CompanyDataDto>();
        foreach(var oldCompData in dataFromApi)
        {
            var actualCompData = actualData.FirstOrDefault(
                actualCompData => 
                    actualCompData.Coordinates.Lat == oldCompData.Coordinates.Lat &&
                        actualCompData.Coordinates.Lon== oldCompData.Coordinates.Lon
            );

            if(actualCompData is null)
                notActualCompanies.Add(oldCompData);
            

        }

        return new CompanyDataDifferenceDto(differenceElements, newCompanies, notActualCompanies);
    }

    

    DifferenceElementDto CompareOneElement(CompanyDataDto oldData, CompanyDataDto newData)
    {
        var oldWorkingtime = oldData.WorkingTime;
        var newWorkingtime = newData.WorkingTime;

        var oldAddress = oldData.Address;
        var newAddress = newData.Address;
        
        var oldNumbers = oldData.Phones.Except(newData.Phones).ToList();
        var newNumbers = newData.Phones.Except(oldData.Phones).ToList();
        

        return new DifferenceElementDto(
            //if any of this array has some element then some changes
            //if no no changes then set null
            PhonesDifference: oldNumbers.Count > 0 || newNumbers.Count > 0 ? new PhoneDataDifferenceDto(newNumbers, oldNumbers) : null,
            
            //if has difference set old and new value
            //if no no changes then set null
            AddressDifference: oldAddress != newAddress ?  new AddressDifferenceDto(oldAddress, newAddress) : null,
            
             //if has difference set old and new value
            //if no no changes then set null
            WorkingtimeDifference: oldWorkingtime != newWorkingtime ? new WorkingtimeDifferenceDto(oldWorkingtime, newWorkingtime) : null
        );

        

    }
}

