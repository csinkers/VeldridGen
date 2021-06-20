# VeldridGen

VeldridGen is an experimental library focused on using .NET 5 Source Generators to improve the development experience when using [Veldrid](https://github.com/mellinoe/veldrid).

Partial classes defining the inputs/outputs of shaders, the layout of resource sets, the layout of uniform buffers etc can be written in a concise form and the corresponding Veldrid descriptor structs, convenience properties, GLSL header file etc will be auto-generated with the aim of changes only needing to be made in one place rather than manually ensuring that the GLSL definitions match up with the struct layouts and descriptors etc.

As an example, the classes in [CommonResources.cs](Example/UAlbion.Core.SpriteRenderer/CommonResources.cs) and [SpriteShader.cs](Example/UAlbion.Core.SpriteRenderer/SpriteShader.cs) provide a minimal description of the shaders and resources required for a sprite batcher. The results of code generation can be seen [here](Example/UAlbion.Core.SpriteRenderer/Generated/VeldridGen/VeldridGen.VeldridGenerator/), which allows the renderer class to be simple and straightforward: [SpriteRenderer.cs](Example/UAlbion.Core.SpriteRenderer/SpriteRenderer.cs).

Currently the generated code is fairly dependent on some base classes and patterns in UAlbion.Core, but future development will make the code generation more customisable to fit a wider range of use cases / engine designs.

Note that compiling the solution will result in a small portion of the generated code being run ([UAlbion.ShaderWriter](Example/UAlbion.ShaderWriter/Program.cs) is run as a post-build event to populate the .h.vert and .h.frag shader headers).
