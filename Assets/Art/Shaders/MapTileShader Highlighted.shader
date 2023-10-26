Shader "Rus/MapTileHighlighted"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _EmissionColor ("_EmissionColor", Color) = (1,1,1,1)
        _AmbientContribution ("_AmbientContribution", float) = 1
        _HighlightColor ("_HighlightColor", Color) = (1,0,0,1)
        _FresnelPower ("_FresnelPower", Float) = 1
        _FresnelScale ("_FresnelScale", Float) = 1
        _FresnelBias ("_FresnelBias", Float) = 1
        

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
                float fresnel : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float4 _LightColor0; // color of light source (from "Lighting.cginc")
            float4 _EmissionColor;
            float _AmbientContribution;
            float4 _HighlightColor;
            float _FresnelPower;
            float _FresnelScale;
            float _FresnelBias;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                // o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                TRANSFER_SHADOW(o);
                float i = normalize(ObjSpaceViewDir(v.vertex));
                o.fresnel = _FresnelBias + _FresnelScale * pow(1 + dot(i, v.normal), _FresnelPower);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 fresnel = _HighlightColor * i.fresnel;
                float4 color =  (float4(tex2D(_MainTex, i.uv)) * SHADOW_ATTENUATION(i) + _EmissionColor) * fresnel;
                float u = i.uv.x;
                float v = i.uv.y;
                
                color = float4(1, 1, 1, 1) * (u );
                return color;
            }
            ENDCG
        }
    }
}
