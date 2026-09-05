using Sekai.Live;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sekai.CustomMusicScoreManager
{
  public class SettingsView : MonoBehaviour
  {
    [SerializeField]
    private Image OverlayBackground;

    [SerializeField]
    private Button CloseButton;

    [SerializeField]
    private PercentSelector BGMVolumeSelector;

    [SerializeField]
    private PercentSelector SEVolumeSelector;

    [SerializeField]
    private CommonSelector NoteSpeedSelector;

    [SerializeField]
    private CommonSelector TimingAdjustSelector;

    [SerializeField]
    private CommonSelector NoteShowRateSelector;

    [SerializeField]
    private SpriteSelector NoteSkinSelector;

    [SerializeField]
    private AlphaSpriteSelector NoteLineAlphaSelector;

    [SerializeField]
    private AlphaSpriteSelector GuideAlphaSelector;

    [SerializeField]
    private NoteSeSelector NoteSeSelector;

    [SerializeField]
    private PercentSelector BrightnessSelector;

    [SerializeField]
    private PercentSelector LaneAlphaSelector;

    [SerializeField]
    private Radio2OptionSelector NoteEffectSelector;

    [SerializeField]
    private Radio2OptionSelector SimultaneousLineSelector;

    [SerializeField]
    private Radio2OptionSelector APEffectSelector;

    [SerializeField]
    private Radio2OptionSelector FastLateFlickSelector;

    [SerializeField]
    private Radio2OptionSelector MirrorSelector;

    [SerializeField]
    private Radio2OptionSelector BackgroundModeSelector;

    [SerializeField]
    private Radio2OptionSelector MusicInfoDisplayModeSelector;

    [SerializeField]
    private Radio2OptionSelector DesktopFullscreenSelector;

    [SerializeField]
    private Radio2OptionSelector MVLineSelector;

    [SerializeField]
    private TMP_InputField TotalPowerSelector;

    private ApplicationLocalSettings LocalSettings;

    private LiveSettingData SettingData;

    private bool IsSetup = false;

    public void Setup(LiveSettingData liveSettingData)
    {
      if (IsSetup)
      {
        return;
      }

      LocalSettings = ApplicationLocalSettings.LoadFromStorage();
      SettingData = liveSettingData;

      BGMVolumeSelector?.Setup((int)(LocalSettings.LiveVolume.Bgm * 100f), 0, 100, 5);
      SEVolumeSelector?.Setup((int)(LocalSettings.LiveVolume.Se * 100f), 0, 100, 5);

      NoteSpeedSelector?.Setup(SettingData.NoteSpeed, LiveConfig.MinNoteSpeed, LiveConfig.MaxNoteSpeed);
      TimingAdjustSelector?.Setup(SettingData.TimingAdjustData, LiveConfig.MinNoteTiming, LiveConfig.MaxNoteTiming);
      if (NoteShowRateSelector != null)
      {
        NoteShowRateSelector.DecimalPlaces = 0;
        NoteShowRateSelector.Setup(SettingData._noteShowRate * 100f, LiveConfig.MinNoteShowRate, LiveConfig.MaxNoteShowRate, 1, 10, 0);
      }
      NoteSkinSelector?.Setup(SettingData.NoteSkinIndex);
      // As a useful "bug", we can make note line and guide alpha use a class that extends SpriteSelector,
      // then use the NoteSkinSelector's left/right buttons on them to switch skins.
      NoteLineAlphaSelector?.Setup(SettingData.NoteSkinIndex, (int)(SettingData.GetNoteAlpha() * 100f), 10, 100, 5);
      GuideAlphaSelector?.Setup(SettingData.NoteSkinIndex, (int)(SettingData.GetGuideAlpha() * 100f), 10, 100, 5);
      NoteSeSelector?.Setup(SettingData.NoteSeIndex);

      BrightnessSelector?.Setup((int)(SettingData.Brightness * 100f), LiveConfig.MinBrightness, LiveConfig.MaxBrightness, (int)LiveConfig.VariationBrightness);
      LaneAlphaSelector?.Setup((int)(SettingData.LaneTransparent * 100f), LiveConfig.MinLaneAlpha, LiveConfig.MaxLaneAlpha, (int)LiveConfig.VariationLaneAlpha);
      NoteEffectSelector?.Setup(SettingData.NoteEffect);

      SimultaneousLineSelector?.Setup(SettingData.UseSimultaneousPushingLine);
      APEffectSelector?.Setup(SettingData.UseAllPerfectEffect);
      FastLateFlickSelector?.Setup(SettingData.IsFastLateFlick);
      MirrorSelector?.Setup(SettingData.IsMirror);

      BackgroundModeSelector?.Setup(1 - SettingData.CustomMusicScoreLiveBackgroundMode ?? 1);

      MusicInfoDisplayModeSelector?.Setup(SettingData.CustomMusicScoreMusicInfoDisplayMode ?? 0);
#if UNITY_EDITOR || UNITY_STANDALONE
      DesktopFullscreenSelector?.Setup(LocalSettings.FullscreenEnabled ?? Screen.fullScreen);
#else
      DesktopFullscreenSelector?.gameObject.SetActive(false);
#endif
      MVLineSelector?.Setup(LocalSettings.EnableMVLine);
      TotalPowerSelector?.SetTextWithoutNotify(LocalSettings.TotalPower.ToString());

      OverlayBackground?.GetComponent<Button>()?.onClick.AddListener(Hide);
      CloseButton?.onClick.AddListener(Hide);

      IsSetup = true;
    }

    public void Show()
    {
      gameObject.SetActive(true);
      gameObject.GetComponent<RectTransform>()?.SetAsLastSibling();
    }

    public void Hide()
    {
      Save();
      gameObject.SetActive(false);
    }

    public void Save()
    {
      if (SettingData == null)
      {
        return;
      }

      if (BGMVolumeSelector != null)
      {
        LocalSettings.LiveVolume.Bgm = (float)(BGMVolumeSelector.Value / 100f);
      }
      if (SEVolumeSelector != null)
      {
        LocalSettings.LiveVolume.Se = (float)(SEVolumeSelector.Value / 100f);
      }
      if (NoteSpeedSelector != null)
      {
        SettingData.NoteSpeed = NoteSpeedSelector.Value;
      }
      if (TimingAdjustSelector != null)
      {
        SettingData.TimingAdjustData = TimingAdjustSelector.Value;
      }
      if (NoteShowRateSelector != null)
      {
        SettingData.SetNoteShowRate(NoteShowRateSelector.Value);
      }
      if (NoteSkinSelector != null)
      {
        SettingData.NoteSkinIndex = NoteSkinSelector.Index;
      }
      if (NoteLineAlphaSelector != null)
      {
        SettingData.NoteAlpha = (float)(NoteLineAlphaSelector.Value / 100f);
      }
      if (GuideAlphaSelector != null)
      {
        SettingData.GuideAlpha = (float)(GuideAlphaSelector.Value / 100f);
      }
      if (NoteSeSelector != null)
      {
        SettingData.NoteSeIndex = NoteSeSelector.Index;
      }
      if (BrightnessSelector != null)
      {
        SettingData.Brightness = (float)(BrightnessSelector.Value / 100f);
      }
      if (LaneAlphaSelector != null)
      {
        SettingData.LaneTransparent = (float)(LaneAlphaSelector.Value / 100f);
      }
      if (NoteEffectSelector != null)
      {
        SettingData.NoteEffect = NoteEffectSelector.Value;
      }
      if (SimultaneousLineSelector != null)
      {
        SettingData.UseSimultaneousPushingLine = SimultaneousLineSelector.Boolean;
      }
      if (APEffectSelector != null)
      {
        SettingData.UseAllPerfectEffect = APEffectSelector.Boolean;
      }
      if (FastLateFlickSelector != null)
      {
        SettingData.IsFastLateFlick = FastLateFlickSelector.Boolean;
      }
      if (MirrorSelector != null)
      {
        SettingData.IsMirror = MirrorSelector.Boolean;
      }
      if (BackgroundModeSelector != null)
      {
        SettingData.CustomMusicScoreLiveBackgroundMode = 1 - BackgroundModeSelector.Value;
      }
      if (MusicInfoDisplayModeSelector != null)
      {
        SettingData.CustomMusicScoreMusicInfoDisplayMode = MusicInfoDisplayModeSelector.Value;
      }
#if UNITY_EDITOR || UNITY_STANDALONE
      if (DesktopFullscreenSelector != null)
      {
        LocalSettings.FullscreenEnabled = DesktopFullscreenSelector.Boolean;
        Screen.fullScreen = DesktopFullscreenSelector.Boolean;
      }
#endif
      if (MVLineSelector != null)
      {
        LocalSettings.EnableMVLine = MVLineSelector.Boolean;
      }
      if (TotalPowerSelector != null)
      {
        LocalSettings.TotalPower = int.Parse(TotalPowerSelector.text);
      }

      ApplicationLocalSettings.SaveToStorage(LocalSettings);
      LiveSettingData.SaveToStorage(SettingData);
    }
  }
}