// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/AreaHighlight" {
	Properties {
		_Color1 ("Color Speed", Color) = (1,1,1,1)
		_Color2 ("Color Range", Color) = (1,1,1,1)
		_Colori ("Color Index", int ) = 0
		_Radius("Radius", float) = 1.0
	}
SubShader
{
	Tags
{
	"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"
}

Blend SrcAlpha OneMinusSrcAlpha

Pass
{

	CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

float4 _MainTex_ST;

float4 _Color1;
float4 _Color2;
float _Radius;
int _Colori;
float4 _Colors[2]; 

struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	fixed4 color : COLOR;
};

struct v2f
{
	float4 vertex : SV_POSITION;
	float2 uv : TEXCOORD0;
	fixed4 color : COLOR;
};

v2f vert(appdata v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.uv = TRANSFORM_TEX(v.uv, _MainTex);
	o.color = v.color;
	o.uv = v.uv;
	return o;
}


float4 frag(v2f i) : SV_Target
{
	_Colors[0] = _Color1;
	_Colors[1] = _Color2;
	float dist = abs(length(i.uv - float2(0.5,0.5)));

	if (dist < _Radius)
		return _Colors[_Colori];
	else
		return float4(0, 0, 0, 0);
}

ENDCG
}
}
}
