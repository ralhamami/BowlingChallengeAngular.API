using BowlingChallengeAngular.API.Controllers;
using BowlingChallengeAngular.API.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BowlingChallengeAngular.API.Services
{
    public class ScorecardService : IScorecardService
    {
        private int _currentFrame;
        public int CurrentFrame
        {
            get
            {
                return _currentFrame;
            }
        }


        private bool _gameOver;
        public bool GameOver
        {
            get
            {
                return _gameOver;
            }
        }

        public List<Frame> Frames { get; set; }

        public ScorecardService()
        {
            InitializeScorecard();
        }
        public void AddScore(int pinsKnockedDown)
        {
            Frames[_currentFrame].AddScore(pinsKnockedDown);

            if (Frames[_currentFrame] is LastFrame && Frames[_currentFrame].IsComplete)
            {
                _gameOver = true;
            }

            if (Frames[_currentFrame].IsComplete && !GameOver)
            {
                Frames[_currentFrame].IsCurrentFrame = false;
                _currentFrame++;
                Frames[_currentFrame].IsCurrentFrame = true;
            }

            RecalculateScore();
        }

        public void RecalculateScore()
        {
            //Add additional points for strikes and spares
            for (int i = 0; i < Frames.Count; i++)
            {
                if (Frames[i].IsComplete && !Frames[i].IsOpenFrame && !Frames[i].IsScoreFinal)
                {
                    //Calculate additional points.
                    int additionalPoints = 0;
                    int additionalShotsToTake = Frames[i].IsStrike ? 2 : Frames[i].IsSpare ? 1 : 0;

                    var additionalShots = Frames.Skip(i + 1).SelectMany(f => f.Shots).Where(s => s.HasValue).Take(additionalShotsToTake);

                    if (additionalShots.Count() == additionalShotsToTake)
                    {
                        additionalPoints = additionalShots.Sum().GetValueOrDefault();
                        Frames[i].IsScoreFinal = true;
                    }

                    Frames[i].FrameTotalScore += additionalPoints;
                }
            }

            //Calculate running totals
            int runningTotal = 0;

            foreach (var frame in Frames)
            {
                if (frame.IsComplete)
                {
                    runningTotal += frame.FrameTotalScore.GetValueOrDefault();
                    frame.TotalScore = runningTotal;
                }
            }
        }

        public void ValidateShot(int pinsKnockedDown, ModelStateDictionary modelState)
        {
            if (pinsKnockedDown > 10 || pinsKnockedDown < 0)
            {
                modelState.AddModelError("Range", "Input must be between 0 and 10.");
            }
            int sum = Frames[_currentFrame].Shots.Where(s => s.HasValue).Sum(s => s.Value) + pinsKnockedDown;

            if (sum > Frames[_currentFrame].MaxPins || sum < 0)
            {
                modelState.AddModelError("Total", $"Input cannot result in frame being more than {Frames[_currentFrame].MaxPins}.");
            }
        }

        private void InitializeScorecard()
        {
            _gameOver = false;
            Frames = new List<Frame>(10);
            _currentFrame = 0;
            for (int i = 0; i < 9; i++)
            {
                Frames.Add(new NormalFrame());
            }
            Frames.Add(new LastFrame());
            Frames[_currentFrame].IsCurrentFrame = true;
        }

        public void ResetScorecard()
        {
            InitializeScorecard();
        }
    }
}
