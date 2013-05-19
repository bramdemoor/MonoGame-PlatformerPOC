namespace PlatformerPOC.Domain
{
    public class Score
    {
        private const int SCORE_WIN = 50;

        private const int PENALTY_DEATH = 25;

        public int Wins { get; private set; }
        public int Deaths { get; private set; }
        public int TotalScore { get; private set; }

        public void Reset()
        {
            Wins = 0;
            Deaths = 0;
            TotalScore = 0;
        }

        public void MarkWin()
        {
            Wins++;
            TotalScore += SCORE_WIN;
        }

        public void MarkDeath()
        {
            Deaths++;
            TotalScore -= PENALTY_DEATH;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1}/{2})", TotalScore, Wins, Deaths);
        }
    }
}