namespace Application;



public interface IMapService
{
    Task<IEnumerable<CompanyDataDto>> GetDataFromYandexApi();
}
public class MapService : IMapService
{
    IHttpClientFactory _httpClientFactory;
    HttpClient _yandexHttpClient;
    public MapService(YandexMapCenterInvestUrlDto yandexMapCredentials)
    {
        _httpClientFactory = new YandexMapHttpClientFactory(yandexMapCredentials);
        _yandexHttpClient = _httpClientFactory.GetHttpClient();

    }
    public async Task<IEnumerable<CompanyDataDto>> GetDataFromYandexApi()
    {
        var response = await _yandexHttpClient.GetAsync(string.Empty);


        var jsonStr = await response.Content.ReadAsStringAsync()
            ?? throw new NullReferenceException("content inner body from yandex api result in sull");
        

        var jobj = JObject.Parse(jsonStr);


        var jArrayFeatures = jobj["features"]
            ?? throw new NullReferenceException("jobject doesn't contains 'propeties'");


        var result = jArrayFeatures.Select(jt => {
            var coordinates = jt["geometry"]?["coordinates"]?.Values<float>().Reverse().ToArray()
                ?? throw new NullReferenceException("coordinates not found");

            var jtMetadata = jt["properties"]?["CompanyMetaData"]
                ?? throw new NullReferenceException("metadata is null");

            var address = jtMetadata["address"]?.ToString()
                ?? throw new NullReferenceException("address is null");

            var phones = jtMetadata["Phones"]?.Select(phoneJt => phoneJt?["formatted"]?.ToString()
                ?? throw new NullReferenceException("formated phone doesn't found")
            ).NormolizePhones().ToArray() ?? throw new NullReferenceException("phones doesn't found");

            var workingTime = jtMetadata["Hours"]?["text"]?.ToString() 
                ?? throw new NullReferenceException("workingTime not found");

            return new CompanyDataDto(new Coordinates(
                coordinates[0], coordinates[1]), 
                address, workingTime, phones);
        });


        return result;
    }
}