using Microsoft.CodeAnalysis;

namespace VeldridGen.Symbols;

public class BuiltInSymbols
{
    // Built-in types
    public INamedTypeSymbol Byte { get; }
    public INamedTypeSymbol Short { get; }
    public INamedTypeSymbol UShort { get; }
    public INamedTypeSymbol Int { get; }
    public INamedTypeSymbol UInt { get; }
    public INamedTypeSymbol Float { get; }
    public INamedTypeSymbol Double { get; }
    public INamedTypeSymbol Vector2 { get; }
    public INamedTypeSymbol Vector3 { get; }
    public INamedTypeSymbol Vector4 { get; }
    public INamedTypeSymbol Matrix4x4 { get; }
    public INamedTypeSymbol NotifyPropertyChanged { get; }

    public BuiltInSymbols(Compilation compilation)
    {
        Byte = VeldridGenUtil.Resolve(compilation, typeof(byte).FullName!);
        Short = VeldridGenUtil.Resolve(compilation, typeof(short).FullName!);
        UShort = VeldridGenUtil.Resolve(compilation, typeof(ushort).FullName!);
        Int = VeldridGenUtil.Resolve(compilation, typeof(int).FullName!);
        UInt = VeldridGenUtil.Resolve(compilation, typeof(uint).FullName!);
        Float = VeldridGenUtil.Resolve(compilation, typeof(float).FullName!);
        Double = VeldridGenUtil.Resolve(compilation, typeof(double).FullName!);
        Vector2 = VeldridGenUtil.Resolve(compilation, typeof(System.Numerics.Vector2).FullName!);
        Vector3 = VeldridGenUtil.Resolve(compilation, typeof(System.Numerics.Vector3).FullName!);
        Vector4 = VeldridGenUtil.Resolve(compilation, typeof(System.Numerics.Vector4).FullName!);
        Matrix4x4 = VeldridGenUtil.Resolve(compilation, typeof(System.Numerics.Matrix4x4).FullName!);
        NotifyPropertyChanged = VeldridGenUtil.Resolve(compilation, typeof(System.ComponentModel.INotifyPropertyChanged).FullName!);
    }
}