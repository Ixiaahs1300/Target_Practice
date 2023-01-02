Shader "Unlit/Lava"
{
    Properties
    {
        _MainTex ("Base Texture (RGB)", 2D) = "white" {}
        _GlowTex ("Glow Texture (RGB)", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1,0,0,1)
        _GlowIntensity ("Glow Intensity", Range(0, 10)) = 5
        _GlowSpeed ("Glow Speed", Range(0, 1)) = 0.5
        _GlowScale ("Glow Scale", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _GlowTex;
        float4 _GlowColor;
        float _GlowIntensity;
        float _GlowSpeed;
        float _GlowScale;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_GlowTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Sample the base texture and apply it to the surface
            float4 baseColor = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = baseColor.rgb;
            o.Alpha = baseColor.a;

            // Sample the glow texture and apply it to the surface
            float4 glowColor = tex2D(_GlowTex, IN.uv_GlowTex);
            o.Emission = _GlowColor * _GlowIntensity * glowColor.rgb;
        }

        ENDCG
    }

    Fallback "Diffuse"
}
