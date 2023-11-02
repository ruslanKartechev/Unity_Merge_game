Shader "Rus/MapTile"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("_Color", Color) = (1,1,1,1)
        [HDR] _EmissionColor ("_EmissionColor", Color) = (1,1,1,1)
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
            float4 _EmissionColor;
            float4 _Color;
            
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
                float4 color = float4(tex2D(_MainTex, i.uv));
                color *= color * _Color;
                color *= SHADOW_ATTENUATION(i);
                color += _EmissionColor;
                return color;
            }
            ENDCG
        }
    }
}
