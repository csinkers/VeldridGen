namespace VeldridGen.Interfaces;

/// <summary>
/// Marker interface for struct types that represent a uniform format in Veldrid.
/// The fields of the struct should be decorated with <see cref="UniformAttribute"/> to indicate the names to be used inside the shader code.
/// </summary>
public interface IUniformFormat { }
