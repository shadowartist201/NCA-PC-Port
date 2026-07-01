#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
float Timer;
float Strength = 0.015f;

sampler TextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

float4 MainPS(float4 pos : SV_POSITION, float4 color0 : COLOR0, float4 texCoord : TEXCOORD0) : COLOR
{
    color0.x = Timer * 6.3661976f * color0.w + 0.5f;
    color0.x = frac(color0.x);
    color0.y = color0.x * 6.2831855f + -3.1415927f;
    color0.x = cos(color0.y);
    color0.y = sin(color0.y);
    color0.xy = color0.xy * Strength + texCoord.xy;
    color0.xyzw = SpriteTexture.Sample(TextureSampler, color0.xy);
    return color0;
}

technique Drunk
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};