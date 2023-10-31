Shader "Rus/MapTileHighlighted"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RadialTex ("RadialTexture", 2D) = "white" {}
        
        [HDR] _EmissionColor ("_EmissionColor", Color) = (1,1,1,1)
        _HighlightColor ("_HighlightColor", Color) = (1,0,0,1)

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
                SHADOW_COORDS(1)
                float2 uvRad : TEXCOORD2;
                
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _RadialTex;
            float4 _RadialTex_ST;
            
            uniform float4 _LightColor0; // color of light source (from "Lighting.cginc")
            float4 _EmissionColor;
            float4 _HighlightColor;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                TRANSFER_SHADOW(o);
                o.uvRad = TRANSFORM_TEX(v.uv, _RadialTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 mainColor = (float4(tex2D(_MainTex, i.uv)) * SHADOW_ATTENUATION(i) + _EmissionColor);
                float4 radial = tex2D(_RadialTex, i.uvRad);
                float fade = radial.x;
                float4 highlight = _HighlightColor;
                highlight *= highlight.a;
                // highlight = float4(1, 1, 1, 1);
                // return float4(i.uvRad.x, 0, 0, 1);
                return mainColor + (highlight * fade);
            }
            ENDCG
        }
    }
}
