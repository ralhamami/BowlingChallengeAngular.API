namespace BowlingChallengeAngular.API.Models
{
    public class LastFrame : Frame
    {
        public override List<int?> Shots { get; set; } = new List<int?> { null, null, null };

        public override int MaxPins => 30;

        public override void AddScore(int score)
        {
            int nextEmpty = Shots.IndexOf(null);
            Shots[nextEmpty] = score;

            EvaluateFrame();
        }

        public override void EvaluateFrame()
        {
            FrameTotalScore = Shots.Sum(s => s.GetValueOrDefault());
            IsComplete = Shots[Shots.Count - 1].HasValue || (Shots.Count(s=>s.HasValue) == 2 && Shots.Take(2).Sum(s=>s.GetValueOrDefault())<10);
            IsScoreFinal = IsComplete;
        }
    }
}
