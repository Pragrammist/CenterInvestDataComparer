public record CompanyDataDifferenceDto(List<DifferenceElementDto> OldElementDifference, List<CompanyDataDto> NewCompanies, List<CompanyDataDto> NotActualCompanies)
{
    public CompanyDataDifferenceDto() : this(new List<DifferenceElementDto>(), new List<CompanyDataDto>(), new List<CompanyDataDto>())
    {
    }
};

public record DifferenceElementDto(PhoneDataDifferenceDto? PhonesDifference = null, 
    AddressDifferenceDto? AddressDifference = null, 
        WorkingtimeDifferenceDto? WorkingtimeDifference = null)
{
    public DifferenceElementDto() : this(null, null, null){}
}

public record PhoneDataDifferenceDto(
    List<string> NewTelephones,
    List<string> OldTelephones
)
{
    public PhoneDataDifferenceDto() : this(new List<string>(), new List<string>()){}
}

public record AddressDifferenceDto(
    string OldAdress,
    string NewAdress
)
{
    private AddressDifferenceDto() :this(string.Empty, string.Empty) {}
};

public record WorkingtimeDifferenceDto(
    string OldWorkingtime,
    string NewWorkingtime
)
{
    private WorkingtimeDifferenceDto() :this(string.Empty, string.Empty) {}
};


