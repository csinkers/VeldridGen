using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    class VeldridTypeInfo
    {
        public VeldridTypeInfo(INamedTypeSymbol symbol, GenerationContext context)
        {
            T Try<T>(string locus, Func<T> func) where T : class
            {
                try { return func(); }
                catch (Exception e)
                {
                    context.Report($"{symbol.ToDisplayString()} could not be initialised as a {locus}: {e.Message}");
                    return null;
                }
            }

            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            if (context == null) throw new ArgumentNullException(nameof(context));
            var symbols = context.Symbols;

            foreach (var iface in symbol.AllInterfaces)
            {
                if (symbols.UniformFormat.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsUniformFormat; // Verify size is a 16-byte multiple, verify no vectors cross 16-byte boundaries etc
                else if (symbols.VertexFormat.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsVertexFormat;
                else if (symbols.ResourceSetHolder.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsResourceSetHolder;
                else if (symbols.FramebufferHolder.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsFramebufferHolder;
                else if (symbols.PipelineHolder.Equals(iface, SymbolEqualityComparer.Default))
                {
                    Pipeline = Try("pipeline", () => new PipelineInfo(symbol, context));
                    if (Pipeline != null)
                        Flags |= TypeFlags.IsPipelineHolder;
                }
                else if (symbols.SamplerHolder.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsSamplerHolder;
                else if (symbols.BufferHolder.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsBufferHolder;
                else if (symbols.TextureHolder.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsTextureHolder;
                else if (symbols.TextureArrayHolder.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsTextureArrayHolder;
                else if (symbols.VertexShader.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsVertexShader;
                else if (symbols.FragmentShader.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsFragmentShader;
            }

            if ((Flags & TypeFlags.IsShader) != 0)
            {
                ShaderType type;
                switch (Flags & TypeFlags.IsShader)
                {
                    case TypeFlags.IsVertexShader: type = ShaderType.Vertex; break;
                    case TypeFlags.IsFragmentShader: type = ShaderType.Fragment; break;
                    default:
                    {
                        context.Report($"{symbol.ToDisplayString()} cannot be declared as multiple types of shader ({Flags & TypeFlags.IsShader})");
                        Flags &= ~TypeFlags.IsShader;
                        type = ShaderType.None;
                        break;
                    }
                }

                if (type != ShaderType.None)
                {
                    Shader = Try("shader", () => new ShaderInfo(type, symbol, context));
                    if (Shader == null)
                        Flags &= ~TypeFlags.IsShader;
                }
            }
        }

        public TypeFlags Flags { get; }
        public INamedTypeSymbol Symbol { get; }
        public PipelineInfo Pipeline { get; }
        public ShaderInfo Shader { get; }
        public List<VeldridMemberInfo> Members { get; } = new();

        public void AddMember(ISymbol memberSym, GenerationContext context)
        {
            var info = new VeldridMemberInfo(memberSym, context);
            if (info.IsRelevant)
                Members.Add(info);
        }
    }
}
