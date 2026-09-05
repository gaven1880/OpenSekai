using Sekai.Live;

namespace Sekai.Core.Live
{
	public class TechnicScoreLogic : ScoreLogic
	{
		public TechnicScoreLogic(LiveBundleBuildData data)
			: base(data)
		{
		}

		public float GetScoreValueForm(int type)
		{
			return BaseNoteScore;
		}

		public void SetupScore(int baseScore)
		{
			BaseNoteScore = baseScore;
		}
	}
}
