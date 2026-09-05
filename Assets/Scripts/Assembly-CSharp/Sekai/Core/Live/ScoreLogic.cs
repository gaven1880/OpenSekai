using System.Linq;
using Sekai.Live;
using UnityEngine;

namespace Sekai.Core.Live
{
	public class ScoreLogic
	{
		private readonly LiveBundleBuildData liveBundleBuildData;

		private MasterPlayLevelScore scoreInfo;

		private float dropOutFactor = 0.7f;

		public float totalScoreF;

		public LiveScore score;

		public float BaseNoteScore { get; set; }

		public bool IsPerfectCombo => score.badCount == 0 && score.missCount == 0 && score.goodCount == 0;

		public bool IsAllPerfectCombo => score.IsAllPerfect;

		public ScoreLogic(LiveBundleBuildData data)
		{
			liveBundleBuildData = data;
			score.life = 1000;
			score.rank = ScoreRank.D;
		}

		public virtual void Setup(LiveBootDataBase bootData, MusicScore musicScore)
		{
			score = default;
			scoreInfo = bootData?.MusicData?.Score;
			int totalCombo = LiveUtility.CalculateTotalComboCount(musicScore);
			if (totalCombo <= 0)
			{
				totalCombo = bootData?.MusicData?.TotalNoteCount ?? 0;
			}
			score.totalComboCount = totalCombo;
			score.life = liveBundleBuildData != null && liveBundleBuildData.Life > 0 ? liveBundleBuildData.Life : 1000;
			score.rank = ScoreRank.D;
			float scoreWeight = musicScore.NoteArray.Sum(note => GetNoteScoreFactor(note));
			float playLevelFactor = 4 + ((Mathf.Clamp(scoreInfo.playLevel, 5, 40) - 5) * 0.02f);
			BaseNoteScore = totalCombo > 0 ? bootData.DeckData.TotalPowerIncludeBuff / scoreWeight * playLevelFactor : 0;
		}

		public virtual void ExcuteEvent(EventBase eventBase)
		{
		}

		private float GetNoteScoreFactor(NoteBase note)
		{
			bool critical = note.Type == NoteType.Critical;

			return note.Category switch
			{
				NoteCategory.Normal => critical ? 2f : 1f,
				NoteCategory.Long => critical ? 2f : 1f,
				NoteCategory.Connection => critical ? 0.2f : 0.1f,
				NoteCategory.Flick => critical ? 3f : 1f,
				NoteCategory.Friction => critical ? 0.2f : 0.1f,
				NoteCategory.FrictionLong => critical ? 0.2f : 0.1f,
				NoteCategory.FrictionFlick => critical ? 3f : 1f,
				NoteCategory.Combo => 0.1f,
				_ => 0f,
			};
		}

		public virtual void UpdateCombo(NoteBase note)
		{
			if (note == null || note.Result == NoteResult.None)
			{
				return;
			}

			score.combo = note.Result < NoteResult.Great ? 0 : score.combo + 1;
			if (score.combo > score.maxCombo)
			{
				score.maxCombo = score.combo;
			}
		}

		public virtual void UpdateNoteResult(NoteBase note)
		{
			switch (note?.Result)
			{
				case NoteResult.JustPerfect:
					score.justPerfectCount++;
					score.perfectCount++;
					break;
				case NoteResult.Perfect:
					score.perfectCount++;
					break;
				case NoteResult.Great:
					score.greatCount++;
					break;
				case NoteResult.Good:
					score.goodCount++;
					break;
				case NoteResult.Auto:
					score.autoCount++;
					break;
				case NoteResult.Bad:
					score.badCount++;
					break;
				case NoteResult.Miss:
					score.missCount++;
					break;
			}

			if (note != null && note.Description == NoteResultDescription.Fast)
			{
				score.fastCount++;
			}
			else if (note != null && note.Description == NoteResultDescription.Late)
			{
				score.lateCount++;
			}
			else if (note != null && note.Description == NoteResultDescription.FlickMiss)
			{
				score.flickCount++;
			}
		}

		public virtual int CalculateAddScore(NoteBase note, float factor = 1f)
		{
			if (note == null)
			{
				return 0;
			}

			float noteScoreFactor = GetNoteScoreFactor(note);
			float comboScoreFactor = 1f + (Mathf.FloorToInt(Mathf.Max(0, Mathf.Clamp(score.combo, 0, 1001) - 1) / 100f) * 0.01f);
			float resultScoreFactor = note.Result switch
			{
				NoteResult.JustPerfect => 1f,
				NoteResult.Perfect => 1f,
				NoteResult.Great => 0.7f,
				NoteResult.Good => 0.5f,
				NoteResult.Auto => 0.7f,
				_ => 0f,
			};

			float addScore = BaseNoteScore * noteScoreFactor * comboScoreFactor * resultScoreFactor;
			if (score.life == 0)
			{
				addScore *= dropOutFactor;
			}
			totalScoreF += addScore;
			score.totalScore = (int)totalScoreF;
			UpdateScoreRank();

			return (int)addScore;
		}

		public virtual void Damage(NoteBase note)
		{
			if (note == null)
			{
				return;
			}

			if (LiveConfig.Damages.TryGetValue(note.Result, out int damage))
			{
				score.life = Mathf.Max(0, score.life - damage);
			}
		}

		public virtual void UpdateScoreRank()
		{
			if (scoreInfo != null && (scoreInfo.s > 0 || scoreInfo.a > 0 || scoreInfo.b > 0 || scoreInfo.c > 0))
			{
				score.rank = ScoreGaugeCalculator.GetScoreRank(scoreInfo, score.totalScore);
				return;
			}
		}
	}
}