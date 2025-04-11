#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
float2 Offset;

sampler TextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

sampler DisplacementSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

float4 MainPS(float4 pos : SV_POSITION, float4 color0 : COLOR0, float4 texCoord : TEXCOORD0) : COLOR
{
	texCoord.zw = texCoord.xy + Offset.xy;
	texCoord.zw = SpriteTexture.Sample(DisplacementSampler, texCoord.zw);
	texCoord.zw = texCoord.zw * float2(0.2f,0.2f) + float2(-0.1f,-0.1f);
	color0.xy = texCoord.zw * color0.ww + texCoord.xy;
	color0 = SpriteTexture.Sample(TextureSampler, color0.xy);
	return color0;
}

technique Pixelate
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};