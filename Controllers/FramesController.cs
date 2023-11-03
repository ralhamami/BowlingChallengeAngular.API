using BowlingChallengeAngular.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BowlingChallengeAngular.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FramesController : ControllerBase
    {
        private readonly IScorecardService scorecardService;

        public FramesController(IScorecardService scorecardService)
        {
            this.scorecardService = scorecardService;
        }

        //Get all frames
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Frame>>> GetFrames() 
        {
            return Ok(scorecardService.Frames);
        }

        //Post score and get frames
        [HttpGet("{pinsKnockedDown}")]
        public async Task<ActionResult<IEnumerable<Frame>>> AddShot(int pinsKnockedDown) 
        {
            if (!scorecardService.GameOver)
            {
                scorecardService.ValidateShot(pinsKnockedDown, ModelState);

                if (ModelState.IsValid)
                {
                    scorecardService.AddScore(pinsKnockedDown);

                    return Ok(scorecardService.Frames);
                }
            }
            else
            {
                ModelState.AddModelError("Game Over", "Game over. Please reset.");
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("reset")]
        public async Task<ActionResult<IEnumerable<Frame>>> ResetFrames()
        {
            scorecardService.ResetScorecard();
            return Ok(scorecardService.Frames);
        }
    }
}
