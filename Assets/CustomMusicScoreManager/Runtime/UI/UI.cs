using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Sekai.CustomMusicScoreManager
{
  public class UI
  {
    private const string BaseFontEbPath = "font/FOT-RodinNTLGPro-EB SDF_Base";

		private const string DynamicFontEbPath = "font/FOT-RodinNTLGPro-EB SDF_Dynamic";

		private const string BaseFontDbPath = "font/FOT-RodinNTLGPro-DB SDF_Base";

		private const string DynamicFontDbPath = "font/FOT-RodinNTLGPro-DB SDF_Dynamic";
		private static TMP_FontAsset _baseFontEB;

		private static TMP_FontAsset _baseFontDB;

		private static bool _fontAssetSetup;

    public static TextMeshProUGUI CreateText(string name, Transform parent, string text, float fontSize, FontStyles style, TextAlignmentOptions alignment)
		{
			GameObject go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
			go.transform.SetParent(parent, false);
			TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
			TMP_FontAsset fontAsset = GetOriginalFontAsset(style);
			if (fontAsset != null)
			{
				tmp.font = fontAsset;
			}
			tmp.text = text;
			tmp.fontSize = fontSize;
			tmp.fontStyle = style;
			tmp.alignment = alignment;
			tmp.color = new Color32(238, 243, 247, 255);
			tmp.enableWordWrapping = false;
			tmp.overflowMode = TextOverflowModes.Ellipsis;
			return tmp;
		}

    private static TMP_FontAsset GetOriginalFontAsset(FontStyles style)
		{
			SetupOriginalFontAssets();
			return (style & FontStyles.Bold) != 0 ? _baseFontEB : _baseFontDB;
		}

    private static void SetupOriginalFontAssets()
		{
			if (_fontAssetSetup)
			{
				return;
			}

			_baseFontEB = Resources.Load<TMP_FontAsset>(BaseFontEbPath);
			_baseFontDB = Resources.Load<TMP_FontAsset>(BaseFontDbPath);
			TMP_FontAsset dynamicFontEB = Resources.Load<TMP_FontAsset>(DynamicFontEbPath);
			TMP_FontAsset dynamicFontDB = Resources.Load<TMP_FontAsset>(DynamicFontDbPath);
			AddFallbackFontAsset(_baseFontEB, dynamicFontEB);
			AddFallbackFontAsset(_baseFontDB, dynamicFontDB);
			_fontAssetSetup = true;
		}

		private static void AddFallbackFontAsset(TMP_FontAsset fontAsset, TMP_FontAsset fallbackFontAsset)
		{
			if (fontAsset == null || fallbackFontAsset == null)
			{
				return;
			}

			if (fontAsset.fallbackFontAssetTable == null)
			{
				fontAsset.fallbackFontAssetTable = new List<TMP_FontAsset>();
			}

			if (!fontAsset.fallbackFontAssetTable.Contains(fallbackFontAsset))
			{
				fontAsset.fallbackFontAssetTable.Add(fallbackFontAsset);
			}
		}

    public static void SetAnchor(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPosition, Vector2 sizeDelta)
		{
			rect.anchorMin = anchorMin;
			rect.anchorMax = anchorMax;
			rect.pivot = pivot;
			rect.anchoredPosition = anchoredPosition;
			rect.sizeDelta = sizeDelta;
		}

		public static void ResizeTextWidth(TextMeshProUGUI text)
		{
			float width = text.preferredWidth;

			LayoutElement layout = text.GetComponent<LayoutElement>();
			layout.minWidth = width;
			layout.preferredWidth = width;
			layout.flexibleWidth = 0;

			LayoutRebuilder.ForceRebuildLayoutImmediate(text.transform as RectTransform);
			LayoutRebuilder.ForceRebuildLayoutImmediate(text.transform.parent as RectTransform);
		}
  }
}