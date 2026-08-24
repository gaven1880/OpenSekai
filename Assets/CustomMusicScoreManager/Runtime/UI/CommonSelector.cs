using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sekai.CustomMusicScoreManager
{
  public class CommonSelector : MonoBehaviour
  {
    [SerializeField]
    private Button Min3Button;

    [SerializeField]
    private Button Min2Button;

    [SerializeField]
    private Button Min1Button;

    [SerializeField]
    private TMP_InputField Input;

    [SerializeField]
    private Button Add1Button;

    [SerializeField]
    private Button Add2Button;

    [SerializeField]
    private Button Add3Button;

    [System.NonSerialized]
    public float Value;

    private float Min;

    private float Max;

    private float Unit1;

    private float Unit2;

    private float Unit3;

    [System.NonSerialized]
    public float DecimalPlaces = 2;

    private bool IsSetup = false;

    public void Setup(float val, float min, float max, float unit1 = 0.01f, float unit2 = 0.1f, float unit3 = 1f)
    {
      if (IsSetup)
      {
        return;
      }

      Min = min;
      Max = max;
      ChangeValue(val);
      Unit1 = unit1;
      Unit2 = unit2;
      Unit3 = unit3;

      Min3Button?.gameObject.SetActive(Unit3 != 0);
      Min2Button?.gameObject.SetActive(Unit2 != 0);
      Min1Button?.gameObject.SetActive(Unit1 != 0);
      Add1Button?.gameObject.SetActive(Unit1 != 0);
      Add2Button?.gameObject.SetActive(Unit2 != 0);
      Add3Button?.gameObject.SetActive(Unit3 != 0);

      Min3Button?.GetComponentInChildren<TextMeshProUGUI>()?.SetText($"-{Unit3}");
      Min2Button?.GetComponentInChildren<TextMeshProUGUI>()?.SetText($"-{Unit2}");
      Min1Button?.GetComponentInChildren<TextMeshProUGUI>()?.SetText($"-{Unit1}");
      Add1Button?.GetComponentInChildren<TextMeshProUGUI>()?.SetText($"+{Unit1}");
      Add2Button?.GetComponentInChildren<TextMeshProUGUI>()?.SetText($"+{Unit2}");
      Add3Button?.GetComponentInChildren<TextMeshProUGUI>()?.SetText($"+{Unit3}");

      ChangeValue(Value);

      Input?.onEndEdit.AddListener(ChangeValue);

      Min3Button?.onClick.AddListener(() => ChangeValue(Value - Unit3));
      Min2Button?.onClick.AddListener(() => ChangeValue(Value - Unit2));
      Min1Button?.onClick.AddListener(() => ChangeValue(Value - Unit1));
      Add1Button?.onClick.AddListener(() => ChangeValue(Value + Unit1));
      Add2Button?.onClick.AddListener(() => ChangeValue(Value + Unit2));
      Add3Button?.onClick.AddListener(() => ChangeValue(Value + Unit3));

      IsSetup = true;
    }

    public void ChangeValue(float val)
    {
      ChangeValue(val.ToString($"F{DecimalPlaces}"));
    }

    public void ChangeValue(string val)
    {
      if (float.TryParse(val, out float value))
      {
        Value = Mathf.Clamp(value, Min, Max);
        Input?.SetTextWithoutNotify(Value.ToString($"F{DecimalPlaces}"));

        if (Min3Button != null)
        {
          Min3Button.interactable = Value - Unit3 >= Min;
        }
        if (Min2Button != null)
        {
          Min2Button.interactable = Value - Unit2 >= Min;
        }
        if (Min1Button != null)
        {
          Min1Button.interactable = Value - Unit1 >= Min;
        }
        if (Add3Button != null)
        {
          Add3Button.interactable = Value + Unit3 <= Max;
        }
        if (Add2Button != null)
        {
          Add2Button.interactable = Value + Unit2 <= Max;
        }
        if (Add1Button != null)
        {
          Add1Button.interactable = Value + Unit1 <= Max;
        }
      }
    }
  }
}