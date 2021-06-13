//!#version 450 // Comments with //! are for tricking the Visual Studio GLSL plugin into doing the right thing
//!#extension GL_KHR_vulkan_glsl: enable

// UAlbion.Core.SpriteKeyFlags
#define SKF_NO_DEPTH_TEST     0x1
#define SKF_USE_ARRAY         0x2
#define SKF_USE_PALETTE       0x4
#define SKF_NO_TRANSFORM      0x8

// UAlbion.Core.SpriteFlags
#define SF_LEFT_ALIGNED       0x1
#define SF_MID_ALIGNED        0x2
#define SF_BOTTOM_ALIGNED     0x4
#define SF_FLIP_VERTICAL      0x8
#define SF_FLOOR             0x10
#define SF_BILLBOARD         0x20
#define SF_ONLY_EVEN_FRAMES  0x40
#define SF_TRANSPARENT       0x80
#define SF_HIGHLIGHT        0x100
#define SF_RED_TINT         0x200
#define SF_GREEN_TINT       0x400
#define SF_BLUE_TINT        0x800
#define SF_DROP_SHADOW     0x1000
#define SF_NO_BOUNDING_BOX 0x2000
#define SF_GRADIENT_PIXELS 0x4000

#define SF_ALIGNMENT_MASK      0x7U
#define SF_OPACITY_MASK 0xFF000000U

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

// Vertex2DTextured
layout(location = 0) in vec2 vPosition;
layout(location = 1) in vec2 vTexCoords;
// SpriteInstanceData
layout(location = 2) in vec3 iTransform1;
layout(location = 3) in vec3 iTransform2;
layout(location = 4) in vec3 iTransform3;
layout(location = 5) in vec3 iTransform4;
layout(location = 6) in vec2 iTexOffset;
layout(location = 7) in vec2 iTexSize;
layout(location = 8) in uint iTexLayer;
layout(location = 9) in uint iFlags;

// SpriteIntermediateData
layout(location = 0) out vec2 oTexPosition;   // Texture Coordinates
layout(location = 1) out flat float oLayer;   // Texture Layer
layout(location = 2) out flat uint oFlags;    // Flags
layout(location = 3) out vec2 oNormCoords;    // Normalised sprite coordinates
layout(location = 4) out vec3 oWorldPosition; // World position

