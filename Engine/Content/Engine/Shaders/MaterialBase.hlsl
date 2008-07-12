#ifndef MATERIAL_BASE_HLSL
#define MATERIAL_BASE_HLSL

cbuffer Global
{
	float4x4 ViewMatrix					: VIEWMATRIX;
	float4x4 ProjectionMatrix			: PROJECTIONMATRIX;
	float4x4 ViewProjectionMatrix		: VIEWPROJECTIONMATRIX;
	float TotalTime						: TOTALTIME;
	float ElapsedTime					: ELAPSEDTIME;
};

cbuffer PerBatch
{
	float4x4 WorldMatrix				: WORLDMATRIX;
	float4x4 WorldViewMatrix			: WORLDVIEWMATRIX;
	float4x4 WorldViewProjectionMatrix	: WORLDVIEWPROJECTIONMATRIX;
};

Texture2DArray PreviousLayerDepthTexture : PREVIOUSLAYERDEPTHTEXTURE;

// Depth peeling
void PeelDepth(float4 fragmentPosition)
{
	float previousDepth = PreviousLayerDepthTexture.Load(int4(fragmentPosition.xy, 0, 0)).r;
	if (fragmentPosition.z <= previousDepth)
		discard;
}

#endif