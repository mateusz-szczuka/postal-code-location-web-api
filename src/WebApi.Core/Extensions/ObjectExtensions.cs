namespace WebApi.Core.Extensions;

public static class ObjectExtensions
{
    public static bool IsNull<T>(this T obj)
        where T : class => obj is null;

    public static bool IsNotNull<T>(this T obj)
        where T : class => obj is not null;
}