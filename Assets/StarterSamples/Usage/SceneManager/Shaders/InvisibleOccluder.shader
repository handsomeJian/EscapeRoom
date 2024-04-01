Shader "Scene/InvisibleOccluder" {
  Properties {
  }
  SubShader {
    Tags{"RenderType" = "Transparent"}
    LOD 100
    Cull off
    ZWrite On
    ZTest Less
    Blend Zero One, Zero One
    Pass {
      CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog
#include "UnityCG.cginc"

      struct appdata {
        float4 vertex : POSITION;
		UNITY_VERTEX_INPUT_INSTANCE_ID
      };

      struct v2f {
        float4 vertex : SV_POSITION;
		UNITY_VERTEX_OUTPUT_STEREO
      };

      v2f vert(appdata v) {
        v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_OUTPUT(v2f, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
        o.vertex = UnityObjectToClipPos(v.vertex);
        return o;
      }

      fixed4 frag(v2f i) : SV_Target {
        return float4(0,0,0,0);
      }
      ENDCG
    }
  }
}
