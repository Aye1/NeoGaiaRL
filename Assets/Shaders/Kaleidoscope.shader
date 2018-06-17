Shader "Tutorial/Basic" {
    Properties {
        _MainColor ("Main Color", Color) = (1,0.5,0.5,1)
		_SecondColor ("Secondary Color", Color) = (0.0, 0.0, 0.0, 1.0)
		_ThirdColor ("Third Color", Color) = (0.0, 0.0, 0.0, 1.0)
		Threshold ("Threshold", Range(0,1)) = 1
		FMod ("FMod", Range(0,1)) = 1
    }
    SubShader {
        Pass {
			CGPROGRAM
			#pragma fragment frag
			#pragma vertex vert

			half4 _MainColor;
			half4 _SecondColor;
			half4 _ThirdColor;
			float Threshold;
			half FMod;

			// vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
				float2 uv : TEXCOORD0;
            };

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			float rand(half seed) {
				return frac(sin(seed));
			}

			v2f vert (appdata v) {
				v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
			}

			fixed4 frag (v2f input) : SV_Target
			{
				fixed4 returnColor;
				half pi = radians(180);
				//float diff = abs(_Time.x - input.uv.x);
				//float diff = abs(input.uv.y / input.uv.x - tan(_Time.x));
				//float angle = radians(diff*90);
				//float rounded = abs(cos(angle));
				//float thresholded = step(angle, Threshold);

			    //float diff = abs(acos(input.uv.x) * length(input.uv) - abs(_CosTime.z));
				//float thresholded = diff;

				// -- Rayon qui tourne
				//float diff = abs(acos(input.uv.x/length(input.uv)) - (_CosTime.z+1)*0.25*pi)/pi;
				//float thresholded = step(diff, Threshold);
				// --

				float a = -1.7320508;
				half time = _Time.x;
				float calc = input.uv.y - a*input.uv.x;
				float newMod = FMod * (fmod(_Time.x, 0.5) * 0.1 + 0.9);
				float diff = fmod(abs(calc - abs(time)*1.5),newMod);
				float diff2 = fmod(abs(calc - abs(time - 0.1)*1.5),newMod);
				float thresholded = step(diff, Threshold * abs(_CosTime.y));
				float thresholded2 = step(diff2, Threshold * abs(_SinTime.z + 0.5));

				half4 prim = _MainColor;
				prim.x = abs(_CosTime.x) * _MainColor.x;
				prim.y = abs(_CosTime.y) * _MainColor.y;
				prim.z = abs(_CosTime.z) * _MainColor.z;

				returnColor = _SecondColor * thresholded 
				+ _ThirdColor * thresholded2 
				+ prim * (1-thresholded - thresholded2);
				//returnColor = _SecondColor * thresholded + _MainColor * (1-thresholded);
				return returnColor;
			}
			ENDCG
        }
    }
}