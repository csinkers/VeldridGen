using System;

namespace VeldridCodeGen.Interfaces
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class VertexInputAttribute : Attribute
    {
        public override object TypeId => this;
        public int Order { get; }
        public Type Type { get; }
        public int InstanceStep { get; set; }
        public VertexInputAttribute(int order, Type type)
        {
            Order = order;
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}