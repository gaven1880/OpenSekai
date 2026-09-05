// Original shader: Sekai/Movie/Grid/SofdecPrimeYuv
// Reconstructed version based on Sprites/Default for only the Filter effect

Shader "Sekai/Movie/Grid"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
    [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
    [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
    [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    _Filter ("Filter", Float) = 0.75
  }

  SubShader
  {
    Tags
    {
      "Queue"="Transparent"
      "IgnoreProjector"="True"
      "RenderType"="Transparent"
      "PreviewType"="Plane"
      "CanUseSpriteAtlas"="True"
    }

    Cull Off
    Lighting Off
    ZWrite Off
    Blend One OneMinusSrcAlpha

    Pass
    {
      CGPROGRAM
        #pragma vertex SpriteVert
        #pragma fragment frag
        #pragma target 3.0
        #pragma multi_compile_instancing
        #pragma multi_compile_local _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
        #include "UnitySprites.cginc"
        float _Filter;

        fixed4 frag(v2f i) : SV_TARGET
        {
          float4 u_xlat0;
          bool u_xlatb0;
          float u_xlat3;
          fixed4 u_xlat16_2;
          u_xlat0.x = (i.vertex.x / _ScreenParams.x * 2.0 - 1.0) * 0.5 + 0.5;
          u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
          u_xlat0.x = u_xlat0.x * _ScreenParams.x;
          u_xlat0.x = u_xlat0.x * 0.25;
          u_xlat0.x = frac(u_xlat0.x);
          u_xlatb0 = 0.5 >= u_xlat0.x;
          u_xlat0.x = u_xlatb0 ? 1.0 : float(0.0);
          u_xlat3 = (-_Filter) + 1.0;
          u_xlat0.x = u_xlat0.x * u_xlat3 + _Filter;
          u_xlat16_2 = SpriteFrag(i);
          u_xlat0 = u_xlat0.xxxx * u_xlat16_2;
          return u_xlat0;
        }
      ENDCG
    }
  }
}
