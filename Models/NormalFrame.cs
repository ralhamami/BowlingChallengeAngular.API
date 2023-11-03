namespace BowlingChallengeAngular.API.Models
{
    public class NormalFrame : Frame
    {
        public override List<int?> Shots { get; set; } = new List<int?> { null, null };

        //A normal frame can never have more than 10 pins knocked down.
        public override int MaxPins => 10;

        public override void AddScore(int score)
        {
            //Get index of next empty shot slot, and add the shot.
            int nextEmpty = Shots.IndexOf(null);
            Shots[nextEmpty] = score;

            EvaluateFrame();
        }

        public override void EvaluateFrame()
        {
            //If the first shot is 10, it's a strike.
            if (Shots[0].HasValue && Shots[0].Value == 10)
            {
                IsStrike = true;
            }
            //If the sum of the shots is 10, it's a spare.
            else if (Shots[0].GetValueOrDefault() + Shots[1].GetValueOrDefault() == 10)
            {
                IsSpare = true;
            }
            //Otherwise, it's an open frame, and the score is final since no additional points awarded.
            else if (Shots[1].HasValue)
            {
                IsOpenFrame = true;
                IsScoreFinal = true;
            }

            FrameTotalScore = Shots.Sum(s => s.GetValueOrDefault());
            
            //Frame is considered complete if first shot is a strike, or if second shot has a value.
            IsComplete = Shots[Shots.Count - 1].HasValue || (Shots[0].GetValueOrDefault() == 10);
        }
    }
}
