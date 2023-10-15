Shader "FollowedTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Toggle(FOLLOW)]_Follow ("Follow", Int) = 1
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile FOLLOW __

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
            CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                #ifdef FOLLOW
                    float3 viewer = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1)).xyz;
                    float3 upDir = float3(0, 1, 0);
                    float3 normalDir = normalize(float3(viewer.x, 0, viewer.z));
                    float3 rightDir = normalize(cross(normalDir, upDir));
                    float3 localPos = rightDir * v.vertex.x + upDir * v.vertex.y + normalDir * v.vertex.z;
                    o.vertex = TransformObjectToHClip(localPos);
                #else
                    o.vertex = TransformObjectToHClip(v.vertex.xyz);
                #endif
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                clip(col.a - 0.01);
                return col;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Shader Graph/FallbackError"
}
