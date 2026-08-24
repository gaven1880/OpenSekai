using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Sekai.Live;

namespace Sekai.CustomMusicScoreManager
{
  public class NoteSeSelector : MonoBehaviour
  {
    [NonSerialized]
    public int Index = 0;

    private int Length = 2;

    [SerializeField]
    private Button MinButton;

    [SerializeField]
    private TextMeshProUGUI DisplayText;

    [SerializeField]
    private Button AddButton;

    [SerializeField]
    private Button PerfectButton;

    [SerializeField]
    private Button FlickButton;

    [SerializeField]
    private Button LongButton;

    [SerializeField]
    private Button FrictionButton;

    private uint currentPlaySeId;

    private Coroutine elapsedStopCoroutine;

    private bool IsSetup = false;

    public void Setup(int index = 0, int length = 2)
    {
      if (IsSetup == true)
      {
        return;
      }

      Length = length;
      ChangeIndex(index);

      MinButton?.onClick.AddListener(() => ChangeIndex(Index - 1));
      AddButton?.onClick.AddListener(() => ChangeIndex(Index + 1));

      PerfectButton?.onClick.AddListener(() => PlayTestSe(LiveSoundDefine.SE_LIVE_PERFECT));
      FlickButton?.onClick.AddListener(() => PlayTestSe(LiveSoundDefine.SE_LIVE_FLICK));
      LongButton?.onClick.AddListener(() => PlayTestSe(LiveSoundDefine.SE_LIVE_LONG, true));
      FrictionButton?.onClick.AddListener(() => PlayTestSe(LiveSoundDefine.SE_LIVE_TRACE));

      IsSetup = true;
    }

    private void ChangeIndex(int index)
    {
      if (Length == 0)
      {
        return;
      }

      string oldBundleName = $"live/tap_se/{LiveConfig.GetNoteSeName(Index)}";

      Index = ((index % Length) + Length) % Length;
      DisplayText?.SetText(LiveConfig.GetNoteSeViewName(Index));

      LiveConfig.SetNoteSeName(Index);
      
      string bundleName = $"live/tap_se/{LiveConfig.GetNoteSeName(Index)}";

      if (!string.Equals(oldBundleName, bundleName, StringComparison.Ordinal))
      {
        SoundManager.Instance.UnloadSoundBundle(oldBundleName);
      }

      SoundManager.Instance.LoadSoundBundle(bundleName, true);
    }

    private void PlayTestSe(string cueName, bool isElapsedStop = false, float elapsedSeconds = 2f)
    {
      if (currentPlaySeId != 0)
      {
        SoundManager.Instance.StopSE(currentPlaySeId);
        currentPlaySeId = 0;
      }
      if (elapsedStopCoroutine != null)
      {
        StopCoroutine(elapsedStopCoroutine);
        elapsedStopCoroutine = null;
      }

      currentPlaySeId = SoundManager.Instance.PlaySE(cueName);

      if (isElapsedStop)
      {
        elapsedStopCoroutine = StartCoroutine(ElapsedStopSe(currentPlaySeId, elapsedSeconds));
      }
    }

    private System.Collections.IEnumerator ElapsedStopSe(uint seId, float seconds)
    {
      yield return new WaitForSeconds(seconds);
      if (currentPlaySeId == seId)
      {
        SoundManager.Instance.StopSE(seId);
        currentPlaySeId = 0;
      }
    }
  }
}