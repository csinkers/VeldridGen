using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace VeldridCodeGen
{
    static class ResourceSetGenerator
    {
        public static void Generate(StringBuilder sb, VeldridTypeInfo type, Symbols symbols)
        {
            /* e.g.
            new ResourceLayoutElementDescription("uSprite", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
            new ResourceLayoutElementDescription("uSpriteSampler", ResourceKind.Sampler, ShaderStages.Fragment),
            new ResourceLayoutElementDescription("_Uniform", ResourceKind.UniformBuffer, ShaderStages.Vertex | ShaderStages.Fragment));
            */
            sb.AppendLine("        public static readonly ResourceLayoutDescription Layout = new(");
            bool first = true;
            foreach (var member in type.Members.Where(x => x.Resource != null))
            {
                if (!first)
                    sb.AppendLine(",");

                var shaderStages = member.Resource.Stages.ToString(); // Util.FormatFlagsEnum(member.Resource.Stages);
                sb.Append($"            new ResourceLayoutElementDescription(\"{member.Resource.Name}\", global::{member.Resource.Kind}, (ShaderStages){shaderStages})");
                first = false;
            }
            sb.AppendLine(");");
            sb.AppendLine();

            foreach (var member in type.Members.Where(x => (x.Flags & MemberFlags.IsResource) != 0))
            {
                if (Equals(member.Resource.Kind, symbols.ResourceKind.UniformBuffer.ToDisplayString())) GenerateUniform(sb, member);
                else if (Equals(member.Resource.Kind, symbols.ResourceKind.TextureReadOnly.ToDisplayString())) GenerateTexture(sb, member);
                else if (Equals(member.Resource.Kind, symbols.ResourceKind.Sampler.ToDisplayString())) GenerateSampler(sb, member);
                else throw new ArgumentOutOfRangeException();
            }

            /* e.g. protected override ResourceSet Build(GraphicsDevice device, ResourceLayout layout) =>
                device.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                    layout,
                    _globalInfo.DeviceBuffer,
                    _projection.DeviceBuffer,
                    _view.DeviceBuffer,
                    _palette.TextureView)); */

            sb.Append(@"        protected override ResourceSet Build(GraphicsDevice device, ResourceLayout layout) =>
            device.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                layout");

            foreach (var member in type.Members.Where(x => x.Resource != null))
            {
                sb.AppendLine(",");

                if (member.Symbol is not IFieldSymbol field)
                    throw new ArgumentOutOfRangeException("Resource set backing members must be fields");

                sb.Append("                ");
                sb.Append(field.Name);
                sb.Append('.');

                if (Equals(member.Resource.Kind, symbols.ResourceKind.UniformBuffer.ToDisplayString())) sb.Append("DeviceBuffer");
                else if (Equals(member.Resource.Kind, symbols.ResourceKind.TextureReadOnly.ToDisplayString())) sb.Append("TextureView");
                else if (Equals(member.Resource.Kind, symbols.ResourceKind.Sampler.ToDisplayString())) sb.Append("Sampler");
                else throw new ArgumentOutOfRangeException();
            }

            sb.AppendLine("));");
        }

        static void GenerateSampler(StringBuilder sb, VeldridMemberInfo member)
        {
            if (member.Symbol is not IFieldSymbol field)
                throw new ArgumentOutOfRangeException("Resource set backing members must be fields");

            /* e.g.
        public SamplerHolder Sampler
        {
            get => _sampler;
            set
            {
                if (_sampler == value) return; 
                if (_sampler != null) _sampler.PropertyChanged -= PropertyDirty; 
                _sampler = value; 
                if (_sampler != null) _sampler.PropertyChanged += PropertyDirty;
                Dirty();
            }
        } */
            var propertyName = Util.UnderscoreToTitleCase(field.Name);
            sb.AppendLine($@"        public {field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {propertyName}
        {{
            get => {field.Name};
            set
            {{
                if ({field.Name} == value) 
                    return;

                if ({field.Name} != null)
                    {field.Name}.PropertyChanged -= PropertyDirty;

                {field.Name} = value;

                if ({field.Name} != null)
                    {field.Name}.PropertyChanged += PropertyDirty;
                Dirty();
            }}
        }}
");
        }

        static void GenerateTexture(StringBuilder sb, VeldridMemberInfo member)
        {
            if (member.Symbol is not IFieldSymbol field)
                throw new ArgumentOutOfRangeException("Resource set backing members must be fields");
            /* e.g.
        public Texture2DHolder Palette
        {
            get => _palette;
            set
            {
                if (_palette == value) return;
                if (_palette != null) _palette.PropertyChanged -= PropertyDirty;
                _palette = value;
                if (_palette != null) _palette.PropertyChanged += PropertyDirty;
                Dirty();
            }
        } */
            var propertyName = Util.UnderscoreToTitleCase(field.Name);
            sb.AppendLine($@"        public {field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {propertyName}
        {{
            get => {field.Name};
            set
            {{
                if ({field.Name} == value) return;

                if ({field.Name} != null)
                    {field.Name}.PropertyChanged -= PropertyDirty;

                {field.Name} = value;

                if ({field.Name} != null)
                    {field.Name}.PropertyChanged += PropertyDirty;
                Dirty();
            }}
        }}
");
        }

        static void GenerateUniform(StringBuilder sb, VeldridMemberInfo member)
        {
            if (member.Symbol is not IFieldSymbol field)
                throw new ArgumentOutOfRangeException("Resource set backing members must be fields");

            /* e.g.
            public SingleBuffer<GlobalInfo> GlobalInfo
            {
                get => _globalInfo;
                set
                {
                    if (_globalInfo == value)
                        return;
                    _globalInfo = value;
                    Dirty();
                }
            }*/
            var propertyName = Util.UnderscoreToTitleCase(field.Name);
            sb.AppendLine($@"        public {field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {propertyName}
        {{
            get => {field.Name};
            set
            {{
                if ({field.Name} == value)
                    return;
                {field.Name} = value;
                Dirty();
            }}
        }}
");
        }

        /* GLSL:
layout(set = 1, binding = 0) uniform _Shared {
	vec3 uWorldSpacePosition;  // 12
	uint _s_padding_1;         // 16
	vec3 uCameraLookDirection; // 28
	uint _s_padding_2;         // 32

	vec2 uResolution;    // 40
	float uTime;         // 44
	float uSpecial1;     // 48

	float uSpecial2;     // 52
	uint uEngineFlags;   // 56
	float uPaletteBlend; // 60
	uint _s_padding_3;   // 64
}; */
    }
}