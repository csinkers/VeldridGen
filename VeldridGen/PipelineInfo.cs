using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    public class PipelineInfo
    {
        public INamedTypeSymbol VertexShader { get; }
        public INamedTypeSymbol FragmentShader { get; }

        public PipelineInfo(INamedTypeSymbol symbol, GenerationContext context)
        {
            foreach (var attrib in symbol.GetAttributes())
            {
                if (attrib.AttributeClass == null)
                    continue;

                if (attrib.AttributeClass.Equals(context.Symbols.Attributes.VertexShader, SymbolEqualityComparer.Default))
                {
                    if (VertexShader != null)
                    {
                        context.Report($"Vertex shader already set for pipeline {symbol.Name} (Pipeline holder types can only have a single VertexShader attribute)");
                        continue;
                    }
                    VertexShader = (INamedTypeSymbol)attrib.ConstructorArguments[0].Value;
                }
                else if (attrib.AttributeClass.Equals(context.Symbols.Attributes.FragmentShader, SymbolEqualityComparer.Default))
                {
                    if (FragmentShader != null)
                    {
                        context.Report($"Fragment shader already set for pipeline {symbol.Name} (Pipeline holder types can only have a single FragmentShader attribute)");
                        continue;
                    }
                    FragmentShader = (INamedTypeSymbol)attrib.ConstructorArguments[0].Value;
                }
            }

            if (VertexShader == null || FragmentShader == null)
                context.Report(DiagnosticSeverity.Warning, $"{symbol.Name} derives from IPipelineHolder, but does not contain any shader attributes");
        }
    }
}
