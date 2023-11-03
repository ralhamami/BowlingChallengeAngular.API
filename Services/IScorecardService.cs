using BowlingChallengeAngular.API.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BowlingChallengeAngular.API.Services
{
    public interface IScorecardService
    {
        public bool GameOver { get; }
        List<Frame> Frames { get; set; }
        void AddScore(int pinsKnockedDown);
        void ValidateShot(int pinsKnockedDown, ModelStateDictionary modelState);
        void ResetScorecard();
    }
}