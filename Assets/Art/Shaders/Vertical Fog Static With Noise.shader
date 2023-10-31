Shader "Rus/Vertical Static Fog Noise URP"
{
      Properties
    {
        _MainColor("Main Color", Color) = (1, 1, 1, .5) 
        _SecondaryColor("Second Color", Color) = (1, 1, 1, .5) 
        _BrightnessBias("Brightness Bias", float) = 0
        
        _NoiseTexture("Noise Texture 1", 2D) = "" {}
        _NoiseTexture2("Noise Texture 2", 2D) = "" {}
        _NoiseTexture3("Noise Texture 3", 2D) = "" {}
        
        _Weight1("Weight 1", float) = 0
        _Weight2("Weight 2", float) = 0
        _Weight3("Weight 3", float) = 0
        _ScrollSpeed1("Scroll speed 1", float) = (0,0,0,0)
        _ScrollSpeed2("Scroll speed 2", float) = (0,0,0,0)
        _ScrollSpeed3("Scroll speed 3", float) = (0,0,0,0)
        
        _RotSpeed("Rot speed", float) = 0
        _BlendSpeed("BlendSpeed", float) = 0
        
        _AddedAlpha("Added Alpha", float) = 0
        _AlphaThreshold("Threshold Alpha", float) = 0
        _UVScale("UVScale", float) = 100
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
                float4 uv1 : TEXCOORD1;
                float4 uv2 : TEXCOORD2;
                 
                float4 data : TEXCOORD4; // blend amount
            };

           float2 _ActorPosition;
           sampler2D _CameraDepthTexture;
           float4 _MainColor;
           float4 _SecondaryColor;
           float _BrightnessBias;
           
           float _RotSpeed;
           float _BlendSpeed;
           float _AddedAlpha;
           float _AlphaThreshold;
           float _UVScale;

           float _Weight1;
           float _Weight2;
           float _Weight3;
           
           float2 _ScrollSpeed1;
           float2 _ScrollSpeed2;
           float2 _ScrollSpeed3;
           
           sampler2D _NoiseTexture;
           float4 _NoiseTexture_ST;
           sampler2D _NoiseTexture2;
           float4 _NoiseTexture2_ST;
           sampler2D _NoiseTexture3;
           float4 _NoiseTexture3_ST;

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
               float4 worldPos = mul(unity_ObjectToWorld, input.vertex);
               float2 world_uv = float2(worldPos.x, worldPos.z) / _UVScale;
               float time = _Time.x;
               float2 uv1 = float2(world_uv.x + (time * _ScrollSpeed1.x) % 10,
                   world_uv.y + (time * _ScrollSpeed1.y) % 10);
                float2 uv2 = float2(world_uv.x + (time * _ScrollSpeed2.x) % 10,
                   world_uv.y + (time * _ScrollSpeed2.y) % 10);
                float2 uv3 = float2(world_uv.x + (time * _ScrollSpeed3.x) % 10,
                   world_uv.y + (time * _ScrollSpeed3.y) % 10);
               
               o.uv1.xy = TRANSFORM_TEX(uv1, _NoiseTexture);
               o.uv1.zw = TRANSFORM_TEX(uv2, _NoiseTexture2);
               o.uv2.xy = TRANSFORM_TEX(uv3, _NoiseTexture3);
               
               o.data.x = sin((_Time * _BlendSpeed) % (2 * UNITY_PI));
               return o;
               
            }
  
            float4 frag(v2f i) : SV_TARGET
            {
                half sum1 =  tex2D(_NoiseTexture, i.uv1.xy).x;
                half sum2 = tex2D(_NoiseTexture2, i.uv1.zw).x;
                half sum3 = tex2D(_NoiseTexture3, i.uv2.xy).x;
                float4 color = saturate((_BrightnessBias + _Weight1 * sum1 + _Weight2 * sum2 + _Weight3 * sum3) * _MainColor);
                half alpha = saturate(color.a + _AddedAlpha - _BrightnessBias);
                color.xyz *= alpha;
                alpha = smoothstep(_AlphaThreshold, 1, alpha);
                color.a = alpha;
                return color;
            }
           
            ENDCG
        }
    }
}