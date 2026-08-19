using System.IO;
using Sekai.MusicScoreMaker.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sekai.CustomMusicScoreManager
{
  public class RowView : MonoBehaviour
	{
    [SerializeField]
    private Image background;

		public Image Background => background;

		public CustomMusicScoreManagerItem Item { get; private set; }

    public Sprite CustomJacketSprite { get; private set; }

		[SerializeField]
    private Image JacketImage;

    [SerializeField]
    private TextMeshProUGUI Title;

    [SerializeField]
    private TextMeshProUGUI ScoreTitle;

    [SerializeField]
    private TextMeshProUGUI Difficulty;

    [SerializeField]
    private TextMeshProUGUI Level;

    [SerializeField]
    private TextMeshProUGUI Status;

    public void Setup(CustomMusicScoreManagerItem item)
    {
      Item = item;

      CustomMusicScoreEntry entry = item.Entry;
      CustomMusicScoreManifest manifest = entry.Manifest;

      if (item.HasJacket)
      {
        byte[] bytes = File.ReadAllBytes(entry.JacketPath);

        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);

        texture.LoadImage(bytes);

        CustomJacketSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        JacketImage.sprite = CustomJacketSprite;
      }

      Title.text = manifest.title;
      ScoreTitle.text = manifest.scoreTitle;

      Difficulty.text = manifest.musicDifficultyType.ToUpper();
      Difficulty.color = ColorUtility.GetDifficultyColor(manifest.musicDifficultyType);
      Level.text = $"Lv.{manifest.playLevel}";

      Status.text = item.StatusText;

      UI.ResizeTextWidth(Difficulty);
      UI.ResizeTextWidth(Level);
      UI.ResizeTextWidth(Status);
    }

    public void Unload()
    {
      Destroy(CustomJacketSprite);
      Destroy(gameObject);
    }
	}
}