using Microsoft.CodeAnalysis;

namespace VeldridGen.Symbols;

public class BuiltInSymbols(Compilation compilation)
{
    // Built-in types
    public INamedTypeSymbol Byte { get; } = VeldridGenUtil.Resolve(compilation, typeof(byte).FullName!);
    public INamedTypeSymbol Short { get; } = VeldridGenUtil.Resolve(compilation, typeof(short).FullName!);
    public INamedTypeSymbol UShort { get; } = VeldridGenUtil.Resolve(compilation, typeof(ushort).FullName!);
    public INamedTypeSymbol Int { get; } = VeldridGenUtil.Resolve(compilation, typeof(int).FullName!);
    public INamedTypeSymbol UInt { get; } = VeldridGenUtil.Resolve(compilation, typeof(uint).FullName!);
    public INamedTypeSymbol Float { get; } = VeldridGenUtil.Resolve(compilation, typeof(float).FullName!);
    public INamedTypeSymbol Double { get; } = VeldridGenUtil.Resolve(compilation, typeof(double).FullName!);
    public INamedTypeSymbol Vector2 { get; } = VeldridGenUtil.Resolve(compilation, typeof(System.Numerics.Vector2).FullName!);
    public INamedTypeSymbol Vector3 { get; } = VeldridGenUtil.Resolve(compilation, typeof(System.Numerics.Vector3).FullName!);
    public INamedTypeSymbol Vector4 { get; } = VeldridGenUtil.Resolve(compilation, typeof(System.Numerics.Vector4).FullName!);
    public INamedTypeSymbol Matrix4X4 { get; } = VeldridGenUtil.Resolve(compilation, typeof(System.Numerics.Matrix4x4).FullName!);
    public INamedTypeSymbol NotifyPropertyChanged { get; } = VeldridGenUtil.Resolve(compilation, typeof(System.ComponentModel.INotifyPropertyChanged).FullName!);
}
