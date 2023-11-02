using Application;


namespace Unit;

public class ComparerTest
{
    
    public ComparerTest()
    {
        
        
    }
    IDataComparerService GetComparerService(IEnumerable<CompanyDataDto> resultFromApi, IEnumerable<CompanyDataDto> actualDataResult)
    {
        var mapService = GetMapService(resultFromApi);

        var actualDataSource = GetActualDataSource(actualDataResult );

        return new DataComparerService(mapService, actualDataSource);
    }
    IMapService GetMapService(IEnumerable<CompanyDataDto> result)
    {
        var mock = new Mock<IMapService>();

        mock.Setup(s => s.GetDataFromYandexApi().Result).Returns(result);

        return mock.Object;
    }
    IActualDataSource GetActualDataSource(IEnumerable<CompanyDataDto> result)
    {
        var mock = new Mock<IActualDataSource>();
        
        mock.Setup(s => s.GetActualData().Result).Returns(result);

        return mock.Object;
    }

    [Theory]
    [InlineData("ул. Пушкина Дом Колотушкина", "ул. Пушкина Дом Колотушкина", false)]
    [InlineData("ул. Пушкина Дом Колотушкина", "ул. Счасться Дом 2", true)]
    public async Task TestAddressDifference(string addressFromApi, string actualAddress, bool hasResult)
    {
        var comparerService = GetComparerService(
            resultFromApi: new CompanyDataDto[]
            {
                new CompanyDataDto(new Coordinates(123, 123), addressFromApi, "пн-вс", new string[]{"88003332605"})
            },
            actualDataResult: new CompanyDataDto[]
            {
                new CompanyDataDto(new Coordinates(123, 123), actualAddress, "пн-вс", new string[]{"88003332605"})
            }
        );

        var compareResult = await comparerService.CompareAsync();


        compareResult.OldElementDifference.Should().Contain(AdressDifferenceExpression(actualAddress, addressFromApi, hasResult));
    }


    
    [Theory]
    [InlineData("пн-вс", "пн-вс", false)]
    [InlineData("пн-вс", "пн-пт", true)]
    public async Task TestWorkingTimeDifference(string workingTimeFromApi, string actualworkingTime, bool hasResult)
    {
        var comparerService = GetComparerService(
            resultFromApi: new CompanyDataDto[]
            {
                new CompanyDataDto(new Coordinates(123, 123), "ул. Пушкина Дом Колотушкина", workingTimeFromApi, new string[]{"88003332605"})
            },
            actualDataResult: new CompanyDataDto[]
            {
                new CompanyDataDto(new Coordinates(123, 123), "ул. Пушкина Дом Колотушкина", actualworkingTime, new string[]{"88003332605"})
            }
        );

        var compareResult = await comparerService.CompareAsync();


        compareResult.OldElementDifference.Should().Contain(WorkingtimeDifferenceExpression(workingTimeFromApi, actualworkingTime, hasResult));
    }

    [Theory]
    [InlineData(new string[]{"88003332605"}, new string[]{"88003332605"}, false)]
    [InlineData(new string[]{"88005553535"}, new string[]{"88003332605"}, true)]
    public async Task TestTelephonesDifference(string[] telephonesFromApi, string[] actualTelephones, bool hasResult)
    {
        var comparerService = GetComparerService(
            resultFromApi: new CompanyDataDto[]
            {
                new CompanyDataDto(new Coordinates(123, 123), "ул. Пушкина Дом Колотушкина", "пн-вс", telephonesFromApi)
            },
            actualDataResult: new CompanyDataDto[]
            {
                new CompanyDataDto(new Coordinates(123, 123), "ул. Пушкина Дом Колотушкина", "пн-вс", actualTelephones)
            }
        );

        var compareResult = await comparerService.CompareAsync();


        compareResult.OldElementDifference.Should().Contain(TelephonesDifferenceExpression(telephonesFromApi, actualTelephones, hasResult));
    }
    public static IEnumerable<object[]> DataWithCompanies() {
        var oldData = new CompanyDataDto(new Coordinates(123, 122), "ул. Счастья", "пн-вс", new string[]{"88003332605"});
        var newData = new CompanyDataDto(new Coordinates(123, 123), "ул. Пушкина Дом Колотушкина", "пн-вс", new string[]{"322323235"});
        return new []
        {
            new object[] { oldData, oldData, false },
            new object[] { oldData, newData, true },
            
        };
    }
        
    
    [Theory]
    [MemberData(nameof(DataWithCompanies))]
    public async Task NewCompanyDetected(CompanyDataDto companyFromApi, CompanyDataDto actualData, bool hasResult)
    {
        var comparerService = GetComparerService(
            resultFromApi: new CompanyDataDto[]
            {
                companyFromApi
            },
            actualDataResult: new CompanyDataDto[]
            {
                actualData
            }
        );

        var compareResult = await comparerService.CompareAsync();

        if(hasResult)
            compareResult.NewCompanies.Should().Contain(el => el == actualData);
        else
            compareResult.NewCompanies.Count.Should().Be(0);

    }

    [Theory]
    [MemberData(nameof(DataWithCompanies))]
    public async Task NotActualCompaniesCompanyDetected(CompanyDataDto companyFromApi, CompanyDataDto actualData, bool hasResult)
    {
        var comparerService = GetComparerService(
            resultFromApi: new CompanyDataDto[]
            {
                companyFromApi
            },
            actualDataResult: new CompanyDataDto[]
            {
                actualData
            }
        );

        var compareResult = await comparerService.CompareAsync();

        if(hasResult)
            compareResult.NotActualCompanies.Should().Contain(el => el == companyFromApi);
        else
            compareResult.NotActualCompanies.Count.Should().Be(0);

    }
    

    Expression<Func<DifferenceElementDto, bool>> TelephonesDifferenceExpression(string[] telephonesFromApi, string[] actualTelephones, bool isTrue = true)
    {
        //to switch negetive or positive testing
        if(isTrue)
            return el => 
                el.PhonesDifference != null && 
                el.PhonesDifference.NewTelephones.Any(t => actualTelephones.Contains(t)) &&
                el.PhonesDifference.OldTelephones.Any(t => telephonesFromApi.Contains(t));
        

        return el => el.PhonesDifference == null;
    }

    

    Expression<Func<DifferenceElementDto, bool>> WorkingtimeDifferenceExpression(string workingTimeFromApi, string actualworkingTime, bool isTrue = true)
    {
        //to switch negetive or positive testing
        if(isTrue)
            return el =>  el.WorkingtimeDifference != null && 
                el.WorkingtimeDifference.NewWorkingtime == actualworkingTime &&
                el.WorkingtimeDifference.OldWorkingtime == workingTimeFromApi;
        

        return el => el.WorkingtimeDifference == null;
    }


    Expression<Func<DifferenceElementDto, bool>> AdressDifferenceExpression(string actualAddress, string addressFromApi, bool isTrue = true)
    {
        //to switch negetive or positive testing
        if(isTrue)
            return el =>  el.AddressDifference != null && 
                el.AddressDifference.NewAdress == actualAddress &&
                el.AddressDifference.OldAdress == addressFromApi;
        

        return el => el.AddressDifference == null;
    }
}