Shader "Unlit/CaseSurfaceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Radius ("Radius", Float) = 0.65
		_HandPos ("HandPosition", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

		Pass
		{
			LOD 100
			//Cull off
			ZWrite off
			Blend OneMinusSrcAlpha SrcAlpha, OneMinusSrcAlpha SrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Radius;
			float4 _HandPos;

			v2f vert(appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				//o.worldPos = v.vertex;


				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float dis = distance(i.worldPos, _HandPos.xyz);
				//float dis = distance(i.worldPos, float3(0, 0, 0));
				float alpha = 1 - step(_Radius, dis);
				return float4(alpha, alpha, alpha, alpha);
			}

			ENDCG
		}

		/*
        Pass
        {
			ZWrite Off
			BlendOp Add, Min
			Blend Zero One, One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				return float4(0, 0, 0, 0);
				/*
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
				
                return col;*/
            }
            ENDCG
        }*/
    }
}
