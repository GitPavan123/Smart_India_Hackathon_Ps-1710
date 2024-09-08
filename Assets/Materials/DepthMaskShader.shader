Shader "Custom/DepthMask"
{
    SubShader
    {
        Tags { "Queue" = "Geometry-1" }
        ColorMask 0
        ZWrite On

        Pass
        {
            // No visual output, only writes to depth buffer
        }
    }
}
