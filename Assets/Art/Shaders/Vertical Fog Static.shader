Shader "Rus/Vertical Static Fog URP"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1, 1, 1, .5)
        _ColorUp("Color On Intersection", Color) = (1, 1, 1, .5)
        _DepthDifferenceFactor("Depth Difference Factor", Float) = 0.15
        _Power("Diff factor power", Float) = 0.38
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" 
            "RenderType"="Transparent"  }
        
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
           };
  
            struct v2f
            {
                float4 scrPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
  
           sampler2D _CameraDepthTexture;
           float4 _MainColor;
           float4 _ColorUp;
           float4 _IntersectionColor;
           float _DepthDifferenceFactor;
           float _Power;
           
            v2f vert(appdata input)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(input.vertex);
                o.scrPos = ComputeScreenPos(o.vertex);
                return o;   
            }
  
            half4 frag(v2f i) : SV_TARGET
            {
                float depth = LinearEyeDepth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)));
                float diff = saturate(_DepthDifferenceFactor * (depth - i.scrPos.w));
                float lerpT = pow(diff, _Power);
                float finalAlpha = lerp(0.0, _MainColor.a, lerpT);
                float4 colorBlended = lerp(_ColorUp, _MainColor, lerpT);
                fixed4 finalColor = fixed4(colorBlended.rgb, finalAlpha);
                return finalColor;
            }
            ENDCG
        }
    }
}