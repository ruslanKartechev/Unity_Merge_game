Shader "Rus/Vertical Static Fog Noise URP"
{
      Properties
    {
        _MainColor("Main Color", Color) = (1, 1, 1, .5) 
        _SecondaryColor("Second Color", Color) = (1, 1, 1, .5) 
        _NoiseTexture("Noise Texture 1", 2D) = "" {}
        _NoiseTexture2("Noise Texture 2", 2D) = "" {}
        _RotSpeed("Rot speed", float) = 0
        _ScrollSpeed("Scroll speed", float) = 0
        _BlendSpeed("BlendSpeed", float) = 0
        _MinAlpha("MinAlpha", float) = 0
        _ActorPosition("Test _ActorPosition", float) = (1, 1,1,1)
    }
    SubShader
    {
        Tags 
        { 
            "Queue" = "Transparent" 
            "RenderType"="Transparent"
        }
        
        Pass
        {
           Blend SrcAlpha OneMinusSrcAlpha
           CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
           #include "UnityCG.cginc"
  
           struct appdata
           {
               float4 vertex : POSITION;
               float2 uv : TEXCOORD0;
           };
  
            struct v2f
            {
                float4 scrPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                 
                float4 data : TEXCOORD4; // blend amount
            };

           float2 _ActorPosition;
           sampler2D _CameraDepthTexture;
           float4 _MainColor;
           float4 _SecondaryColor;
           float _ScrollSpeed;
           float _RotSpeed;
           float _BlendSpeed;
           float _MinAlpha;
           
           sampler2D _NoiseTexture;
           float4 _NoiseTexture_ST;
           sampler2D _NoiseTexture2;
           float4 _NoiseTexture2_ST;

           void RotateRadians(half2 UV, half2 Center, half Rotation, out half2 Out)
           {
                //rotation matrix
                UV -= Center;
                half s = sin(Rotation);
                half c = cos(Rotation);

                //center rotation matrix
                half2x2 rMatrix = half2x2(c, -s, s, c);
                rMatrix *= 0.5;
                rMatrix += 0.5;
                rMatrix = rMatrix*2 - 1;

                //multiply the UVs by the rotation matrix
                UV.xy = mul(UV.xy, rMatrix);
                UV += Center;

                Out = UV;
           }
    
            inline float Remap(float input, float from1, float to1, float from2, float to2)
            {
                return (input - from1) / (to1 - from1) * (to2 - from2) + from2;
            }   

            v2f vert(appdata input)
            {
               v2f o;
               o.vertex = UnityObjectToClipPos(input.vertex);
               o.scrPos = ComputeScreenPos(o.vertex);
               float2 iuv = float2(o.scrPos.x , o.scrPos.y ) / 100;
               
               float2 uv = TRANSFORM_TEX(iuv, _NoiseTexture);
               float2 uv2 = TRANSFORM_TEX(iuv, _NoiseTexture2);
               half scroll = (_Time * _ScrollSpeed) % 10;
               half rot = (_Time * _RotSpeed) % 10;
                
               uv.x += scroll;
               uv2.x += scroll;
               
               RotateRadians(uv, half2(.5, .5), rot, uv);
               RotateRadians(uv2, half2(.5, .5), rot, uv2);
               o.uv1 = uv;
               o.uv2 = uv2;
               o.data.x = sin((_Time * _BlendSpeed) % (2 * UNITY_PI));
               return o;
               
            }
  
            float4 frag(v2f i) : SV_TARGET
            {
                float4 col1 = tex2D(_NoiseTexture, i.uv1);
                float4 col2 = tex2D(_NoiseTexture2, i.uv2);
                half sum1 =  (col1.x + col1.y + col1.z) / 3.0;
                half sum2 =  (col2.x + col2.y + col2.z) / 3.0;
                
                col1 = sum1 * _MainColor;
                col2 = sum2 * _SecondaryColor;
                
                half noiseBlendLerp = i.data.x;
                // noiseBlendLerp = 0;
                float4 color = saturate(lerp(col1, col2, noiseBlendLerp));
                // float4 color = saturate(col1 + lerp(col1, float4(0,0,0,0), noiseBlendLerp));
                half alpha = saturate(color.a + _MinAlpha);
                // float distance = length(i.scrPos.xyzw - _ActorPosition);
                // if(distance < 100)
                    // alpha = 0;
                color.a = alpha;
                // if(color.a < _MinAlpha)
                    // color.a = _MinAlpha;
                return color;
            }
           
           


           
            ENDCG
        }
    }
}