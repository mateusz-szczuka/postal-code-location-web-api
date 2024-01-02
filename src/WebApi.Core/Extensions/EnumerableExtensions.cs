namespace WebApi.Core.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        => InternalIsNullOrEmpty(collection);

    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> collection)
        => !InternalIsNullOrEmpty(collection);

    private static bool InternalIsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        if (collection is null)
        {
            return true;
        }

        if (!collection.Any())
        {
            return true;
        }

        return false;
    }
}