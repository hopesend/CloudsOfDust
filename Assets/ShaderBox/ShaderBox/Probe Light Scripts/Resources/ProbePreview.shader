Shader "Hidden/PC/ProbePreview"
{

Properties {
    _ColorX("ColorX", Color) = (1,1,1,1)
    _ColorY("ColorY", Color) = (1,1,1,1)
    _ColorZ("ColorZ", Color) = (1,1,1,1)
    _ColorNX("ColorNX", Color) = (1,1,1,1)
    _ColorNY("ColorNY", Color) = (1,1,1,1)
    _ColorNZ("ColorNZ", Color) = (1,1,1,1)
}


SubShader { 
Pass {

	Tags {"LightMode" = "ForwardBase"}
	LOD 400

	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma fragmentoption ARB_precision_hint_fastest

    #include "UnityCG.cginc"

    uniform half4 _ColorX;
    uniform half4 _ColorY;
    uniform half4 _ColorZ;
    uniform half4 _ColorNX;
    uniform half4 _ColorNY;
    uniform half4 _ColorNZ;


	struct v2f
	{
		float4	pos : SV_POSITION;
		float2	uv : TEXCOORD0;
        fixed3  customColor: TEXCOORD1;
	}; 
     
    
    float _Amount;

    v2f vert (appdata_full v) 
    {
        v2f o;
	    float3 nSquared;
        fixed3 aColor = fixed3(0,0,0);
        float3 normal = mul((float3x3)_Object2World, v.normal).xyz;

	    nSquared = normal * normal;

        if (normal.x > 0)
	        aColor += nSquared.x * _ColorX.xyz;
        else aColor += nSquared.x * _ColorNX.xyz;
        if (normal.y > 0)
	        aColor += nSquared.y * _ColorY.xyz;
        else aColor += nSquared.y * _ColorNY.xyz;
        if (normal.z > 0)
	        aColor += nSquared.z * _ColorZ.xyz;
        else aColor += nSquared.z * _ColorNZ.xyz;

        o.customColor = aColor;
      
        v.vertex.xyz += v.normal * _Amount;     

		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord.xy;

		return o;
    }


    float4 frag (v2f i) : COLOR
    {

        return float4(i.customColor,1);
    }


    ENDCG
}

}

FallBack "VertexLit"
}