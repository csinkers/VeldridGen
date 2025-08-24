using Microsoft.CodeAnalysis;

namespace VeldridGen;
#pragma warning disable RS1017 // DiagnosticId for analyzers must be a non-null constant
internal static class Diag
{
    const string Prefix = "VGen"; // Error code prefix
    static string Id(int id) => $"{Prefix}{id:D3}";

    static readonly DiagnosticDescriptor ErrorDef = new(Id(1), "Veldrid Code Generation Error", "{0} {1}", "General", DiagnosticSeverity.Error, true);
    static readonly DiagnosticDescriptor WarnDef = new(Id(2), "Veldrid Code Generation Warning", "{0} {1}", "General", DiagnosticSeverity.Warning, true);
    static readonly DiagnosticDescriptor InfoDef = new(Id(3), "Veldrid Code Generation Info", "{0} {1}", "General", DiagnosticSeverity.Info, true);

    public static readonly DiagnosticDescriptor TypeResolution =
        new(Id(4), "VeldridGen Type Resolution Failure",
            "The symbol \"{0}\" could not be found in the current compilation. Veldrid and VeldridGen.Interfaces must be referenced by all assemblies invoking VeldridGen.",
            "Dependencies", DiagnosticSeverity.Error, true);

    static void Common(SourceProductionContext context, DiagnosticDescriptor descriptor, string message)
    {
        message = message.Replace("\r\n", "\\n");
        var diag = Diagnostic.Create(ErrorDef, null, descriptor.DefaultSeverity, message);
        context.ReportDiagnostic(diag);
    }

    public static void Error(this SourceProductionContext context, string message) => Common(context,ErrorDef, message);
    public static void Warn(this SourceProductionContext context, string message) => Common(context,WarnDef, message);
    public static void Info(this SourceProductionContext context, string message) => Common(context,InfoDef, message);
}
#pragma warning restore RS1017 // DiagnosticId for analyzers must be a non-null constant
