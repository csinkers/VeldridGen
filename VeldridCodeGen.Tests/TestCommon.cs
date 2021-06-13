using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace VeldridCodeGen.Tests
{
    public static class TestCommon
    {
        public static Compilation CreateCompilation(string source, OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary)
            => CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview)) },
                new[]
                {
                    MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "netstandard").Location),
                    MetadataReference.CreateFromFile(AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "System.Runtime").Location),
                    MetadataReference.CreateFromFile(typeof(INotifyPropertyChanged).GetTypeInfo().Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(Veldrid.GraphicsDevice).GetTypeInfo().Assembly.Location)
                },
                new CSharpCompilationOptions(outputKind));

        static GeneratorDriver CreateDriver(Compilation c, params ISourceGenerator[] generators)
            => CSharpGeneratorDriver.Create(generators, parseOptions: (CSharpParseOptions)c.SyntaxTrees.First().Options);

        public static Compilation RunGenerators(Compilation c, out ImmutableArray<Diagnostic> diagnostics, params ISourceGenerator[] generators)
        {
            CreateDriver(c, generators).RunGeneratorsAndUpdateCompilation(c, out var d, out diagnostics);
            return d;
        }
    }
}