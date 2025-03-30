using Microsoft.CodeAnalysis;
using VeldridGen.Symbols;

namespace VeldridGen;

public class AllSymbols(Compilation compilation)
{
    public InterfaceSymbols Interfaces { get; } = new(compilation);
    public AttributeSymbols Attributes { get; } = new(compilation);
    public BuiltInSymbols BuiltIn { get; } = new(compilation);
    public VeldridSymbols Veldrid { get; } = new(compilation);
}
