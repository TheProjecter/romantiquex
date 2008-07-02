#include "BlenderBase.hlsl"
#include "../CommonVertexShaders.hlsl"

int Layer;

float4 PS_DebugBlender(float4 position : SV_POSITION) : SV_TARGET	
{
	return ColorLayersToBlend.Load(int4(position.xy, Layer, 0));
}

technique10 BlendLayers
{
	pass SinglePass
	{
		SetVertexShader(CompileShader(vs_4_0, VS_FullScreenQuad_Position()));
        SetGeometryShader(NULL);
        SetPixelShader(CompileShader(ps_4_0, PS_DebugBlender()));
	}
}
