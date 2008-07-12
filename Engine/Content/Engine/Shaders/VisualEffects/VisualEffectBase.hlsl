#ifndef VISUAL_EFFECT_BASE_HLSL
#define VISUAL_EFFECT_BASE_HLSL

Texture2DArray ColorLayers : COLORTEXTUREARRAY;
Texture2DArray NormalLayers : NORMALTEXTUREARRAY;
Texture2DArray DepthLayers : DEPTHTEXTUREARRAY;

int LayerCount : LAYERCOUNT;

struct GS_INPUT
{
	float4 Position		: SV_POSITION;
};

struct PS_INPUT
{
    float4 Position		: SV_POSITION;
    uint RTIndex		: SV_RENDERTARGETARRAYINDEX;
};

[maxvertexcount(24)]
void GS_VisualEffect(triangle GS_INPUT input[3], inout TriangleStream<PS_INPUT> stream)
{
    PS_INPUT output;
    for(int layer = 0; layer < LayerCount; ++layer)
    {
        output.RTIndex = layer;
        for(int vertex = 0; vertex < 3; ++vertex)
        {
            output.Position = input[vertex].Position;
            stream.Append(output);
        }
        stream.RestartStrip();
    }
}

float3 ExtractNormal(int2 position, int layer)
{
	return NormalLayers.Load(int4(position, layer, 0)).rgb * 2.f - 1.f;
}

float4 ExtractColor(int2 position, int layer)
{
	return ColorLayers.Load(int4(position, layer, 0));
}

#endif