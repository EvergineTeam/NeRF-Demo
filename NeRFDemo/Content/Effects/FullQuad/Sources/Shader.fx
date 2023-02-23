[Begin_ResourceLayout]

	Texture2D Input 			: register(t0);
	SamplerState Sampler		: register(s0);

[End_ResourceLayout]

[Begin_Pass:Default]
	[Profile 10_0]
	[Entrypoints VS=VS PS=PS]

	struct VS_IN
	{
		uint id			: SV_VertexID;
	};

	struct Vertex {
		float4 pos;
		float2 tex;
	};
	
	struct PS_IN
	{
		float4 pos : SV_POSITION;
		float2 tex : TEXCOORD;
	};

	PS_IN VS(VS_IN input)
	{
		PS_IN output = (PS_IN)0;
	
		input.id %= 3;
	
		float modId = (int)input.id % 2;
		float divId = (int)input.id / 2;
	
		output.pos = float4(divId * 4 - 1, modId * 4 - 1, 0, 1);
		output.tex = float2(divId * 2, modId * -2 + 1);
	
		return output;
	}

	float4 GammaToLinear(const float4 color)
	{
		return float4(pow(color.rgb, 2.2), color.a);
	}
	
	float4 PS(PS_IN input) : SV_Target
	{
				float4 color = Input.Sample(Sampler, input.tex);
	
#if !GAMMA_COLORSPACE
		color = GammaToLinear(color);
#endif
		return color;
	}

[End_Pass]