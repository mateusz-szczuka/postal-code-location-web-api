using System.Reflection;

namespace WebApi.Core;

public static class AssemblyCoreReference
{
    public static readonly Assembly Assembly = typeof(AssemblyCoreReference).Assembly;
}