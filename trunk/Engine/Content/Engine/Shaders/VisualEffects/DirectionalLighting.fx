#include "LightingEffectBase.hlsl"

float3 LightDirection;

float4 PS_DirectionalLighting(PS_INPUT input) : SV_TARGET	
{
	float4 color = ExtractColor(input.Position.xy, input.RTIndex);
	float3 normal = ExtractNormal(input.Position.xy, input.RTIndex);
	float3 lighting = max(0.f, dot(normal, -LightDirection)) * LightColor.rgb * LightIntensity;
	return float4(normal * 0.5f + 0.5f, 1.f);
	return float4(color.rgb * lighting, color.a);
}

technique10 RenderEffect
{
	pass SinglePass
	{
		SetVertexShader(CompileShader(vs_4_0, VS_FullScreenQuad_Position()));
        SetGeometryShader(CompileShader(gs_4_0, GS_VisualEffect()));
        SetPixelShader(CompileShader(ps_4_0, PS_DirectionalLighting()));
	}
}
