using System.Net.Http.Json;

namespace Application;


public interface IHttpClientFactory
{
    HttpClient GetHttpClient();
}

public class YandexMapHttpClientFactory : IHttpClientFactory
{
    public YandexMapHttpClientFactory(YandexMapCenterInvestUrl yandexMapCredentials)
    {
        SetYandexCredentails(yandexMapCredentials);
    }
    static HttpClient _httpClient = null!;

    public HttpClient GetHttpClient() 
    {
        if(_httpClient is null)
            _httpClient = new HttpClient();
        return _httpClient;
        
    }

    public void SetYandexCredentails(YandexMapCenterInvestUrl yandexMapCredentials)
    {
        var httpClient = GetHttpClient();

        httpClient.BaseAddress = new Uri(yandexMapCredentials.UrlWithApiKey);
    }
}


public interface IMapService
{
    Task<IEnumerable<Feature>> GetDataFromYandexApi();
}



public interface IDataComparerService 
{
    Task<CompanyDataDifference> CompareAsync(Companies companies);
}

public class DataComparerService : IDataComparerService
{
    IMapService _mapService;
    public DataComparerService(YandexMapCenterInvestUrl yandexMapCredentials)
    {
        _mapService = new MapService(yandexMapCredentials);
    }
    public async Task<CompanyDataDifference> CompareAsync(Companies companies)
    {
        var dataFromApi = await _mapService.GetDataFromYandexApi();

        var differenceElements = new List<DifferenceElement>();
        int currentDataIterator = 0;
        foreach(var feature in dataFromApi)
        {
            
            if(currentDataIterator < companies.CompanyList.Length)
            {
                var newData = companies.CompanyList[currentDataIterator];
                var oldData = feature;
                var difference =  CompareOneElement(oldData, newData);
                differenceElements.Add(difference);
            }

            currentDataIterator++;
        }
        List<NewCompanyData> newCompanyData = new List<NewCompanyData>();

        currentDataIterator++;
        var oldDataArrayLength = currentDataIterator;
        if(oldDataArrayLength < companies.CompanyList.Length)
        {   

            var newData = companies.CompanyList[currentDataIterator..].Select(comp => new NewCompanyData(
                address: comp.Address.Text,
                workingTime: comp.Workingtime.Text,
                telephones: comp.PhoneList.Select(s => s.Number).ToList()
            ));
            newCompanyData.AddRange(newData);
        }


        return new CompanyDataDifference(differenceElements, newCompanyData);
    }

    DifferenceElement CompareOneElement(Feature feature, Company company)
    {
        var oldWorkingtime = feature.Properties.CompanyMetaData.Hours.Text;
        var newWorkingtime = company.Workingtime.Text;

        var oldAddress = feature.Properties.CompanyMetaData.Address;
        var newAddress = company.Address.Text;

        var oldNumbers = company.PhoneList.IntersectBy(feature.Properties.CompanyMetaData.Phones.Select(s => s.Formatted), s => s.Number).Select(s => s.Number).ToList();
        var newNumbers = feature.Properties.CompanyMetaData.Phones.IntersectBy(company.PhoneList.Select(s => s.Number), t => t.Formatted).Select(s => s.Formatted).ToList();

        return new DifferenceElement(
            //if any of this array has some element then some changes
            //if no no changes then set null
            PhonesDifference: oldNumbers.Count > 0 || newNumbers.Count > 0 ? new PhoneDataDifference(newNumbers, oldNumbers) : null,
            
            //if has difference set old and new value
            //if no no changes then set null
            UrlDifference: oldAddress != newAddress ?  new AddressDifference(oldAddress, newAddress) : null,
            
             //if has difference set old and new value
            //if no no changes then set null
            WorkingtimeDifference: oldWorkingtime != newWorkingtime ? new WorkingtimeDifference(oldWorkingtime, newWorkingtime) : null
        );

        

    }
}


public class MapService : IMapService
{
    IHttpClientFactory _httpClientFactory;
    HttpClient _yandexHttpClient;
    public MapService(YandexMapCenterInvestUrl yandexMapCredentials)
    {
        _httpClientFactory = new YandexMapHttpClientFactory(yandexMapCredentials);
        _yandexHttpClient = _httpClientFactory.GetHttpClient();

    }
    public async Task<IEnumerable<Feature>> GetDataFromYandexApi()
    {
        var response = await _yandexHttpClient.GetAsync("");

        var resutl = await response.Content.ReadFromJsonAsync<YandexMapApiResultRoot>() 
            ?? throw new NullReferenceException("content inner body from yandex api result in sull");

        return resutl.Features;
    }
}