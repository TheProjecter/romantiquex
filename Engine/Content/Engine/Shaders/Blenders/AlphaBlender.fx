#include "BlenderBase.hlsl"
#include "../CommonVertexShaders.hlsl"

float4 PS_AlphaBlender(float4 position : SV_POSITION) : SV_TARGET	
{
	float4 color = float4(0, 0, 0, 1);
	for (int i = 0; i < LayerCount; ++i)
	{
		float4 layerColor = ColorLayersToBlend.Load(int4(position.xy, LayerCount - i - 1, 0));
		color = lerp(color, layerColor, layerColor.a);
	}
	return color;
}

technique10 BlendLayers
{
	pass SinglePass
	{
		SetVertexShader(CompileShader(vs_4_0, VS_FullScreenQuad_Position()));
        SetGeometryShader(NULL);
        SetPixelShader(CompileShader(ps_4_0, PS_AlphaBlender()));
	}
}
