using System;

namespace VeldridGen
{
    public sealed class TypeResolutionException : Exception
    {
        public TypeResolutionException(string typeName) : base($"Could not resolve type \"{typeName}\"")
        {
            TypeName = typeName;
        }
        public string TypeName { get; }
    }
}
