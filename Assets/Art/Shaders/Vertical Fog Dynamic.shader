Shader "Rus/Vertical Dynamic Fog URP"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1, 1, 1, .5) 
        _ColorUp("Color On Intersection", Color) = (1, 1, 1, .5)
        _ColorNoiseBlend("Color blended with main color", Color) = (1, 1, 1, .5)
        
        _FogDepth("Fog Depth", Range(0,1)) = 0.15
        _Power("Diff factor power",  Range(0,2)) = 0.38
        _NoiseTexture("Noise Texture", 2D) = "" {}
        _ScrollSpeed("Scroll speed", float) = 0
        _NoiseWaveSpeed("Noise Wave Speed", float) = 50
        _WaveAmplitudeMin("Wave Amplitude Min", Range(0,1)) = 0
        _WaveAmplitudeMax("Wave Amplitude Max", Range(0,1)) = 1
        _ColorNoiseBlendFactor("Color Noise Blend Factor", Range(0,1)) = 0.5
        _AlphaSubMax("Alpha sub max value", Range(0,1)) = 0.5
        
        [Header(Frequencies)]
        _omega1("Frequency 1", Float) = 10
        _omega2("Frequency 2", Float) = 12
        _omega3("Frequency 3", Float) = 14
        [Header(Amplitudes)]
        _WaveWeight1("Wave weight 1", Float) = 1
        _WaveWeight2("Wave weight 1", Float) = 1
        _WaveWeight3("Wave weight 1", Float) = 1
        [Header(Directions)]
        _Dir1("Wave Dir 1", Vector) = (1,1,0,0)
        _Dir2("Wave Dir 2", Vector) = (1,1,0,0)
        _Dir3("Wave Dir 3", Vector) = (1,1,0,0)
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
            ZWrite On
            HLSLPROGRAM
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
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
                float4 clipPos : SV_POSITION;
            };
  
            sampler2D _CameraDepthTexture;
            float4 _MainColor;
            float4 _ColorUp;
            float4 _ColorNoiseBlend;
            float4 _IntersectionColor;
            float _FogDepth;
            float _Power;
            float _ScrollSpeed;
            float _ColorNoiseBlendFactor;
            float _AlphaSubMax;
            
            sampler2D _NoiseTexture;
            float4 _NoiseTexture_ST;
            float _NoiseWaveSpeed;
            float _WaveAmplitudeMin;
            float _WaveAmplitudeMax;
            // Waves' frequencies
            float _omega1;
            float _omega2;
            float _omega3;
            // Waves' Weights (Amplitudes)
            float _WaveWeight1;
            float _WaveWeight2;
            float _WaveWeight3;
            // Waves' Propagation Directions
            float2 _Dir1;
            float2 _Dir2;
            float2 _Dir3;
           
            v2f vert(appdata input)
            {
                v2f o;
                o.clipPos = UnityObjectToClipPos(input.vertex);
                o.worldPos = mul(unity_ObjectToWorld, input.vertex);
                o.screenPos = ComputeNonStereoScreenPos(o.clipPos);
                float2 uv = TRANSFORM_TEX(input.uv, _NoiseTexture);
                uv.x = (input.uv + _Time * _ScrollSpeed) % 10;
                o.uv = uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;   
            }

            inline float Remap(float input, float from1, float to1, float from2, float to2)
            {
                return (input - from1) / (to1 - from1) * (to2 - from2) + from2;
            }   
  
            float4 frag(v2f i) : SV_TARGET
            {
                float noise_color = tex2D(_NoiseTexture, float2(i.uv.x, i.uv.y)).x;
                float wave_value = _WaveWeight1 * sin(_omega1 * (i.uv.x * _Dir1.x + i.uv.y * _Dir1.y ) * UNITY_PI + _Time * _NoiseWaveSpeed)
                                 + _WaveWeight2 * sin(_omega2 * (i.uv.x * _Dir2.x + i.uv.y * _Dir2.y ) * UNITY_PI + _Time * _NoiseWaveSpeed)
                                 + _WaveWeight3 * sin(_omega3 * (i.uv.x * _Dir3.x + i.uv.y * _Dir3.y ) * UNITY_PI + _Time * _NoiseWaveSpeed);

                float weights_sum = _WaveWeight1 + _WaveWeight2 + _WaveWeight3;
                wave_value = Remap(wave_value, -(weights_sum), weights_sum, _WaveAmplitudeMin, _WaveAmplitudeMax);
                float noise_color_factor = noise_color * wave_value;
                float main_color_blend_t = saturate(Remap(noise_color_factor, 0, _WaveAmplitudeMax, 0, _ColorNoiseBlendFactor));
                
                float depth = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
                // The difference  (depth - i.screenPos.w) is 1 where there is nothing, 0 where there is an opaque object
                float depth_diff = saturate(_FogDepth * (depth - i.screenPos.w));
                float alpha_lerp_t = pow(depth_diff, _Power);
                float lerp_noise_color = lerp(noise_color_factor, 0, depth - i.screenPos.w);
                float color_a = saturate(lerp(0, _MainColor.a, alpha_lerp_t) - lerp_noise_color - _AlphaSubMax);
                
                float3 vertical_blend_color = lerp(_ColorUp.xyz, _MainColor.xyz, alpha_lerp_t);
                float3 blend_main_color = lerp(vertical_blend_color.rgb, _ColorNoiseBlend, main_color_blend_t);
                float4 final_color = float4(blend_main_color, color_a);
                // noise_color_factor = saturate(noise_color_factor);
                // final_color = float4(noise_color_factor,noise_color_factor,noise_color_factor,1);
                return final_color;   
            }
            ENDHLSL
        }
    }
}