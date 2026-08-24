using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sekai.CustomMusicScoreManager
{
  public class AlphaSpriteSelector : SpriteSelector
  {
    [SerializeField]
    private PercentSelector AlphaSelector;

    [NonSerialized]
    public int Value = 100;

    public void Setup(int index, int alpha, int min, int max, int stepSize = 5)
    {
      if (IsSetup == true)
      {
        return;
      }

      Value = alpha;

      Setup(index);
      AlphaSelector?.Setup(alpha, min, max, stepSize);
    }

    private void Update()
    {
      if (Image != null)
      {
        Color color = Image.color;
        if (AlphaSelector != null)
        {
          Value = AlphaSelector.Value;
        }
        color.a = Value / 100f;
        Image.color = color;
      }
    }
  }
}