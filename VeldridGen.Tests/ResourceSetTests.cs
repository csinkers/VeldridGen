using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Xunit;

namespace VeldridGen.Tests
{
    public class ResourceSetTests
    {
        [Fact]
        public void ResourceSetTest1()
        {
            const string source =
                @"using System.ComponentModel;
using System.Numerics;
using VeldridGen.Interfaces;
using Veldrid;

namespace VeldridGenTests
{
" +
                BaseClasses.ResourceSetHolderSource +
                BaseClasses.SingleBufferSource +
                BaseClasses.Texture2DHolderSource + @"
    public struct GlobalInfo: IUniformFormat
    {
        public Vector3 CameraPosition;
        public float Time;
    }

    public sealed partial class CommonSet : ResourceSetHolder
    {
        [Resource(""_Shared"")]                         SingleBuffer<GlobalInfo>     _globalInfo; 
        [Resource(""uPalette"", ShaderStages.Fragment)] Texture2DHolder              _palette;
    }
}
";
            Compilation comp = TestCommon.CreateCompilation(source);
            Compilation newComp = TestCommon.RunGenerators(comp, out var generatorDiags, new ResourceSetGenerator());
            var generatedTrees = newComp.RemoveSyntaxTrees(comp.SyntaxTrees).SyntaxTrees.ToList();

            Assert.Single(generatedTrees);
            Assert.Empty(generatorDiags);
            var diag = newComp.GetDiagnostics();
            Assert.Empty(diag);
        }

        class ResourceSetGenerator : VeldridGenerator
        {
            protected override void GenerateResourceSet(StringBuilder sb, VeldridTypeInfo type, GenerationContext context)
            {
                sb.AppendLine("        public static readonly ResourceLayoutDescription Layout = new(");
                bool first = true;
                foreach (var member in type.Members.Where(x => x.Resource != null))
                {
                    if (!first) sb.AppendLine(",");
                    var shaderStages = member.Resource.Stages.ToString();
                    sb.Append($"            new ResourceLayoutElementDescription(\"{member.Resource.Name}\", global::{member.Resource.Kind}, (ShaderStages){shaderStages})");
                    first = false;
                }
                sb.AppendLine(");");
                sb.AppendLine();
                foreach (var member in type.Members.Where(x => x.Resource != null))
                {
                    if (Equals(member.Resource.Kind, context.Symbols.ResourceKind.UniformBuffer.ToDisplayString())) GenerateUniform(sb, member, context);
                    else if (Equals(member.Resource.Kind, context.Symbols.ResourceKind.TextureReadOnly.ToDisplayString())) GenerateTexture(sb, member, context);
                    else if (Equals(member.Resource.Kind, context.Symbols.ResourceKind.Sampler.ToDisplayString())) GenerateSampler(sb, member, context);
                    else context.Report($"Resource {member.Symbol.ToDisplayString()} in set {type.Symbol.ToDisplayString()} was of unexpected kind {member.Resource.Kind}");
                }
                sb.Append(@"        protected override ResourceSet Build(GraphicsDevice device, ResourceLayout layout) =>
            device.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                layout");

                foreach (var member in type.Members.Where(x => x.Resource != null))
                {
                    sb.AppendLine(",");
                    if (member.Symbol is not IFieldSymbol field)
                    {
                        context.Report($"Resource set backing members must be fields (member {member.Symbol.ToDisplayString()} in {type.Symbol.ToDisplayString()} was a {member.Symbol.GetType().Name})");
                        continue;
                    }
                    sb.Append("                ");
                    sb.Append(field.Name);
                    sb.Append('.');

                    if (Equals(member.Resource.Kind, context.Symbols.ResourceKind.UniformBuffer.ToDisplayString())) sb.Append("DeviceBuffer");
                    else if (Equals(member.Resource.Kind, context.Symbols.ResourceKind.TextureReadOnly.ToDisplayString())) sb.Append("DeviceTexture");
                    else if (Equals(member.Resource.Kind, context.Symbols.ResourceKind.Sampler.ToDisplayString())) sb.Append("Sampler");
                    else context.Report($"Resource {member.Symbol.ToDisplayString()} in {type.Symbol.ToDisplayString()} was of unexpected kind \"{member.Resource.Kind}\"");
                }

                sb.AppendLine("));");
            }

            protected override void GenerateVertexFormat(StringBuilder sb, VeldridTypeInfo type, GenerationContext context) { }
            protected override void GenerateFramebuffer(StringBuilder sb, VeldridTypeInfo type, GenerationContext context) { }
            protected override void GeneratePipeline(StringBuilder sb, VeldridTypeInfo type, GenerationContext context) { }
            protected override void GenerateShader(StringBuilder sb, VeldridTypeInfo type, GenerationContext context) { }
            static void GenerateSampler(StringBuilder sb, VeldridMemberInfo member, GenerationContext context)
            {
                var field = AsField(member.Symbol, context);
                if (field == null) return;
                var propertyName = VeldridGenUtil.UnderscoreToTitleCase(field.Name);
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

            static void GenerateTexture(StringBuilder sb, VeldridMemberInfo member, GenerationContext context)
            {
                var field = AsField(member.Symbol, context);
                if (field == null) return;
                var propertyName = VeldridGenUtil.UnderscoreToTitleCase(field.Name);
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

            static void GenerateUniform(StringBuilder sb, VeldridMemberInfo member, GenerationContext context)
            {
                var field = AsField(member.Symbol, context);
                if (field == null) return;
                var propertyName = VeldridGenUtil.UnderscoreToTitleCase(field.Name);
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

            static IFieldSymbol AsField(ISymbol symbol, GenerationContext context)
            {
                if (symbol is IFieldSymbol field) return field;
                context.Report(
                    $"Resource set backing members must be fields (member {symbol.ToDisplayString()} in " +
                    $"{symbol.ContainingType.ToDisplayString()} was a {symbol.GetType().Name})");
                return null;
            }
        }
    }
}
