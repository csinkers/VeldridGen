using System;

namespace VeldridCodeGen.Interfaces
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class VertexFormatAttribute : Attribute
    {
        public override object TypeId => this;
        public Type Type { get; }
        public int InstanceStep { get; set; }
        public VertexFormatAttribute(Type type) => Type = type;
    }
}