using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Sekai.CustomMusicScoreManager
{
  public class PercentSelector : MonoBehaviour
  {
    [SerializeField]
    private Button MinButton;

    [SerializeField]
    private Button AddButton;

    [SerializeField]
    private TMP_InputField Input;

    [NonSerialized]
    public int Value;

    private int Min;

    private int Max;

    private int StepSize;

    private bool IsSetup = false;

    public void Setup(int value, int min, int max, int stepSize = 5)
    {
      if (IsSetup == true)
      {
        return;
      }

      Min = min;
      Max = max;
      StepSize = stepSize;
      ChangeValue(value);

      MinButton?.onClick.AddListener(() => ChangeValue(Value - StepSize));
      AddButton?.onClick.AddListener(() => ChangeValue(Value + StepSize));
      Input?.onEndEdit.AddListener(ChangeValue);

      IsSetup = true;
    }

    private void ChangeValue(int val)
    {
      ChangeValue(val.ToString());
    }

    private void ChangeValue(string val)
    {
      if (int.TryParse(val, out int value))
      {
        Value = Mathf.Clamp(Mathf.FloorToInt(value / StepSize) * StepSize, Min, Max);
        Input?.SetTextWithoutNotify($"{Value}%");

        if (MinButton != null)
        {
          MinButton.interactable = Value - StepSize >= Min;
        }
        if (AddButton != null)
        {
          AddButton.interactable = Value + StepSize <= Max;
        }
      }
    }
  }
}