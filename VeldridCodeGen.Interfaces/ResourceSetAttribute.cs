using System;

namespace VeldridCodeGen.Interfaces
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class ResourceSetAttribute : Attribute
    {
        public override object TypeId => this;
        public Type Type { get; }
        public ResourceSetAttribute(Type type) => Type = type;
    }
}