using Veldrid;
namespace UAlbion.Core.SpriteRenderer
{
    public partial class SpriteVertexShader
    {

        public static (string, string) ShaderSource()
        {
            return ("SpriteSV.vert", @"
//!#version 450 // Comments with //! are just for the VS GLSL plugin
//!#extension GL_KHR_vulkan_glsl: enable

// SpriteFlags
#define SF_NONE 0x0U
#define SF_LEFT_ALIGNED 0x1U
#define SF_MID_ALIGNED 0x2U
#define SF_BOTTOM_ALIGNED 0x4U
#define SF_ALIGNMENT_MASK 0x7U
#define SF_FLIP_VERTICAL 0x8U
#define SF_FLOOR 0x10U
#define SF_BILLBOARD 0x20U
#define SF_ONLY_EVEN_FRAMES 0x40U
#define SF_TRANSPARENT 0x80U
#define SF_HIGHLIGHT 0x100U
#define SF_RED_TINT 0x200U
#define SF_GREEN_TINT 0x400U
#define SF_BLUE_TINT 0x800U
#define SF_DEBUG_MASK 0xE00U
#define SF_DROP_SHADOW 0x1000U
#define SF_NO_BOUNDING_BOX 0x2000U
#define SF_GRADIENT_PIXELS 0x4000U
#define SF_OPACITY_MASK 0xFF000000U

// EngineFlags
#define EF_SHOW_BOUNDING_BOXES 0x1U
#define EF_SHOW_CENTRE 0x2U
#define EF_FLIP_DEPTH_RANGE 0x4U
#define EF_FLIP_YSPACE 0x8U
#define EF_VSYNC 0x10U
#define EF_HIGHLIGHT_SELECTION 0x20U
#define EF_USE_CYL_BILLBOARDS 0x40U
#define EF_RENDER_DEPTH 0x80U

// SpriteKeyFlags
#define SKF_NO_DEPTH_TEST 0x1U
#define SKF_USE_ARRAY_TEXTURE 0x2U
#define SKF_USE_PALETTE 0x4U
#define SKF_NO_TRANSFORM 0x8U


");
        }
    }
}
