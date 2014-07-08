#ifndef ADV_LIGHTING_INCLUDED
#define ADV_LIGHTING_INCLUDED

struct LightOutput 
{
	half3 Albedo;
	half3 Normal;
	half Specular;
};
	
fixed4 _LightColor0;
fixed4 _SpecColor;
half _SpecularPower;
half _GlossPower;
fixed4 _Color;


inline fixed4 HalfBlingPhongAdvenced (LightOutput s, half3 lightDir, half3 viewDir, fixed3 shadow) 
{
	#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
	#endif
	
	fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
	
	#ifdef LIGHTMAP_ON
		fixed NdotL = max (0.0,dot(s.Normal, lightDir) * 0.5 + 1);
	#else
		fixed NdotL = max (0.0,dot(s.Normal, lightDir));
	#endif
	
	half3 h = normalize (lightDir + viewDir);
	half nh = max (0, dot(h, s.Normal));
	half specPower = (s.Specular * shadow) * _SpecularPower;
	half spec = pow(nh, _GlossPower * 128) * specPower;
					
	fixed4 result;

	#ifdef LIGHTMAP_ON
		fixed3 diff = s.Albedo * shadow * NdotL * _LightColor0.rgb * _Color.rgb; 
	#else
		fixed3 diff = s.Albedo * lerp(shadow * NdotL,ambient + 1,ambient) * _LightColor0.rgb * _Color.rgb;
	#endif

	result.rgb = (diff + spec);
	result.a = spec;
	return result;
}




#endif
