#include "VisualEffectBase.hlsl"
#include "../CommonVertexShaders.hlsl"

float4 AmbientColor;
float AmbientIntensity;

float4 PS_AmbientLighting(PS_INPUT input) : SV_TARGET	
{
	float4 color = ColorLayers.Load(int4(input.Position.xy, input.RTIndex, 0));
	return float4(color.rgb * AmbientColor.rgb * AmbientIntensity, color.a);
}

technique10 RenderEffect
{
	pass SinglePass
	{
		SetVertexShader(CompileShader(vs_4_0, VS_FullScreenQuad_Position()));
        SetGeometryShader(CompileShader(gs_4_0, GS_VisualEffect()));
        SetPixelShader(CompileShader(ps_4_0, PS_AmbientLighting()));
	}
}
