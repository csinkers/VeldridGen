using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VeldridCodeGen
{
    [Generator]
    public class VeldridGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new VeldridSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
        }

        void GenerateResourceSets()
        {
        }

        void GenerateResourceSet()
        {
            // 1) class derives from IResourceLayout
            // 2) Generate for fields w/ ResourceAttribute
            /*
        public static readonly ResourceLayoutDescription Layout = new(
            new ResourceLayoutElementDescription("{resourceAttrib.Name}", ResourceKind.UniformBuffer, {resourceAttrib.Stages}),
            Kind: IBufferHolder => UniformBuffer, ITextureHolder => TextureReadOnly, ISamplerHolder => Sampler, TODO: Read/write textures, structured buffers

        ResourceSet _resourceSet; // TODO: Diagnostic if name taken
        string _name; // TODO: Diagnostic if name taken

        public CommonSet()
        {
            {config.ConstructorBlock(context)}
            On<DeviceCreatedEvent>(_ => Dirty());
            On<DestroyDeviceObjectsEvent>(_ => Dispose());
        }

        public ResourceSet DeviceSet => _resourceSet;

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                if (_resourceSet != null)
                    _resourceSet.Name = _name;
            }
        }

        public SingleBuffer<GlobalInfo> GlobalInfo // Copy type from field
        {
            get => _globalInfo;
            set
            {
                if (_globalInfo == value)
                    return;
                _globalInfo = value;
                Dirty(); // {config.PropertyChangedBlock("GlobalInfo", "_globalInfo")}
            }
        }

        public Texture2DHolder Palette // Copy type, transform name
        {
            get => _palette;
            set
            {
                if (_palette == value) return;

                if (_palette != null)
                    _palette.PropertyChanged -= PropertyDirty;

                _palette = value;

                if (_palette != null)
                    _palette.PropertyChanged += PropertyDirty;
                Dirty(); // {config.PropertyChangedBlock("Palette", "_palette")}
            }
        }

        // config.ExtraMethods
        protected override void Subscribed() => Dirty();
        protected override void Unsubscribed() => Dispose();
        void Dirty() => On<PrepareFrameResourceSetsEvent>(Update);
        void PropertyDirty(object sender, PropertyChangedEventArgs e) => Dirty();

        void Update(IVeldridInitEvent e)
        {
            if (_resourceSet != null)
                Dispose();

            var layoutSource = Resolve<IResourceLayoutSource>(); // {config.PreCreateSetBlock}
            _resourceSet = e.Device.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                layoutSource.Get(GetType(), e.Device), // {config.GetResourceLayoutBlock}
                _globalInfo.DeviceBuffer,
                _projection.DeviceBuffer,
                _view.DeviceBuffer,
                _palette.TextureView));

            _resourceSet.Name = Name;
            Off<PrepareFrameResourcesEvent>();
        }

        public void Dispose()
        {
            _resourceSet?.Dispose();
            _resourceSet = null;
        }
GLSL:
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
};

        */
        }

        void GenerateVertexFormats()
        {
            // Derives from IVertexFormat
            // Fields with InputParam attrib (name + optional format)
        }

        void GenerateVertexFormat()
        {
            /*
    public readonly partial struct Vertex2DTextured // match access specifier, name
    {
        public static VertexLayoutDescription Layout = new(
            new VertexElementDescription("vPosition", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2), // attrib.Name
            new VertexElementDescription("vTextCoords", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2));

            Types:
            float => Float1
            Vector2 => Float2
            Vector3 => Float3
            Vector4 => Float4
            uint => UInt1
            int => Int1
            // TODO: various packed formats, like half floats, 2xbyte, 4xbyte, 2xushort etc
    }
            Vertex GLSL:
// {type name}
layout(location = {index}) in vec2 {attrib.Name};
layout(location = {index}) in uint {attrib.Name};

             */
        }

        void GenerateFramebuffers()
        {

/*
    public partial class OffscreenFramebuffer
    {
        protected override Framebuffer CreateFramebuffer(IVeldridInitEvent e)
        {
            _depth = e.Device.ResourceFactory.CreateTexture(new TextureDescription(
                Width, Height, 1, 1, 1,
                PixelFormat.R32_Float, TextureUsage.DepthStencil, TextureType.Texture2D));

            _color = e.Device.ResourceFactory.CreateTexture(new TextureDescription(
                Width, Height, 1, 1, 1,
                PixelFormat.B8_G8_R8_A8_UNorm, TextureUsage.RenderTarget, TextureType.Texture2D));

            var description = new FramebufferDescription(_depth, _color);
            return e.Device.ResourceFactory.CreateFramebuffer(ref description);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _depth?.Dispose();
            _color?.Dispose();
            _depth = null;
            _color = null;
        }
    } */
        }

        void GenerateGlsl()
        {
            /*
            Vertex:
            Resource sets
            Vertex layouts / input params
            */
        }


    }

    public class VeldridSyntaxReceiver : ISyntaxContextReceiver
    {
        public List<FieldDeclarationSyntax> Fields { get; } = new();
        public List<PropertyDeclarationSyntax> Properties { get; } = new();
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
        }
    }

