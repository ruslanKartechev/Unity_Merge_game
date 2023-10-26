Shader "Rus/MapTile"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _EmissionColor ("_EmissionColor", Color) = (1,1,1,1)
        _AmbientContribution ("_AmbientContribution", float) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                SHADOW_COORDS(1)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float4 _LightColor0; // color of light source (from "Lighting.cginc")
            float4 _EmissionColor;
            float _AmbientContribution;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                TRANSFER_SHADOW(o);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 color =  (float4(tex2D(_MainTex, i.uv)) * SHADOW_ATTENUATION(i) + _EmissionColor);
                return color;
            }
            ENDCG
        }
    }
}
