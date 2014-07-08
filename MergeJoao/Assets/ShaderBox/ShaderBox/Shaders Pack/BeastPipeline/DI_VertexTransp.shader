Shader "ShaderBox/Soft Specular Transparent/Self-Illumin/___Low" 
{

Properties 
{
	_LightMap  ("Brightness Light Map ", Float) = 0.4
	_SpecColor ("Specular Color for Light Map", Color ) = (0.5, 0.5, 0.5, 1)
	_Shininess ("Shininess for Light Map", Range(0.03, 2)) = 1.0
	_EmissionLM ("Emission for Light Map", Float) = 10
	_Color("Diffuse Color ", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Diffuse (RGB)  Refl Mask (A)", 2D) = "white" {}
	_Illum ("Illumination (A)", 2D) = "white" {}
	_TransparencyLM ("Transparency Color for Light Map",  2D) = "white" {}
	_ReflectPower("Reflection Power", Float) = 1.0
	_ReflectBrightness("Reflection Brightness", Float) = 1.0
	_ReflectionCube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
}

SubShader {
	Tags{ "Queue"="Transparent" "RenderType"="Transparent" }
	LOD 100 

	Pass {
		Name "ForwardBase"
		Tags {"LightMode" = "ForwardBase"}
		Blend SrcAlpha OneMinusSrcAlpha 
		ColorMask RGB
			
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#pragma fragmentoption ARB_precision_hint_fastest
				
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
				
			struct v2f
			{
				float4	pos : SV_POSITION;
				float2	uv : TEXCOORD0;
				float3 lightmapUV : TEXCOORD1;
				float light : TEXCOORD2;
				float3	worldRefl : TEXCOORD3;
				LIGHTING_COORDS(4,5)
			}; 

			#ifndef LIGHTMAP_OFF
				float4 unity_LightmapST;
			#endif

			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord.xy;
				half3 lightDir =  ObjSpaceLightDir(v.vertex);

				#ifndef USING_DIRECTIONAL_LIGHT
					lightDir = normalize(lightDir);
				#endif
				
				half3 eyeDir = ObjSpaceViewDir(v.vertex);
				float3 refl = mul((float3x3)_Object2World,reflect(normalize(-eyeDir),v.normal));
				o.worldRefl = float3(refl.x,refl.y,refl.z);

				#ifndef LIGHTMAP_OFF
					o.lightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					o.light = max (0.0,dot(v.normal, lightDir) * 0.5 + 1);
				#else
					o.light = max (0.0,dot(v.normal, lightDir));
				#endif

				o.lightmapUV.z = v.color.a;
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _Illum;
			samplerCUBE _ReflectionCube;
			
			half4 _Color;
			fixed4 _LightColor0;
			
			#ifndef LIGHTMAP_OFF
				sampler2D unity_Lightmap;
				sampler2D unity_LightmapInd;
			#endif

			half _ReflectPower;
			half _ReflectBrightness;
			half _LightMap;

			float4 frag(v2f i) : COLOR
			{
				fixed4 diff = tex2D(_MainTex, i.uv);
				fixed illumin = tex2D(_Illum, i.uv).a;
				half atten = LIGHT_ATTENUATION(i);
				
				fixed3 Emission = texCUBE(_ReflectionCube, i.worldRefl);
				Emission.rgb *= _ReflectBrightness;

				fixed4 result;

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;

				#ifdef LIGHTMAP_ON
					fixed4 lightMap = tex2D (unity_Lightmap, i.lightmapUV.xy);
					fixed4 lightMapInd = tex2D (unity_LightmapInd, i.lightmapUV.xy);
					lightMap.rgb = DecodeLightmap(lightMap);
					lightMapInd.rgb = DecodeLightmap(lightMapInd);

					half3 lm = lightMap.rgb  * _LightMap; 
					fixed3 shadow = max(min(lm,(atten * 2) * 0.8), lightMapInd.rgb);
					result.rgb = diff.rgb  * shadow * i.light * _LightColor0 * _Color.rgb;
				#else
					result.rgb = diff.rgb * lerp((atten * 2) * i.light,ambient + 1,ambient ) * _LightColor0.rgb * _Color.rgb;
				#endif
				
				result.rgb = lerp(result.rgb ,Emission, diff.a * _ReflectPower);
				result.rgb = result.rgb + diff.rgb * illumin;
				result.a = _Color.a * i.lightmapUV.z;
					 
				return result;
			}

		ENDCG
	}

	Pass {
		Name "ForwardAdd"
		Tags {"LightMode" = "ForwardAdd"}
		Blend One One
			
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdadd
			#pragma fragmentoption ARB_precision_hint_fastest
				
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
				
			struct v2f
			{
				float4	pos : SV_POSITION;
				float2	uv : TEXCOORD0;
				float light : TEXCOORD1;
				float alpha : TEXCOORD2;
				LIGHTING_COORDS(3,4)
			}; 

			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord.xy;
				half3 lightDir =  ObjSpaceLightDir(v.vertex);
				o.light = max (0.0,dot(v.normal, lightDir));
				o.alpha = v.color.a;
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
				
			sampler2D _MainTex;
			half4 _Color;
			fixed4 _LightColor0;

			float4 frag(v2f i) : COLOR
			{
				fixed4 diff = tex2D(_MainTex, i.uv);
				half atten = LIGHT_ATTENUATION(i);
				diff.rgb *= i.alpha;
				fixed4 result;
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
				result.rgb = diff.rgb * lerp((atten * 2) * i.light,ambient + 1,ambient ) * _Color.rgb * _LightColor0;
					 
				result.a = 0;

				return result;
			}
		ENDCG
	}
}


	Fallback "Hidden/ShaderBox/Self-Illumin/Transparent/VertexLit"
}
