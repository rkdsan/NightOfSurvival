Shader "Custom/ProtectiveEffect"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf NoLight noshadow alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
        };


        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex - _Time.x) * _Color;
            fixed4 d = tex2D(_MainTex, IN.uv_MainTex + _Time.x) * _Color;

            o.Albedo = c.rgb * d.rgb;
            o.Alpha = _Color.a;
        }

        float4 LightingNoLight(SurfaceOutput s, float3 lightDir, float atten) {
            float3 col = s.Albedo * (_LightColor0.rgb * 0.5 + 0.5);
            return float4(col, s.Alpha);
        }

        ENDCG
    }
    FallBack "Diffuse"
}
