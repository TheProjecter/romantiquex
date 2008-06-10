#include "VisualEffectBase.hlsl"
#include "../CommonVertexShaders.hlsl"

void PS_AmbientLighting(
	in float4 inPosition	: SV_POSITION,
	in float2 inTexCoord	: TEXCOORD,
	
	out float4 outColor	: SV_TARGET)
{
	outColor = float4(1, 1, 1, 1);
}

technique10 RenderEffect
{
	pass SinglePass
	{
		SetVertexShader(CompileShader(vs_4_0, VS_FullScreenQuad()));
        	SetGeometryShader(NULL);
        	SetPixelShader(CompileShader(ps_4_0, PS_AmbientLighting()));
	}
}
