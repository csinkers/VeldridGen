namespace VeldridCodeGen
{
    public static class Attributes
    {
        public const string Namespace           = "VeldridCodeGen.Interfaces";
        public const string ColorAttachmentName = "ColorAttachmentAttribute";
        public const string DepthAttachmentName = "DepthAttachmentAttribute";
        public const string InputParamName      = "InputParamAttribute";
        public const string ResourceName        = "ResourceAttribute";

        public static string ColorAttachmentFullName = $"{Namespace}.{ColorAttachmentName}";
        public static string DepthAttachmentFullName = $"{Namespace}.{DepthAttachmentName}";
        public static string InputParamFullName      = $"{Namespace}.{InputParamName}";
        public static string ResourceFullName        = $"{Namespace}.{ResourceName}";

        public static string Source => $@"
using System;
using Veldrid;

namespace {Namespace}
{{
    public sealed class {ColorAttachmentName} : Attribute // For frame buffers
    {{
        public PixelFormat Format {{ get; }}
        public {ColorAttachmentName}(PixelFormat format) => Format = format;
    }}

    public sealed class {DepthAttachmentName} : Attribute // For frame buffers
    {{
        public PixelFormat Format {{ get; }}
        public {DepthAttachmentName}(PixelFormat format) => Format = format;
    }}

    public sealed class {InputParamName} : Attribute // For vertex struct members
    {{
        public {InputParamName}(string name) => Name = name;
        public {InputParamName}(string name, VertexElementFormat format)
        {{
            Name = name;
            Format = format;
        }}

        public string Name {{ get; }}
        public VertexElementFormat Format {{ get; }}
    }}

    public sealed class {ResourceName} : Attribute // For resource set members
    {{
        public {ResourceName}(string name)
        {{
            Name = name;
            Stages = ShaderStages.Fragment | ShaderStages.Vertex;
        }}

        public {ResourceName}(string name, ShaderStages stages)
        {{
            Name = name;
            Stages = stages;
        }}

        public string Name {{ get; }}
        public ShaderStages Stages {{ get; }}
    }}
}}
";

        /* Currently unused

           public interface IResourceLayout : IDisposable { string Name { get; set; } } 

           public class FragmentShaderAttribute : Attribute
           {
               public Type Type { get; }
               public FragmentShaderAttribute(Type type) => Type = type ?? throw new ArgumentNullException(nameof(type));
           }

           [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
           public class ResourceSetAttribute : Attribute
           {
               public override object TypeId => this;
               public Type Type { get; }
               public ResourceSetAttribute(Type type) => Type = type;
           }
    
           [AttributeUsage(AttributeTargets.Enum)]
           public sealed class ShaderVisibleAttribute : Attribute {}
    
           public sealed class UniformAttribute : Attribute
           {
               public UniformAttribute(string name) => Name = name;
               public string Name { get; }
           } 
    
           [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
           public class VertexFormatAttribute : Attribute
           {
               public override object TypeId => this;
               public Type Type { get; }
               public int InstanceStep { get; set; }
               public VertexFormatAttribute(Type type) 
                   => Type = type ?? throw new ArgumentNullException(nameof(type);
           }
    
           public class VertexShaderAttribute : Attribute
           {
               public Type Type { get; }
               public VertexShaderAttribute(Type type) 
                   => Type = type ?? throw new ArgumentNullException(nameof(type));
           }*/
    }
}

