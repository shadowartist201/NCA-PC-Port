#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler TextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

float4 MainPS(float4 pos : SV_POSITION, float4 color0 : COLOR0, float4 texCoord : TEXCOORD0) : COLOR
{
	color0.x = 10.0f * color0.w;
	color0.x = floor(color0.x);
	color0 = color0.x * float4(0.00078125, 0.0013888889, 0.000390625, 0.00069444446);
	texCoord.z = rcp(color0.x);
	texCoord.w = rcp(color0.y);
	texCoord.xy = texCoord.zw * texCoord.xy;
	texCoord.xy = floor(texCoord.xy);
	color0.xy = color0.xy * texCoord.xy + color0.zw;
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