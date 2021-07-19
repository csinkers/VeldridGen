# VeldridGen

VeldridGen is an experimental library focused on using .NET 5 Source Generators to improve the development experience when using [Veldrid](https://github.com/mellinoe/veldrid).

Partial classes defining the inputs/outputs of shaders, the layout of resource sets, the layout of uniform buffers etc can be written in a concise form and the corresponding Veldrid descriptor structs, convenience properties, GLSL header file etc will be auto-generated with the aim of changes only needing to be made in one place rather than manually ensuring that the GLSL definitions match up with the struct layouts and descriptors etc.

As an example, the classes in [CommonResources.cs](Example/SpriteRenderer/CommonResources.cs) and [SpriteShader.cs](Example/SpriteRenderer/SpriteShader.cs) provide a minimal description of the shaders and resources required for a sprite batcher. The results of code generation can be seen [here](Example/SpriteRenderer/Generated/VeldridGen.Example.Engine.CodeGen/VeldridGen.Example.Engine.CodeGen.ExampleVeldridGenerator/), which allows the renderer class to be simple and straightforward: [SpriteRenderer.cs](Example/SpriteRenderer/SpriteRenderer.cs).

Note that compiling the solution will result in a small portion of the generated code being run ([Example.TestApp](Example/TestApp/Program.cs) is run as a post-build event to populate the .h.vert and .h.frag shader headers).
