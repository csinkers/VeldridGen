using System.Linq;
using Microsoft.CodeAnalysis;
using Xunit;

namespace VeldridGen.Tests
{
    public class ResourceSetTests
    {
        [Fact]
        public void ResourceSetTest1()
        {
            const string source = 
                @"using System.ComponentModel;
using System.Numerics;
using VeldridGen.Interfaces;
using Veldrid;

namespace VeldridGenTests
{
" + 
                BaseClasses.ResourceSetHolderSource +
                BaseClasses.SingleBufferSource +
                BaseClasses.Texture2DHolderSource + @"
    public struct GlobalInfo: IUniformFormat
    {
        public Vector3 CameraPosition;
        public float Time;
    }

    public sealed partial class CommonSet : ResourceSetHolder
    {
        [Resource(""_Shared"")]                         SingleBuffer<GlobalInfo>     _globalInfo; 
        [Resource(""uPalette"", ShaderStages.Fragment)] Texture2DHolder              _palette;
    }
}
";
            Compilation comp = TestCommon.CreateCompilation(source);
            Compilation newComp = TestCommon.RunGenerators(comp, out var generatorDiags, new VeldridGenerator());
            var generatedTrees = newComp.RemoveSyntaxTrees(comp.SyntaxTrees).SyntaxTrees.ToList();

            Assert.Single(generatedTrees);
            Assert.Empty(generatorDiags);
            var diag = newComp.GetDiagnostics();
            Assert.Empty(diag);
        }
    }
}
