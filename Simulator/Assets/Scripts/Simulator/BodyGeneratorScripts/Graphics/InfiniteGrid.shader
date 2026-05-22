Shader "Unlit/InfiniteGrid"
{
    Properties
    {
        [Toggle] _WorldUV ("Use World Space UV", Float) = 1.0
        _GridScale ("Grid Scale", Float) = 1.0
        _GridBias ("Grid Bias", Float) = 0.5
        _GridDiv ("Grid Divisions", Float) = 10.0
        _BaseColor ("Base Color", Color) = (0,0,0,1)
        _LineColor ("Line Color", Color) = (1,1,1,1)
        _LineWidth ("Line Width (in pixels)", Range(0,10)) = 0.33
        _MajorLineWidth ("Major Line Width (in pixels)", Range(0,10)) = 2.0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 cameraPos : TEXCOORD1;
            };

            bool _WorldUV;
            float _GridScale, _GridDistScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0)).xyz;
                o.uv = (_WorldUV ? worldPos.xz : v.uv) * _GridScale;
                
                if (UNITY_MATRIX_P[3][3] >= 1.0) // if ortographic
                    o.cameraPos = float3(0.0, 0.0, 1.0);
                else // perspective
                    o.cameraPos = mul(UNITY_MATRIX_V, float4(worldPos, 1.0)).xyz;
                // adjust for persp fov & ortho scale
                o.cameraPos /= UNITY_MATRIX_P[1][1];

                return o;
            }

            half4 _LineColor, _BaseColor;
            float _GridDiv, _GridBias, _LineWidth, _MajorLineWidth;

            fixed4 frag (v2f i) : SV_Target
            {
                // number of divisions in the grid
                float gridDiv = max(round(_GridDiv), 2.0);

                // distance to surface (or orth scale)
                // float viewLength = length(i.cameraPos);
                // log length of view length
                // float logLength = log(viewLength)/log(gridDiv) - _GridBias;

                // optimization of the above two lines taking advantage of this fact:
                // log(sqrt(value)) == 0.5 * log(value)
                float logLength = (0.5 * log(dot(i.cameraPos, i.cameraPos)))/log(gridDiv) - _GridBias;

                // get stepped log values
                float logA = floor(logLength);
                float logB = logA + 1.0;
                float blendFactor = frac(logLength);

                // scales for each UV set derived from log values
                float uvScaleA = pow(gridDiv, logA);
                float uvScaleB = pow(gridDiv, logB);

                // scale each UV
                float2 UVA = i.uv / uvScaleA;
                float2 UVB = i.uv / uvScaleB;

                // proceedural grid
                // we use a third UV set for the grid to show major grid lines thicker
                float logC = logA + 2.0;
                float uvScaleC = pow(gridDiv, logC);
                float2 UVC = i.uv / uvScaleC;

                // proceedural grid UVs sawtooth to triangle wave
                float2 gridUVA = 1.0 - abs(frac(UVA) * 2.0 - 1.0);
                float2 gridUVB = 1.0 - abs(frac(UVB) * 2.0 - 1.0);
                float2 gridUVC = 1.0 - abs(frac(UVC) * 2.0 - 1.0);

                // adjust line width based on blend factor
                float lineWidthA = _LineWidth * (1-blendFactor);
                float lineWidthB = lerp(_MajorLineWidth, _LineWidth, blendFactor);
                float lineWidthC = _MajorLineWidth * blendFactor;

                // fade lines out when below 1 pixel wide
                float lineFadeA = saturate(lineWidthA);
                float lineFadeB = saturate(lineWidthB);
                float lineFadeC = saturate(lineWidthC);

                // get screen space derivatives of base UV
                float2 uvLength = max(float2(length(float2(ddx(i.uv.x), ddy(i.uv.x))), length(float2(ddx(i.uv.y), ddy(i.uv.y)))), 0.00001);

                // calculate UV space width for anti-aliasing
                // This is done by scaling base UV rather than using derivatives of the pre-scaled UVs
                // to avoid artifacts at scale discontinuity edges. Note, this intentionally does not
                // into account the * 2.0 that was applied to the grid UVs so these are half derivs!
                float2 lineAAA = uvLength / uvScaleA;
                float2 lineAAB = uvLength / uvScaleB;
                float2 lineAAC = uvLength / uvScaleC;

                // use smoothstep to get nice anti-aliasing on lines
                // line width * half deriv == 1.0 equals 1 pixel wide rather than 2 pixels wide
                // +/- 1.5 * half deriv == +/- 0.75 pixel AA
                float2 grid2A = smoothstep((lineWidthA + 1.5) * lineAAA, (lineWidthA - 1.5) * lineAAA, gridUVA);
                float2 grid2B = smoothstep((lineWidthB + 1.5) * lineAAB, (lineWidthB - 1.5) * lineAAB, gridUVB);
                float2 grid2C = smoothstep((lineWidthC + 1.5) * lineAAC, (lineWidthC - 1.5) * lineAAC, gridUVC);

                // combine x and y grid lines together and apply < 1 pixel width fade
                float gridA = saturate(grid2A.x + grid2A.y) * lineFadeA;
                float gridB = saturate(grid2B.x + grid2B.y) * lineFadeB;
                float gridC = saturate(grid2C.x + grid2C.y) * lineFadeC;

                // combine all 3 grids together
                float grid = saturate(gridA + max(gridB, gridC));

                // lerp between base and line color
                float4 col = lerp(_BaseColor, _LineColor, grid);

                col.a = grid;

                return col;
            }
            ENDCG
        }
    }
}