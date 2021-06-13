using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace VeldridCodeGen
{
    class VeldridTypeInfo
    {
        public VeldridTypeInfo(INamedTypeSymbol symbol, Symbols symbols)
        {
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            foreach (var iface in symbol.AllInterfaces)
            {
                if (symbols.UniformFormat.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsUniformFormat;
                else if (symbols.VertexFormat.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsVertexFormat;
                else if (symbols.ResourceSetHolder.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsResourceSetHolder;
                else if (symbols.FramebufferHolder.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsFramebufferHolder;
                else if (symbols.PipelineHolder.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsPipelineHolder;
                else if (symbols.SamplerHolder.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsSamplerHolder;
                else if (symbols.BufferHolder.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsBufferHolder;
                else if (symbols.TextureHolder.Equals(iface, SymbolEqualityComparer.Default))
                    Flags |= TypeFlags.IsTextureHolder;
            }
        }

        public TypeFlags Flags { get; }
        public INamedTypeSymbol Symbol { get; }
        public List<VeldridMemberInfo> Members { get; } = new();

        public void AddMember(ISymbol memberSym, Symbols symbols)
        {
            var info = new VeldridMemberInfo(memberSym, symbols);
            if (info.Flags != 0)
                Members.Add(info);
        }
    }
}
