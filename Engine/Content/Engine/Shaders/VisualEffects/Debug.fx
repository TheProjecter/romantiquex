#include "VisualEffectBase.hlsl"
#include "../CommonVertexShaders.hlsl"

int Layer;
int LayerType;

void PS_Debug(
	in float4 inPosition	: SV_POSITION,
	in float2 inTexCoord	: TEXCOORD,
	
	out float4 outColor	: SV_TARGET)
{
	int4 sampleCoord = int4(inPosition.xy, Layer, 0);
	
	if (LayerType == 0)
		outColor = ColorLayers.Load(sampleCoord);
	else if (LayerType == 1)
		outColor = NormalLayers.Load(sampleCoord);
	else if (LayerType == 2)
		outColor = float4(DepthLayers.Load(sampleCoord).rrr, 1);
	else
		outColor = 0;	
}

technique10 RenderEffect
{
	pass SinglePass
	{
		SetVertexShader(CompileShader(vs_4_0, VS_FullScreenQuad()));
		SetGeometryShader(NULL);
		SetPixelShader(CompileShader(ps_4_0, PS_Debug()));
	}
}
