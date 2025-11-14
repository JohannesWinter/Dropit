Shader "Unlit/DarknessMask"
{
    Properties
    {
        _MousePos("Mouse Position", Vector) = (0.5, 0.5, 0, 0)
        _Radius("Radius", Float) = 0.05
        _AlphaRadius("AlphaRadius", Float) = 0.05
        _Color("Color", Color) = (0, 0, 0, 1)
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Overlay" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _MousePos;
            float _Radius;
            float _AlphaRadius;
            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 delta = i.uv - _MousePos.xy;
                float aspect = _ScreenParams.x / _ScreenParams.y;
                delta.x *= aspect;

                float dist = length(delta);

                float alpha = smoothstep(_Radius, _Radius + _AlphaRadius, dist);

                return fixed4(_Color.rgb, alpha * _Color.a);
            }
            ENDCG
        }
    }
}
