Shader "Rus/Map Tile Fade"
{
    Properties
    {
        _MainTex ("Texture 1", 2D) = "white" {}
        _MainTex2 ("Texture 2", 2D) = "white" {}
        _NoiseTexture ("NoiseTexture", 2D) = "white" {}
        
        _Fade("Fade", float) = 1
        _FadeThreshold("Fade Threshold", float) = 1
        _Color1("Color1", Color) = (1,1,1,1)
        _Color2("Color2", Color) = (1,1,1,1)
        
        [HDR] _EmissionColor1 ("EmissionColor1", Color) = (1,1,1,1)
        [HDR] _EmissionColor2 ("EmissionColor2", Color) = (1,1,1,1)
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uvNoise : TEXCOORD1;
                SHADOW_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _MainTex2;
            float4 _MainTex2_ST;
            sampler2D _NoiseTexture;
            float4 _NoiseTexture_ST;

            float4 _EmissionColor1;
            float4 _EmissionColor2;
            float4 _Color1;
            float4 _Color2;
            float _FadeThreshold;
            float _Fade;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvNoise = TRANSFORM_TEX(v.uv, _NoiseTexture);
                TRANSFER_SHADOW(o);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 color1 = float4(tex2D(_MainTex, i.uv));
                color1 *= color1 * _Color1;
                color1 += _EmissionColor1;

                float4 color2 = float4(tex2D(_MainTex2, i.uv));
                color2 *= color2 * _Color2;
                color2 += _EmissionColor2;

                float noise = step(_FadeThreshold, tex2D(_NoiseTexture, i.uvNoise).x);
                float fade = saturate(noise + _Fade);
                // fade = step(_FadeThreshold, fade);
                return (color1 * (1 - fade) + color2 * (fade) ) * SHADOW_ATTENUATION(i);;
            }
            ENDCG
        }
    }
}
