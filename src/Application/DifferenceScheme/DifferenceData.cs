public record CompanyDataDifference(List<DifferenceElement> OldElementDifference, List<NewCompanyData> NewCompanies);

public record DifferenceElement(PhoneDataDifference? PhonesDifference = null, AddressDifference? UrlDifference = null, WorkingtimeDifference? WorkingtimeDifference = null);

public record PhoneDataDifference(
    List<string> NewTelephones,
    List<string> OldTelephones
);

public record AddressDifference(
    string OldAdress,
    string NewAdress
);

public record WorkingtimeDifference(
    string OldWorkingtime,
    string NewWorkingtime
);

public record NewCompanyData(string address, string workingTime, List<string> telephones);

