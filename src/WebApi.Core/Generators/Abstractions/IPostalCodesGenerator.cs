namespace WebApi.Core.Generators.Abstractions;

public interface IPostalCodesGenerator
{
    IReadOnlyList<string> GetAllPostalCodes();

    IEnumerable<string> GetRandomPostalCodes();
}