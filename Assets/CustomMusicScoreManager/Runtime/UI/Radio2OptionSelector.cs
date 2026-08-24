using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sekai.CustomMusicScoreManager
{
  public class Radio2OptionSelector : MonoBehaviour
  {
    [SerializeField]
    private Button Button1;

    [SerializeField]
    private Image Button1Fill;

    [SerializeField]
    private Button Button2;

    [SerializeField]
    private Image Button2Fill;

    [NonSerialized]
    public int Value = 0;

    public bool Boolean => Value == 1 ? true : false;

    private bool IsSetup = false;

    public void Setup(bool value)
    {
      Setup(value == true ? 1 : 0);
    }

    public void Setup(int value)
    {
      if (IsSetup == true)
      {
        return;
      }

      ChangeValue(value);

      Button1?.onClick.AddListener(() => ChangeValue(0));
      Button2?.onClick.AddListener(() => ChangeValue(1));

      IsSetup = true;
    }

    private void ChangeValue(int value)
    {
      Value = ((value % 2) + 2) % 2;

      Button1Fill.gameObject.SetActive(Value == 0);
      Button2Fill.gameObject.SetActive(Value == 1);
    }
  }
}