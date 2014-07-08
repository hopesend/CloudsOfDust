Shader "ShaderBox/Soft Specular Transparent/Self-Illumin/_High" {

Properties 
{
	_LightMap  ("Brightness Light Map ", Float) = 0.4
	_SpecColor ("Specular Color for Light Map", Color ) = (0.5, 0.5, 0.5, 1)
	_Shininess ("Shininess for Light Map", Range(0.03, 2)) = 1.0
	_EmissionLM ("Emission for Light Map", Float) = 10
	_Color("Diffuse Color", Color) = (0.5,0.5,0.5,0.5)
	_SpecularPower("Specular", Float) = 1.0
	_GlossPower("Gloss", Float) = 1.0
	_ReflectPower("Reflection Power", Float) = 1.0
	_ReflectBrightness("Reflection Brightness", Float) = 1.0
	_UVDetailTile("UV Detail Tile", Float) = 1.0
	_DetailFactor("Detail Factor", Range (0, 1)) = 1.0
	_UVDetailNormalTile("UV Detail Normal Tile", Float) = 1.0
	_MainTex ("Diffuse (RGB) Specular (A)", 2D) = "white" {}
	_NormalTex ("Normal (RGB)", 2D) = "bump" {}
	_ReflectionCube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
	_Illum ("Illumination (A)", 2D) = "white" {}
	_Detail ("Detail map", 2D) = "white" {}
	_DetailNormal ("Detail Normal map", 2D) = "white" {}
	_RimLightColor("Rim Light Color", Color) = (0.5,0.5,0.5,0.5)
	_RimLightRamp ("Rim Light", 2D) = "white" {}
	_TransparencyLM ("Transparency Color for Light Map",  2D) = "white" {}
}



SubShader {
	Tags{ "Queue"="Transparent"  "RenderType"="Transparent" }
	LOD 400
	Pass 
	{
		Name "ForwardBase" 
		Tags {"LightMode" = "ForwardBase" }
		Blend SrcAlpha OneMinusSrcAlpha 
		ColorMask RGB
	
		CGPROGRAM
			#pragma exclude_renderers ps3 xbox360 gles
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#pragma fragmentoption ARB_precision_hint_fastest
				
	
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "HLSLSupport.cginc"
			#include "AdvLighting.cginc"
				
			struct v2f
			{
				float4	pos : SV_POSITION;
				float2	uv : TEXCOORD0;
				float3	lightDirT : TEXCOORD1;
				float3	viewDirT : TEXCOORD2;
				float3	worldRefl : TEXCOORD3;
				float3 	lightmapUV : TEXCOORD4;
				LIGHTING_COORDS(5,6)
			}; 

			#ifndef LIGHTMAP_OFF
				float4 unity_LightmapST;
			#endif

			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord.xy;
				TANGENT_SPACE_ROTATION;
				half3 eyeDir = ObjSpaceViewDir(v.vertex);
				o.lightDirT = mul(rotation, ObjSpaceLightDir(v.vertex));
				o.viewDirT = mul(rotation, eyeDir);
				float3 refl = mul((float3x3)_Object2World,reflect(normalize(eyeDir),v.normal));
				o.worldRefl = float3(-refl.x,-refl.y,refl.z);

				#ifndef LIGHTMAP_OFF
					o.lightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				o.lightmapUV.z = v.color.a;

				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
				
			sampler2D _MainTex;
			sampler2D _NormalTex;
			sampler2D _Detail;
			sampler2D _DetailNormal;
			samplerCUBE _ReflectionCube;
			sampler2D _RimLightRamp;
			sampler2D _Illum;
			
			#ifndef LIGHTMAP_OFF
				sampler2D unity_Lightmap;
				sampler2D unity_LightmapInd;
			#endif

			half _ReflectPower;
			half _ReflectBrightness;
			half _UVDetailTile;
			half _DetailFactor;
			half _LightMap;
			half _UVDetailNormalTile;
			half3 _RimLightColor;

			float4 frag(v2f i) : COLOR
			{

				LightOutput o;
				fixed4 diff = tex2D(_MainTex, i.uv);
				fixed illumin = tex2D(_Illum, i.uv).a;
				fixed3 detail = tex2D(_Detail, i.uv * _UVDetailTile).rgb;
				diff.rgb = lerp(detail.rgb *  diff.rgb ,  diff.rgb, _DetailFactor);
				o.Albedo = diff.rgb;
				fixed4 normal = tex2D(_NormalTex, i.uv);
				fixed4 detailNormal =  tex2D(_DetailNormal, i.uv * _UVDetailNormalTile);
				o.Normal = UnpackNormal(normal);
				o.Normal.xy += UnpackNormal(detailNormal).rg;
				o.Normal = normalize(o.Normal);
				
				half3 vReflection = reflect(i.worldRefl,o.Normal);
				fixed3 Emission = texCUBE(_ReflectionCube, vReflection).rgb;
				Emission *= _ReflectBrightness;
				o.Specular =  diff.a;
				half atten = LIGHT_ATTENUATION(i);

				i.viewDirT = normalize(i.viewDirT);

				half3 shadowRT = (atten * 2);

				#ifdef LIGHTMAP_ON
					fixed4 lightMap = tex2D (unity_Lightmap, i.lightmapUV.xy);
					fixed4 lightMapInd = tex2D (unity_LightmapInd, i.lightmapUV.xy);
					lightMap.rgb = DecodeLightmap(lightMap);
					lightMapInd.rgb = DecodeLightmap(lightMapInd);

					fixed3 lm = lightMap.rgb * _LightMap;
					fixed3 shadow = max(min(lm,shadowRT  * 0.8 ), lightMapInd.rgb);
					fixed4 light = HalfBlingPhongAdvenced(o, i.lightDirT, i.viewDirT, shadow);
				#else
					fixed4 light = HalfBlingPhongAdvenced(o, i.lightDirT, i.viewDirT, shadowRT);
				#endif
					
				fixed4 result;
				result.rgb = lerp(light.rgb ,Emission.rgb, diff.a * _ReflectPower);
				result.a = _Color.a * i.lightmapUV.z;

				fixed rampSample = dot(o.Normal, i.viewDirT);
				fixed intensity = tex2D(_RimLightRamp, rampSample.xx).r;
				result.rgb += intensity * _RimLightColor.rgb;
				result.rgb = result.rgb + diff.rgb * illumin;
				
				return result;
			}

		ENDCG
	}


	Pass {
		Name "ForwardAdd"
		Tags {"LightMode" = "ForwardAdd"}
		Blend One One
			
		CGPROGRAM
			#pragma exclude_renderers ps3 xbox360 gles
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdadd
			#pragma fragmentoption ARB_precision_hint_fastest
				
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "AdvLighting.cginc"
				
			struct v2f
			{
				float4	pos : SV_POSITION;
				float2	uv : TEXCOORD0;
				float3	lightDirT : TEXCOORD1;
				float3	viewDirT : TEXCOORD2;
				float   alpha : TEXCOORD3;
				LIGHTING_COORDS(4,5)
			}; 

			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord.xy;
				TANGENT_SPACE_ROTATION;
				half3 eyeDir = ObjSpaceViewDir(v.vertex);
				o.lightDirT = mul(rotation, ObjSpaceLightDir(v.vertex));
				o.viewDirT = mul(rotation, eyeDir);
				o.alpha = v.color.a;
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
				
			sampler2D _MainTex;
			sampler2D _NormalTex;
			sampler2D _DetailNormal;
			samplerCUBE _ReflectionCube;
			half _UVDetailNormalTile;
			sampler2D _Sprecular;

			float4 frag(v2f i) : COLOR
			{
				LightOutput o;
				fixed4 diff = tex2D(_MainTex, i.uv);
				diff.rgb *= i.alpha;
				o.Albedo = diff.rgb;
				fixed specular = tex2D(_Sprecular, i.uv).r;
				fixed4 normal = tex2D(_NormalTex, i.uv);
				fixed4 detailNormal =  tex2D(_DetailNormal, i.uv * _UVDetailNormalTile);
				o.Normal = UnpackNormal(normal);
				o.Normal.xy += UnpackNormal(detailNormal).rg;
				o.Normal = normalize(o.Normal);
								
				o.Specular = specular;
				half atten = LIGHT_ATTENUATION(i);
				fixed4 light = HalfBlingPhongAdvenced(o, i.lightDirT, i.viewDirT,fixed3(1,1,1));
				fixed4 result = light * atten;
				result.a = 0;
				return result;
			}
		ENDCG
	}


}

Fallback "ShaderBox/Soft Specular Transparent/Self-Illumin/__Middle"
}
