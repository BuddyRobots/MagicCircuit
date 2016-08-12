//Shader "Custom/shader" {
//	Properties {
//		_Color ("Color", Color) = (1,1,1,1)
//		_MainTex ("Albedo (RGB)", 2D) = "white" {}
//		_Glossiness ("Smoothness", Range(0,1)) = 0.5
//		_Metallic ("Metallic", Range(0,1)) = 0.0
//	}
//	SubShader {
//		Tags { "RenderType"="Opaque" }
//		LOD 200
//		
//		CGPROGRAM
//		// Physically based Standard lighting model, and enable shadows on all light types
//		#pragma surface surf Standard fullforwardshadows
//
//		// Use shader model 3.0 target, to get nicer looking lighting
//		#pragma target 3.0
//
//		sampler2D _MainTex;
//
//		struct Input {
//			float2 uv_MainTex;
//		};
//
//		half _Glossiness;
//		half _Metallic;
//		fixed4 _Color;
//
//		void surf (Input IN, inout SurfaceOutputStandard o) {
//			// Albedo comes from a texture tinted by color
//			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
//			o.Albedo = c.rgb;
//			// Metallic and smoothness come from slider variables
//			o.Metallic = _Metallic;
//			o.Smoothness = _Glossiness;
//			o.Alpha = c.a;
//		}
//		ENDCG
//	}
//	FallBack "Diffuse"
//}

Shader "My/BlackLine" {
	
	// A simple shader to draw lines 
	// (backface culling and lightning are turned off)
	
    Properties {
    	_Color ("Main Color", Color) = (0,0,0,1)
    	//_BackColor ("Back Color", Color) = (1,0,0,1)
    }
    
    SubShader {
        Pass {
        	Blend SrcAlpha OneMinusSrcAlpha 
            ZWrite Off
        	Color [_Color]
            Lighting Off
            Cull Off
        }
        
        /* Pass {
        	Blend SrcAlpha OneMinusSrcAlpha 
            ZWrite Off
        	Color [_BackColor]
            Lighting Off
            Cull Front  
        } */
    }
    
    FallBack "VertexLit"
} 