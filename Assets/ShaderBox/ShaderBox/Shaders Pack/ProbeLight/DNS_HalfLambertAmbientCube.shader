
Shader "ShaderBox/Probe/__Middle" {
Properties 
{
	_Diffbrightness("Diff Brightness", Float) = 1.0
	_SpecularPower("Specular", Float) = 0.0
	_GlossPower("Gloss", Float) = 0.0
	_Diffuse ("Diffuse map (RGB)", 2D) = "" {}
	_LightMask ("Specular (R)  Gloss (G) Reflection (B)", 2D) = "" {}
	_Normal ("Normal map (RGB)", 2D) = "" {}
}

CGINCLUDE

	struct SurfaceOutput2 {
		fixed3 Albedo;
		fixed3 Normal;
		fixed Gloss;
		half Specular;
		fixed Alpha;
		fixed3 CustomColor;
	};
		
	fixed4 _LightColor0;
	half _SpecularPower;
	half _GlossPower;
	half _Diffbrightness;

	#include "UnityCG.cginc"
	#include "AutoLight.cginc"
	#include "HLSLSupport.cginc"  
	

	inline half4 LightingHalfLambert (SurfaceOutput2 s, half3 lightDir, half3 viewDir, half atten)
	{
	
		half diff = max(0.0, dot(s.Normal, lightDir));
		fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;

		half3 h = normalize (lightDir + normalize(viewDir));
		half nh = max (0, dot(h, s.Normal));
		half specPower = s.Specular * _SpecularPower;
		half spec = pow(nh, s.Gloss * (_GlossPower * 128)) * specPower;

		fixed4 c;
		c.rgb = (s.Albedo * lerp(s.CustomColor * atten * diff,s.CustomColor * (ambient + 1),ambient) + spec) * _LightColor0.rgb * _Diffbrightness;
		c.a = spec;

		return c;
	}

	inline half4 LightingHalfLambertOmni (SurfaceOutput2 s, half3 lightDir, half3 viewDir, half atten)
	{
	
		half diff = max(0.0, dot(s.Normal, lightDir) * 0.5 + 0.5);

		half3 h = normalize (lightDir + normalize(viewDir));
		half nh = max (0, dot(h, s.Normal));
		half specPower = s.Specular * _SpecularPower;
		half spec = pow(nh, s.Gloss * (_GlossPower * 128)) * specPower;

		fixed4 c;
		c.rgb = (s.Albedo * _LightColor0.rgb * diff + spec * _LightColor0.rgb) * atten;   
		c.a = spec;

		return c;
	}

ENDCG


SubShader { 

	Tags { "RenderType"="Opaque" }
	LOD 300 

	Pass {
		Name "ContentBaseChar"
		Tags {"LightMode" = "ForwardBase"}


		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fwdbase
		#pragma fragmentoption ARB_precision_hint_fastest

   
		uniform sampler2D _Diffuse;
		uniform sampler2D _Normal;
		uniform sampler2D _LightMask;
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
			float3	lightDir : TEXCOORD1;
			float3	viewDir : TEXCOORD2;
			fixed3  customColor: TEXCOORD3;
			LIGHTING_COORDS(4,5)
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
			TANGENT_SPACE_ROTATION;
			float3 eyeDir = ObjSpaceViewDir(v.vertex);
			o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));
			o.viewDir = mul(rotation, eyeDir);

			TRANSFER_VERTEX_TO_FRAGMENT(o);
			return o;
		}

		float4 frag (v2f i) : COLOR
		{
			SurfaceOutput2 o;
			fixed4 texDiff = tex2D(_Diffuse,i.uv);
			fixed4 texNormal = tex2D(_Normal, i.uv);
			fixed3 lightMasks = tex2D(_LightMask, i.uv).rgb;
			o.Albedo = texDiff.rgb;
			o.Alpha = texDiff.a;
			o.Specular = lightMasks.r * _SpecularPower;
			o.Gloss = lightMasks.g;
			o.Normal =  UnpackNormal(texNormal);
			o.CustomColor =  i.customColor;
			half atten = LIGHT_ATTENUATION(i);

			half4 final = LightingHalfLambert(o,i.lightDir,i.viewDir,atten);

			return final;
		}


		ENDCG
	}


	Pass {
		Name "ContentAddChar"
		Tags {"LightMode" = "ForwardAdd"}
		Blend One One

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fwdadd
		#pragma fragmentoption ARB_precision_hint_fastest

	 
		uniform sampler2D  _FresnelColor;
		uniform sampler2D _Diffuse;
		uniform sampler2D _Normal;
		uniform sampler2D _LightMask;
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
			float3	lightDir : TEXCOORD1;
			float3	viewDir : TEXCOORD2;
			LIGHTING_COORDS(3,4)
		}; 


		v2f vert (appdata_full v) 
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = v.texcoord.xy;
			TANGENT_SPACE_ROTATION;
			float3 eyeDir = ObjSpaceViewDir(v.vertex);
			o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));
			o.viewDir = mul(rotation, eyeDir);

			TRANSFER_VERTEX_TO_FRAGMENT(o);
			return o;
		}


		float4 frag (v2f i) : COLOR
		{
			SurfaceOutput2 o;
			fixed4 texDiff = tex2D(_Diffuse,i.uv);
			fixed4 texNormal = tex2D(_Normal, i.uv);
			fixed3 lightMasks = tex2D(_LightMask, i.uv).rgb;
			o.Albedo = texDiff.rgb;
			o.Alpha = texDiff.a;
			o.Specular = lightMasks.r * _SpecularPower;
			o.Gloss = lightMasks.g;
			o.Normal =  UnpackNormal(texNormal);
			o.CustomColor =  float3(0,0,0);
			half atten = LIGHT_ATTENUATION(i);
			return LightingHalfLambertOmni(o,i.lightDir,i.viewDir,atten);
		}

		ENDCG
	}


}

FallBack "ShaderBox/Probe/___Low"
}

