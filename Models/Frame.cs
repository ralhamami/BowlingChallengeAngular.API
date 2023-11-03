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

        //public Frame(bool isLastFrame = false)
        //{
        //    IsLastFrame = isLastFrame;
        //    if (isLastFrame)
        //    {
        //        Shots.Add(null);
        //        MAX_PINS = 30;
        //    }
        //}

        // You can include common methods or logic here
        public abstract void AddScore(int score);
        //{
        //    int nextEmpty = Shots.IndexOf(null);
        //    Shots[nextEmpty] = score;

        //    EvaluateFrame();
        //    return true;
        //}

        public abstract void EvaluateFrame();
        //{
        //    if (Shots[0].HasValue && Shots[0].Value == 10)
        //    {
        //        IsStrike = true;
        //    }
        //    else if (Shots[0].GetValueOrDefault() + Shots[1].GetValueOrDefault() == 10)
        //    {
        //        IsSpare = true;
        //    }
        //    else if (Shots[1].HasValue)
        //    {
        //        IsOpenFrame = true;
        //        IsScoreFinal = true;
        //    }

        //    FrameTotalScore = Shots.Sum(s=>s.GetValueOrDefault());
        //    IsComplete = Shots[Shots.Count-1].HasValue || (Shots[0].GetValueOrDefault() == 10 && !IsLastFrame);
        //}
    }

}