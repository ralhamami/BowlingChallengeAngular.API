namespace BowlingChallengeAngular.API.Models
{
    public class LastFrame : Frame
    {
        public override List<int?> Shots { get; set; } = new List<int?> { null, null, null };

        //The last frame can have up to 30 pins knocked down if all strikes.
        public override int MaxPins => 30;

        public override void AddScore(int score)
        {
            //Get index of next empty shot slot, and add the shot.
            int nextEmpty = Shots.IndexOf(null);
            Shots[nextEmpty] = score;

            EvaluateFrame();
        }

        public override void EvaluateFrame()
        {
            FrameTotalScore = Shots.Sum(s => s.GetValueOrDefault());
            
            //Last frame is considered complete if the third shot slot has a value,
            //or if the first two shots don't add up to at least 10, meaning no bonus shot.
            IsComplete = Shots[Shots.Count - 1].HasValue || (Shots.Count(s=>s.HasValue) == 2 && Shots.Take(2).Sum(s=>s.GetValueOrDefault())<10);

            //Score is considered final in last frame since additional points aren't awarded.
            IsScoreFinal = IsComplete;
        }
    }
}
