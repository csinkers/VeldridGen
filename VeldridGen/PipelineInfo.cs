using System;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    class PipelineInfo
    {
        public INamedTypeSymbol VertexShader { get; }
        public INamedTypeSymbol FragmentShader { get; }

        public PipelineInfo(INamedTypeSymbol symbol, Symbols symbols)
        {
            foreach (var attrib in symbol.GetAttributes())
            {
                if (attrib.AttributeClass == null)
                    continue;

                if (attrib.AttributeClass.Equals(symbols.VertexShaderAttrib, SymbolEqualityComparer.Default))
                {
                    if (VertexShader != null) throw new InvalidOperationException("Vertex shader already set for pipeline " + symbol.Name);
                    VertexShader = (INamedTypeSymbol)attrib.ConstructorArguments[0].Value;
                }
                else if (attrib.AttributeClass.Equals(symbols.FragmentShaderAttrib, SymbolEqualityComparer.Default))
                {
                    if (FragmentShader != null) throw new InvalidOperationException("Fragment shader already set for pipeline " + symbol.Name);
                    FragmentShader = (INamedTypeSymbol)attrib.ConstructorArguments[0].Value;
                }
            }

            if (VertexShader == null || FragmentShader == null)
                throw new InvalidOperationException($"Pipeline {symbol.Name} requires one or more shader attributes");
        }
    }
}
