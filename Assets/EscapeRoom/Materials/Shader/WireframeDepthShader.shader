Shader "Unlit/WireframeDepthShader" {
  Properties {
    _WireframeColor("WireframeColor", Color) = (1, 0, 0, 1)
    _Color("Color", Color) = (1, 1, 1, 1)
    _DistanceMultipler("DistanceMultiplier", Range(1, 5)) = 1
    _DepthBias("Depth Bias", float) = 0.06
	_LineWidth("Line Width", float) = 0.01
  }

  SubShader {
    Pass {
      CGPROGRAM
      #include "UnityCG.cginc"
      #pragma vertex vert
	  //#pragma geometry geom
      #pragma fragment frag

      #include "Packages/com.meta.xr.depthapi/Runtime/BiRP/EnvironmentOcclusionBiRP.cginc"
      #pragma multi_compile _ HARD_OCCLUSION SOFT_OCCLUSION

      half4 _WireframeColor, _Color;
      float _LineThickness, _DistanceMultipler, _DepthBias, _LineWidth;

      struct appdata
      {
        float4 vertex : POSITION;
        float4 color : COLOR; // barycentric coords
        UNITY_VERTEX_INPUT_INSTANCE_ID
      };

      struct v2f
      {
        float4 vertex : SV_POSITION;
        float3 vertexView : TEXCOORD0;
        float3 color: COLOR;

		float3 barycentric : TEXCOORD2;

        META_DEPTH_VERTEX_OUTPUT(1)

        UNITY_VERTEX_INPUT_INSTANCE_ID
        UNITY_VERTEX_OUTPUT_STEREO // required for stereo
      };

      v2f vert(appdata v)
      {
        v2f o;
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_INITIALIZE_OUTPUT(v2f, o);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		

        o.vertex = UnityObjectToClipPos(v.vertex);
        o.vertexView = UnityObjectToViewPos(v.vertex);
        o.color = v.color;

        META_DEPTH_INITIALIZE_VERTEX_OUTPUT(o, v.vertex);
        return o;
      }

	  
	  /*[maxvertexcount(3)]
	  void geom(triangle v2f IN[3], inout TriangleStream<v2f> triStream)
	  {
		  v2f o;
		  o = IN[0];
		  //o.pos = IN[0].vertex;
		  o.barycentric = float3(1.0, 0.0, 0.0);
		  triStream.Append(o);
		  o = IN[1];
		  //o.pos = IN[1].vertex;
		  o.barycentric = float3(0.0, 1.0, 0.0);
		  triStream.Append(o);
		  o = IN[2];
		  //o.pos = IN[2].vertex;
		  o.barycentric = float3(0.0, 0.0, 1.0);
		  triStream.Append(o);
	  }*/
	  

      half4 frag(v2f i) : SV_Target
      {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
        // on edge, one or the coordinates is 0
        float closest = min(i.color.x, min(i.color.y, i.color.z));
		//float closest = min(i.barycentric.x, min(i.barycentric.y, i.barycentric.z));

        // use distance to make far away edges visible
        float distance = length(i.vertexView) * _DistanceMultipler * _LineWidth;
        float val = closest/distance;

        half4 color = lerp(_WireframeColor, _Color, clamp(val,0,1));

        META_DEPTH_OCCLUDE_OUTPUT_PREMULTIPLY(i, color, _DepthBias);

        return color;
      }
      ENDCG
    }
  }
}
