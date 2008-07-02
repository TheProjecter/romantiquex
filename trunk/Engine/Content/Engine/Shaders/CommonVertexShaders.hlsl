#ifndef COMMON_VERTEX_SHADERS_H
#define COMMON_VERTEX_SHADERS_H

void VS_FullScreenQuad_Position(
	in uint inId		: SV_VertexID,
	
	out float4 outPosition	: SV_POSITION)
{
	float2 texCoord = float2((inId << 1) & 2, inId & 2);
	outPosition = float4(texCoord * float2(2.0f, -2.0f) + float2(-1.0f, 1.0f), 0.0f, 1.0f);
}

#endif