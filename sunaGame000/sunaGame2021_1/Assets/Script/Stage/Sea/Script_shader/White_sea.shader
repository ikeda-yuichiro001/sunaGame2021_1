Shader "Custom/White_sea"
{
	Properties 
	{ 
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" }
		LOD 200
		Cull Off
		CGPROGRAM  
		#pragma surface surf Standard alpha:fade 
		#pragma target 3.0  
		struct Input { float3 worldPos; };  
		 
		void surf(Input IN, inout SurfaceOutputStandard o) 
		{     
			o.Albedo = fixed4(1,1, 1,1);
			o.Alpha = (IN.worldPos.y + 20) *  -0.1; //3 / -0.15
	    } 
		ENDCG
	}
	FallBack "Diffuse"
}
   