using UnityEngine;
using UnityEngine.UI;
using System;

namespace Sekai.CustomMusicScoreManager
{
  public class SpriteSelector : MonoBehaviour
  {
    [SerializeField]
    private Sprite[] Sprites;

    [SerializeField]
    protected Image Image;

    [SerializeField]
    private Button MinButton;

    [SerializeField]
    private Button AddButton;

    [NonSerialized]
    public int Index;

    protected bool IsSetup = false;

    public void Setup(int index)
    {
      if (IsSetup == true)
      {
        return;
      }

      Index = index;
      UpdateSprite();
      MinButton?.onClick.AddListener(() => ChangeIndex(Index - 1));
      AddButton?.onClick.AddListener(() => ChangeIndex(Index + 1));
      IsSetup = true;
    }

    public virtual void ChangeIndex(int index)
    {
      if (Sprites == null || Sprites.Length == 0)
      {
        return;
      }

      Index = ((index % Sprites.Length) + Sprites.Length) % Sprites.Length;
      UpdateSprite();
    }

    public void UpdateSprite()
    {
      if (Image != null && Sprites != null && Sprites.Length > 0)
      {
        Image.sprite = Sprites[Index];
      }
    }
  }
}