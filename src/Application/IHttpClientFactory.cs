namespace Application;

public interface IHttpClientFactory
{
    HttpClient GetHttpClient();
}

public class YandexMapHttpClientFactory : IHttpClientFactory
{
    public YandexMapHttpClientFactory(YandexMapCenterInvestUrlDto yandexMapCredentials)
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

    public void SetYandexCredentails(YandexMapCenterInvestUrlDto yandexMapCredentials)
    {
        var httpClient = GetHttpClient();

        httpClient.BaseAddress = new Uri(yandexMapCredentials.UrlWithApiKey);
    }
}