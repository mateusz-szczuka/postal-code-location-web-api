using Microsoft.Extensions.Options;
using WebApi.Core.Generators.Abstractions;
using WebApi.Core.Options;

namespace WebApi.Core.Generators;

public sealed class PostalCodesGenerator : IPostalCodesGenerator
{
    private readonly PostalCodeGeneratorOptions _postalCodeGeneratorOptions;

    public PostalCodesGenerator(IOptions<PostalCodeGeneratorOptions> postalCodeGeneratorOptions)
    {
        _postalCodeGeneratorOptions = postalCodeGeneratorOptions?.Value ?? throw new ArgumentNullException(nameof(postalCodeGeneratorOptions));
    }

    public IReadOnlyList<string> GetAllPostalCodes()
    {
        return _postalCodeGeneratorOptions.PostalCodes;
    }

    public IEnumerable<string> GetRandomPostalCodes()
    {
        var quantity = Random.Shared.Next(_postalCodeGeneratorOptions.RandMinValue, _postalCodeGeneratorOptions.RandMaxValue);
        return _postalCodeGeneratorOptions.PostalCodes.OrderBy(_ => Random.Shared.Next()).Take(quantity);
    }
}