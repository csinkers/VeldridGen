using System;

namespace VeldridGen;

public sealed class TypeResolutionException(string typeName) : Exception($"Could not resolve type \"{typeName}\"")
{
    public string TypeName { get; } = typeName;
}
