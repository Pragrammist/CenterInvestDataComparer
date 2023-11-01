namespace Application;

public static class StringHelpers
{
    public static string NormolizePhone(this string phone) =>
        phone.Replace("+7", "8").Replace(" ", string.Empty);

    public static IEnumerable<string> NormolizePhones(this IEnumerable<string> phones) =>
        phones.Select(s => s.NormolizePhone());

    
}