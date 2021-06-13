//!#version 450 // Comments with //! are for tricking the Visual Studio GLSL plugin into doing the right thing
//!#extension GL_KHR_vulkan_glsl: enable

// UAlbion.Core.EngineFlags
#define EF_SHOW_BOUNDING_BOXES  0x1U
#define EF_SHOW_CENTRE          0x2U
#define EF_FLIP_DEPTH_RANGE     0x4U
#define EF_FLIP_Y_SPACE         0x8U
#define EF_VSYNC               0x10U
#define EF_HIGHLIGHT_SELECTION 0x20U
#define EF_USE_CYL_BILLBOARDS  0x40U
#define EF_RENDER_DEPTH        0x80U

// UAlbion.Core.SpriteKeyFlags
#define SKF_NO_DEPTH_TEST     0x1U
#define SKF_USE_ARRAY         0x2U
#define SKF_USE_PALETTE       0x4U
#define SKF_NO_TRANSFORM      0x8U

// UAlbion.Core.SpriteFlags
#define SF_NONE                  0
#define SF_ALIGNMENT_MASK      0x7U
#define SF_DEBUG_MASK        0xe00U
#define SF_OPACITY_MASK 0xFF000000U
#define SF_LEFT_ALIGNED        0x1U
#define SF_MID_ALIGNED         0x2U
#define SF_BOTTOM_ALIGNED      0x4U
#define SF_FLIP_VERTICAL       0x8U
#define SF_FLOOR              0x10U
#define SF_BILLBOARD          0x20U
#define SF_ONLY_EVEN_FRAMES   0x40U
#define SF_TRANSPARENT        0x80U
#define SF_HIGHLIGHT         0x100U
#define SF_RED_TINT          0x200U
#define SF_GREEN_TINT        0x400U
#define SF_BLUE_TINT         0x800U
#define SF_DROP_SHADOW      0x1000U
#define SF_NO_BOUNDING_BOX  0x2000U
#define SF_GRADIENT_PIXELS  0x4000U

// Resource Sets
layout(set = 0, binding = 0) uniform _Shared {
	vec3 uWorldSpacePosition;  // 12
	uint _s_padding_1;         // 16
	vec3 uCameraLookDirection; // 28
	uint _s_padding_2;         // 32

	vec2 uResolution;    // 40
	float uTime;         // 44
	float uSpecial1;     // 48

	float uSpecial2;     // 52
	uint uEngineFlags;   // 56
	float uPaletteBlend; // 60
	uint _s_padding_3;   // 64
};

layout(set = 0, binding = 1) uniform _Projection { mat4 uProjection; };
layout(set = 0, binding = 2) uniform _View { mat4 uView; };
layout(set = 0, binding = 3) uniform texture2D uPalette; //! // vdspv_0_3

layout(set = 1, binding = 0) uniform texture2DArray uSprite; //! // vdspv_1_0
layout(set = 1, binding = 1) uniform sampler uSpriteSampler; //! // vdspv_1_1

layout(set = 1, binding = 2) uniform _Uniform {
	uint uFlags;
	float uTexSizeW;
	float uTexSizeH;
	uint _u_padding_3;
};

// SpriteIntermediateData
layout(location = 0) in vec2 iTexPosition;   // Texture Coordinates
layout(location = 1) in flat float iLayer;   // Texture Layer
layout(location = 2) in flat uint  iFlags;   // Flags
layout(location = 3) in vec2 iNormCoords;    // Normalised sprite coordinates
layout(location = 4) in vec3 iWorldPosition; // World-space position

// ColorOutput
layout(location = 0) out vec4 oColor;

