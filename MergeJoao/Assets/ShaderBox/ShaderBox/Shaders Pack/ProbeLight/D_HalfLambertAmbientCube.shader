
Shader "ShaderBox/Probe/___Low" {
Properties 
{
	_Diffbrightness("Diff Brightness", Float) = 1.0
	_Diffuse ("Diffuse map (RGB)", 2D) = "" {}

}

SubShader { 

	Tags { "RenderType"="Opaque" }
	LOD 100 

	Pass {
		Name "ContentBaseChar"
		Tags {"LightMode" = "ForwardBase"}


		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fwdbase
		#pragma fragmentoption ARB_precision_hint_fastest
		#include "UnityCG.cginc"
		#include "AutoLight.cginc"
		#include "HLSLSupport.cginc" 
   
		uniform sampler2D _Diffuse;
		uniform half4 _ColorX;
		uniform half4 _ColorY;
		uniform half4 _ColorZ;
		uniform half4 _ColorNX;
		uniform half4 _ColorNY;
		uniform half4 _ColorNZ;
		fixed4 _LightColor0;
		half _Diffbrightness;
 

		struct v2f
		{
			float4	pos : SV_POSITION;
			float2	uv : TEXCOORD0;
			float3	lightDir : TEXCOORD1;
			fixed3  customColor: TEXCOORD2;
			LIGHTING_COORDS(3,4)
		}; 


		v2f vert (appdata_full v) 
		{
			v2f o;
			float3 nSquared;
			fixed3 aColor = fixed3(0,0,0);
			float3 normal = normalize (mul((float3x3)_Object2World, v.normal).xyz);

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

			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = v.texcoord.xy;
			half3 lightDir =  ObjSpaceLightDir(v.vertex);

			#ifndef USING_DIRECTIONAL_LIGHT
				lightDir = normalize(lightDir);
			#endif

			o.lightDir = max (0.0,dot(v.normal, lightDir));

			TRANSFER_VERTEX_TO_FRAGMENT(o);
			return o;
		}

		float4 frag (v2f i) : COLOR
		{

			fixed4 texDiff = tex2D(_Diffuse,i.uv);
		    fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
			half atten = LIGHT_ATTENUATION(i);
			fixed4 final;
		    final.rgb = texDiff * lerp(i.customColor * atten * i.lightDir,i.customColor * (ambient + 1),ambient) * _LightColor0.rgb * _Diffbrightness;
			//final.rgb = texDiff.rgb * max(i.customColor * 1.5 , atten ) * _LightColor0.rgb * 0.5 * _Diffbrightness;
			final.a = texDiff.a; 
			return final;
		}


		ENDCG
	}

}

FallBack "VertexLit"
}

