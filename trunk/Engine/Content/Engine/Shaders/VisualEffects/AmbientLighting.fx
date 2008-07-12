#include "LightingEffectBase.hlsl"

float4 PS_AmbientLighting(PS_INPUT input) : SV_TARGET	
{
	float4 color = ExtractColor(input.Position.xy, input.RTIndex);
	return float4(color.rgb * LightColor.rgb * LightIntensity, color.a);
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
