using System;
using Veldrid;

namespace UAlbion.CodeGen
{
    public sealed class ResourceAttribute : Attribute
    {
        public ResourceAttribute(string name)
        {
            Name = name;
            Stages = ShaderStages.Fragment | ShaderStages.Vertex;
        }

        public ResourceAttribute(string name, ShaderStages stages)
        {
            Name = name;
            Stages = stages;
        }

        public string Name { get; }
        public ShaderStages Stages { get; }
    }

    public sealed class UniformAttribute : Attribute
    {
        public UniformAttribute(string name) => Name = name;

        public string Name { get; }
    }
    public sealed class InputParam : Attribute
    {
        public InputParam(string name) => Name = name;

        public string Name { get; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class ResourceSetAttribute : Attribute
    {
        public override object TypeId => this;
        public Type Type { get; }
        public ResourceSetAttribute(Type type) => Type = type;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class VertexFormatAttribute : Attribute
    {
        public override object TypeId => this;
        public Type Type { get; }
        public int InstanceStep { get; set; }
        public VertexFormatAttribute(Type type) => Type = type;
    }
}