public record CompanyDataDto(
    Coordinates Coordinates, 
    string Address, 
    string WorkingTime, 
    string[] Phones
)
{
    private CompanyDataDto() : this(new Coordinates(0,0), string.Empty, string.Empty, Array.Empty<string>())
    {

    }
};

public record Coordinates(float Lat, float Lon)
{
    private Coordinates () : this (0, 0){}
};