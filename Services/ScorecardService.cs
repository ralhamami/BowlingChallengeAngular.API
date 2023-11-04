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
        private readonly ILogger<ScorecardService> logger;

        public bool GameOver
        {
            get
            {
                return _gameOver;
            }
        }

        public List<Frame> Frames { get; set; }= new List<Frame>();

        public ScorecardService(ILogger<ScorecardService> logger)
        {
            InitializeScorecard();
            this.logger = logger;
        }
        public void AddScore(int pinsKnockedDown)
        {
            try
            {
                //Call the frame's AddScore so it can handle keeping its own score.
                Frames[_currentFrame].AddScore(pinsKnockedDown);

                //If we're in the last frame and the frame is marked as complete, signal game over.
                if (Frames[_currentFrame] is LastFrame && Frames[_currentFrame].IsComplete)
                {
                    _gameOver = true;
                }

                //If the current frame is complete and it's not game over,
                //then set the new current frame.
                if (Frames[_currentFrame].IsComplete && !GameOver)
                {
                    Frames[_currentFrame].IsCurrentFrame = false;
                    _currentFrame++;
                    Frames[_currentFrame].IsCurrentFrame = true;
                }

                RecalculateScore();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        public void RecalculateScore()
        {
            try
            {
                //Add additional points for strikes and spares
                for (int i = 0; i < Frames.Count; i++)
                {
                    //Only calculate the score if the frame is complete
                    //and is not an open frame (since they don't need any special calculation)
                    //and the score is not marked final yet (as we wouldn't want to keep adding
                    //additional points to strikes/spares we already awarded).
                    if (Frames[i].IsComplete && !Frames[i].IsOpenFrame && !Frames[i].IsScoreFinal)
                    {
                        //Calculate additional points by determining how many additional shots
                        //we should add the score of.
                        int additionalPoints = 0;
                        int additionalShotsToTake = Frames[i].IsStrike ? 2 : Frames[i].IsSpare ? 1 : 0;

                        //Then get the values of those additional shots, and add them to the respective
                        //frame, but ONLY after we've moved enough shots forward to do so.
                        var additionalShots = Frames.Skip(i + 1).SelectMany(f => f.Shots).Where(s => s.HasValue).Take(additionalShotsToTake);

                        if (additionalShots.Count() == additionalShotsToTake)
                        {
                            additionalPoints = additionalShots.Sum().GetValueOrDefault();
                            Frames[i].IsScoreFinal = true;
                        }

                        Frames[i].FrameTotalScore += additionalPoints;
                    }
                }

                //Calculate running totals for the bottom of each frame.
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
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        public void ValidateShot(int pinsKnockedDown, ModelStateDictionary modelState)
        {
            try
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
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        private void InitializeScorecard()
        {
            try
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
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        public void ResetScorecard()
        {
            InitializeScorecard();
        }
    }
}
