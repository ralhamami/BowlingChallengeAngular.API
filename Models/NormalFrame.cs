namespace BowlingChallengeAngular.API.Models
{
    public class NormalFrame : Frame
    {
        public override List<int?> Shots { get; set; } = new List<int?> { null, null };

        public override int MaxPins => 10;

        public override void AddScore(int score)
        {
            int nextEmpty = Shots.IndexOf(null);
            Shots[nextEmpty] = score;

            EvaluateFrame();
        }

        public override void EvaluateFrame()
        {
            if (Shots[0].HasValue && Shots[0].Value == 10)
            {
                IsStrike = true;
            }
            else if (Shots[0].GetValueOrDefault() + Shots[1].GetValueOrDefault() == 10)
            {
                IsSpare = true;
            }
            else if (Shots[1].HasValue)
            {
                IsOpenFrame = true;
                IsScoreFinal = true;
            }

            FrameTotalScore = Shots.Sum(s => s.GetValueOrDefault());
            IsComplete = Shots[Shots.Count - 1].HasValue || (Shots[0].GetValueOrDefault() == 10);
        }
    }
}
