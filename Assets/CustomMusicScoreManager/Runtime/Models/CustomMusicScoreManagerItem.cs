using System;
using Sekai.MusicScoreMaker.Common;

namespace Sekai.CustomMusicScoreManager
{
	public sealed class CustomMusicScoreManagerItem
	{
		public CustomMusicScoreEntry Entry { get; }

		public DateTime LastWriteTime { get; }

		public bool HasManifest { get; }

		public bool HasScore { get; }

		public bool HasAudio { get; }

		public bool HasJacket { get; }

		public string StatusText
		{
			get
			{
				if (!HasManifest)
				{
					return "設定がありません";
				}
				if (!HasScore)
				{
					return "譜面がありません";
				}
				if (!HasAudio)
				{
					return "音声がありません";
				}
				if (!HasJacket)
				{
					return "ジャケットがありません";
				}
				return "準備完了";
			}
		}

		public bool IsReadyForEdit => HasManifest;

		public CustomMusicScoreManagerItem(
			CustomMusicScoreEntry entry,
			DateTime lastWriteTime,
			bool hasManifest,
			bool hasScore,
			bool hasAudio,
			bool hasJacket)
		{
			Entry = entry;
			LastWriteTime = lastWriteTime;
			HasManifest = hasManifest;
			HasScore = hasScore;
			HasAudio = hasAudio;
			HasJacket = hasJacket;
		}
	}
}
