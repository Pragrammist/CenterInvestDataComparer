// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

using System.Text.Json.Serialization;

public record Availability(
    [property: JsonPropertyName("Intervals")] IReadOnlyList<Interval> Intervals,
    [property: JsonPropertyName("Monday")] bool? Monday,
    [property: JsonPropertyName("Tuesday")] bool? Tuesday,
    [property: JsonPropertyName("Wednesday")] bool? Wednesday,
    [property: JsonPropertyName("Thursday")] bool? Thursday,
    [property: JsonPropertyName("Friday")] bool? Friday,
    [property: JsonPropertyName("Saturday")] bool? Saturday
);

public record Category(
    [property: JsonPropertyName("class")] string Class,
    [property: JsonPropertyName("name")] string Name
);

public record CompanyMetaData(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("address")] string Address,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("Phones")] IReadOnlyList<Phone> Phones,
    [property: JsonPropertyName("Categories")] IReadOnlyList<Category> Categories,
    [property: JsonPropertyName("Hours")] Hours Hours
);

public record Feature(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("geometry")] Geometry Geometry,
    [property: JsonPropertyName("properties")] Properties Properties
);

public record Geometry(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("coordinates")] IReadOnlyList<double?> Coordinates
);

public record Hours(
    [property: JsonPropertyName("text")] string Text,
    [property: JsonPropertyName("Availabilities")] IReadOnlyList<Availability> Availabilities
);

public record Interval(
    [property: JsonPropertyName("from")] string From,
    [property: JsonPropertyName("to")] string To
);

public record Phone(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("formatted")] string Formatted
);

public record Properties(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("boundedBy")] IReadOnlyList<List<double?>> BoundedBy,
    [property: JsonPropertyName("uri")] string Uri,
    [property: JsonPropertyName("CompanyMetaData")] CompanyMetaData CompanyMetaData
);


public record YandexMapApiResultRoot(
    [property: JsonPropertyName("features")] IReadOnlyList<Feature> Features
);

public record SearchRequest(
    [property: JsonPropertyName("request")] string Request,
    [property: JsonPropertyName("skip")] int? Skip,
    [property: JsonPropertyName("results")] int? Results,
    [property: JsonPropertyName("boundedBy")] IReadOnlyList<List<double?>> BoundedBy
);

public record SearchResponse(
    [property: JsonPropertyName("found")] int? Found,
    [property: JsonPropertyName("display")] string Display
);