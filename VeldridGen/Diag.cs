using Microsoft.CodeAnalysis;

namespace VeldridGen
{
#pragma warning disable RS1017 // DiagnosticId for analyzers must be a non-null constant
    static class Diag
    {
        const string Prefix = "VGen"; // Error code prefix
        static string Id(int id) => $"{Prefix}{id:D3}";

        public static DiagnosticDescriptor Generic = new(Id(1), "Veldrid Code Generation Error", "{0}", "General", DiagnosticSeverity.Error, true);
        public static DiagnosticDescriptor TypeResolution =
         new(Id(2), "VeldridGen Type Resolution Failure",
             "The symbol \"{0}\" could not be found in the current compilation. Veldrid and VeldridGen.Interfaces must be referenced by all assemblies invoking VeldridGen.",
             "Dependencies", DiagnosticSeverity.Error, true);
    }
#pragma warning restore RS1017 // DiagnosticId for analyzers must be a non-null constant
}