#if false
    [Generator]
    public class ShaderGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // Register any callbacks required
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var options = GetLoadOptions(context);
            var nameCodeSequence = SourceFilesFromAdditionalFiles(options);
            foreach ((string name, string code) in nameCodeSequence)
                context.AddSource($"{name}", SourceText.From(code, Encoding.UTF8));
        }

        static IEnumerable<(bool, AdditionalText)> GetLoadOptions(GeneratorExecutionContext context)
        {
            foreach (AdditionalText file in context.AdditionalFiles)
            {
                if (!Path.GetExtension(file.Path).Equals(".csv", StringComparison.OrdinalIgnoreCase)) 
                    continue;

                // are there any options for it?
                // context.AnalyzerConfigOptions.GetOptions(file)
                //     .TryGetValue("build_metadata.additionalfiles.CsvLoadType", out string? loadTimeString);
                // Enum.TryParse(loadTimeString, ignoreCase: true, out CsvLoadType loadType);

                context.AnalyzerConfigOptions
                    .GetOptions(file)
                    .TryGetValue("build_metadata.additionalfiles.CacheObjects", out string cacheObjectsString);

                bool.TryParse(cacheObjectsString, out bool cacheObjects);
                yield return (cacheObjects, file);
            }
        }

        static IEnumerable<(string, string)> SourceFilesFromAdditionalFile(bool cacheObjects, AdditionalText file)
        {
            string className = Path.GetFileNameWithoutExtension(file.Path);
            // string csvText = file.GetText()!.ToString();
            return new (string, string)[] { (className, GenerateClassFile(className)) };
        }

        static IEnumerable<(string, string)> SourceFilesFromAdditionalFiles(IEnumerable<(bool cacheObjects, AdditionalText file)> pathsData)
            => pathsData.SelectMany(d => SourceFilesFromAdditionalFile(d.cacheObjects, d.file));

        static string GenerateClassFile(string ns, string access, string className)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($@"
namespace {ns}
{{
    {access} partial class {className}
    {{
    }}
}}
");
            return sb.ToString();
        }

        [Generator]
        public class ResourceSetGenerator : ISourceGenerator
        {
            public void Initialize(GeneratorInitializationContext context)
            {
                // Register a syntax receiver that will be created for each generation pass
                context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
            }

            class SyntaxReceiver : ISyntaxReceiver
            {
                public List<FieldDeclarationSyntax> CandidateFields { get; } = new List<FieldDeclarationSyntax>();
                public List<PropertyDeclarationSyntax> CandidateProperties { get; } = new List<PropertyDeclarationSyntax>();

                /// <summary>
                /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
                /// </summary>
                public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
                {
                    // any field with at least one attribute is a candidate for property generation
                    if (syntaxNode is FieldDeclarationSyntax {AttributeLists: {Count: > 0}} fieldSyntax)
                        CandidateFields.Add(fieldSyntax);
                    else if (syntaxNode is PropertyDeclarationSyntax {AttributeLists: {Count: > 0}} propertySyntax)
                        CandidateProperties.Add(propertySyntax);
                }
            }

            public void Execute(GeneratorExecutionContext context)
            {
                if (context.SyntaxReceiver is not SyntaxReceiver receiver)
                    return;

                // CSharpParseOptions options = (context.Compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
                Compilation compilation = context.Compilation;
                INamedTypeSymbol resourceAttribSymbol = compilation.GetTypeByMetadataName("UAlbion.CodeGen.ResourceAttribute");
            }
        }

        [Generator]
        public class AutoNotifyGenerator : ISourceGenerator
        {
            const string attributeText = @"
using System;
namespace AutoNotify
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class AutoNotifyAttribute : Attribute
    {
        public AutoNotifyAttribute() { }
        public string PropertyName { get; set; }
    }
}
";

            public void Initialize(GeneratorInitializationContext context) 
                => context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());

            public void Execute(GeneratorExecutionContext context)
            {
                // add the attribute text
                context.AddSource("AutoNotifyAttribute", attributeText);

                // retrieve the populated receiver 
                if (context.SyntaxReceiver is not SyntaxReceiver receiver)
                    return;

                // we're going to create a new compilation that contains the attribute.
                // TODO: we should allow source generators to provide source during initialize, so that this step isn't required.
                var options = (CSharpParseOptions)((CSharpCompilation)context.Compilation).SyntaxTrees[0].Options;
                var parsedAttribText = CSharpSyntaxTree.ParseText(SourceText.From(attributeText, Encoding.UTF8), options);
                var compilation = context.Compilation.AddSyntaxTrees(parsedAttribText);

                // get the newly bound attribute, and INotifyPropertyChanged
                INamedTypeSymbol attributeSymbol = compilation.GetTypeByMetadataName("AutoNotify.AutoNotifyAttribute");
                INamedTypeSymbol notifySymbol    = compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");

                // loop over the candidate fields, and keep the ones that are actually annotated
                var fieldSymbols = new List<IFieldSymbol>();
                foreach (FieldDeclarationSyntax field in receiver.CandidateFields)
                {
                    SemanticModel model = compilation.GetSemanticModel(field.SyntaxTree);
                    foreach (VariableDeclaratorSyntax variable in field.Declaration.Variables)
                    {
                        // Get the symbol being declared by the field, and keep it if it's annotated
                        var fieldSymbol = (IFieldSymbol)model.GetDeclaredSymbol(variable);
                        if (fieldSymbol != null && fieldSymbol.GetAttributes().Any(ad => ad.AttributeClass!.Equals(attributeSymbol, SymbolEqualityComparer.Default)))
                            fieldSymbols.Add(fieldSymbol);
                    }
                }

                // group the fields by class, and generate the source
                foreach (var group in fieldSymbols.GroupBy(f => f.ContainingType))
                {
                    string classSource = ProcessClass(group.Key, group.ToList(), attributeSymbol, notifySymbol);
                    context.AddSource($"{group.Key.Name}_autoNotify.cs", classSource);
                }
            }

            string ProcessClass(INamedTypeSymbol classSymbol, List<IFieldSymbol> fields, ISymbol attributeSymbol, ISymbol notifySymbol)
            {
                if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
                    return null; //TODO: issue a diagnostic that it must be top level

                string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

                // begin building the generated source
                StringBuilder source = new StringBuilder($@"
using System.ComponentModel;

namespace {namespaceName}
{{
    public partial class {classSymbol.Name} : {notifySymbol.ToDisplayString()}
    {{
");

                // if the class doesn't implement INotifyPropertyChanged already, add it
                if (!classSymbol.Interfaces.Contains(notifySymbol))
                {
                    source.Append("public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;");
                }

                // create properties for each field 
                foreach (IFieldSymbol fieldSymbol in fields)
                {
                    ProcessField(source, fieldSymbol, attributeSymbol);
                }

                source.Append("} }");
                return source.ToString();
            }

            void ProcessField(StringBuilder source, IFieldSymbol fieldSymbol, ISymbol attributeSymbol)
            {
                // get the name and type of the field
                string fieldName = fieldSymbol.Name;
                ITypeSymbol fieldType = fieldSymbol.Type;

                // get the AutoNotify attribute from the field, and any associated data
                AttributeData attributeData = fieldSymbol.GetAttributes().Single(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
                TypedConstant overridenNameOpt = attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "PropertyName").Value;

                string propertyName = chooseName(fieldName, overridenNameOpt);
                if (propertyName.Length == 0 || propertyName == fieldName)
                {
                    //TODO: issue a diagnostic that we can't process this field
                    return;
                }

                source.Append($@"
public {fieldType} {propertyName} 
{{
    get 
    {{
        return this.{fieldName};
    }}

    set
    {{
        this.{fieldName} = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof({propertyName})));
    }}
}}

");

                string chooseName(string fieldName, TypedConstant overridenNameOpt)
                {
                    if (!overridenNameOpt.IsNull)
                    {
                        return overridenNameOpt.Value.ToString();
                    }

                    fieldName = fieldName.TrimStart('_');
                    if (fieldName.Length == 0)
                        return string.Empty;

                    if (fieldName.Length == 1)
                        return fieldName.ToUpper();

                    return fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1);
                }

            }

            /// <summary>
            /// Created on demand before each generation pass
            /// </summary>
            class SyntaxReceiver : ISyntaxReceiver
            {
                public List<FieldDeclarationSyntax> CandidateFields { get; } = new List<FieldDeclarationSyntax>();

                /// <summary>
                /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
                /// </summary>
                public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
                {
                    // any field with at least one attribute is a candidate for property generation
                    if (syntaxNode is FieldDeclarationSyntax {AttributeLists: {Count: > 0}} fieldDeclarationSyntax)
                        CandidateFields.Add(fieldDeclarationSyntax);
                }
            }
        }
    }
#endif
}
