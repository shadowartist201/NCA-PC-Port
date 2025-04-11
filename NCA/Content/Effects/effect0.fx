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
	float4 r2;
	
	texCoord = SpriteTexture.Sample(TextureSampler, texCoord.xy);
	r2.x = dot(texCoord.zxy, float3(0.11f,0.3f,0.59f));
	color0.xyz = r2.xxx >= color0.www;
	color0.w = texCoord.w;
	return color0;
}

technique BlackWhite
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};