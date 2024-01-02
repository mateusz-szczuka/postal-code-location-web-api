using System.ComponentModel.DataAnnotations;
using WebApi.Core.Extensions;

namespace WebApi.Core.Options;

public sealed record PostalCodeGeneratorOptions : IValidatableObject
{
    public int RandMinValue { get; init; }

    public int RandMaxValue { get; init; }

    public IReadOnlyList<string> PostalCodes { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PostalCodes.IsNullOrEmpty())
        {
            yield return new ValidationResult(
                "PostalCodes cannot be null or empty.",
                new[] { nameof(PostalCodes) });
        }

        if (PostalCodes.Any(code => code.Length != 6))
        {
            yield return new ValidationResult(
                "All postal codes must have a length of 6 characters.",
                new[] { nameof(PostalCodes) });
        }

        if (RandMaxValue > PostalCodes.Count)
        {
            yield return new ValidationResult(
                "RandMaxValue cannot be greater than the count of PostalCodes.",
                new[] { nameof(RandMaxValue) });
        }

        if (RandMinValue < 1)
        {
            yield return new ValidationResult(
                "RandMinValue cannot be less than 1.",
                new[] { nameof(RandMinValue) });
        }

        if (RandMaxValue <= RandMinValue)
        {
            yield return new ValidationResult(
                "RandMaxValue cannot be less than RandMinValue.",
                new[] { nameof(RandMaxValue) });
        }
    }
}