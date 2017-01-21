texture ScreenTexture;

sampler TextureSampler = sampler_state
{
	Texture = <ScreenTexture>;
};

float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(TextureSampler, uv);
	
	uv *= 1.0 - uv.yx;  
	float vig = uv.x*uv.y * 15.0;
	vig = pow(vig, 0.25);

	return float4(vig * color.xyz, color.w);
}

technique BlackAndWhite
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}