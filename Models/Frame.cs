using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.Collections.Generic;

namespace BowlingChallengeAngular.API.Models
{
    public abstract class Frame
    {
        public abstract int MaxPins { get; }
        public abstract List<int?> Shots { get; set; }
        public int? FrameTotalScore { get; set; }
        public int? TotalScore { get; set; }
        public bool IsComplete { get; set; }
        public bool IsStrike { get; set; }
        public bool IsSpare { get; set; }
        public bool IsOpenFrame { get; set; }
        public bool IsCurrentFrame { get; set; }
        public bool IsScoreFinal { get; set; }

        public abstract void AddScore(int score);
        public abstract void EvaluateFrame();
    }

}