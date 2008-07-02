#include "../Engine/Shaders/MaterialBase.hlsl"

Texture2D DiffuseMap;

SamplerState SampleLinear
{
	Filter = MIN_MAG_MIP_LINEAR;
};

void Test_VS(
	in float4 inPosition	: POSITION,
	in float2 inTexCoord	: TEXCOORD,
	in float3 inNormal		: NORMAL,
	
	out float4 outPosition	: SV_POSITION,
	out float2 outTexCoord	: TEXCOORD0,
	out float3 outNormal	: TEXCOORD1)
{
	outPosition = mul(inPosition, WorldViewProjectionMatrix);
	outTexCoord = inTexCoord;
	outNormal = mul(inNormal, WorldMatrix);
}

void Test_PS(
	in float4 inPosition	: SV_POSITION,
	in float2 inTexCoord	: TEXCOORD0,
	in float3 inNormal		: TEXCOORD1,
	
	out float4 outColor		: SV_Target0,
	out float4 outNormal	: SV_Target1)
{
	PeelDepth(inPosition);
	
	// Output material info
	outNormal = float4(inNormal * 0.5f + 0.5f, 1);
	outColor = DiffuseMap.Sample(SampleLinear, inTexCoord);
	outColor.a *= 0.7f;
}

RasterizerState RS
{
	CullMode = None;
};

technique10 FillGBuffer
{
  pass SinglePass
  {
    SetVertexShader(CompileShader(vs_4_0, Test_VS()));
    SetGeometryShader(NULL);
    SetPixelShader(CompileShader(ps_4_0, Test_PS()));
    
    SetRasterizerState(RS);
  }
}
