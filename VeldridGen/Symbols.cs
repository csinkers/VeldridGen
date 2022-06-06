using Microsoft.CodeAnalysis;
using VeldridGen.Symbols;

namespace VeldridGen
{
    public class AllSymbols
    {
        public InterfaceSymbols Interfaces { get; }
        public AttributeSymbols Attributes { get; }
        public BuiltInSymbols BuiltIn { get; }
        public VeldridSymbols Veldrid { get; }
        public AllSymbols(Compilation compilation)
        {
            Interfaces = new InterfaceSymbols(compilation);
            Attributes = new AttributeSymbols(compilation);
            BuiltIn = new BuiltInSymbols(compilation);
            Veldrid = new VeldridSymbols(compilation);
        }
    }
}
