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
	color0 = SpriteTexture.Sample(TextureSampler, texCoord.xy);
	return float4(-color0.xyz + 1.0f, color0.w);
}

technique Invert
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};