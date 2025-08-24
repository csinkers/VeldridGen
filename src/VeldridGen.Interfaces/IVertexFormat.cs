namespace VeldridGen.Interfaces;

/// <summary>
/// Marker interface for struct types that implement vertex formats used in shaders.
/// The fields/properties of the struct should be decorated with <see cref="VertexAttribute"/> to indicate the names to be used in the shader. 
/// </summary>
public interface IVertexFormat { }
