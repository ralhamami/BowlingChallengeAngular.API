using BowlingChallengeAngular.API.Models;
using BowlingChallengeAngular.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BowlingChallengeAngular.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FramesController : ControllerBase
    {
        private readonly IScorecardService scorecardService;
        private readonly ILogger<FramesController> logger;

        public FramesController(IScorecardService scorecardService, ILogger<FramesController> logger)
        {
            this.scorecardService = scorecardService;
            this.logger = logger;
        }

        //Get all frames.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Frame>>> GetFrames() 
        {
            try
            {
                return Ok(scorecardService.Frames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(500,ex.Message);
            }
        }

        //Post score and get frames.
        [HttpGet("{pinsKnockedDown}")]
        public async Task<ActionResult<IEnumerable<Frame>>> AddShot(int pinsKnockedDown) 
        {
            try
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
            catch(Exception ex)
            {
                logger.LogError(ex,ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //Reset the scorecard.
        [HttpPost]
        [Route("reset")]
        public async Task<ActionResult<IEnumerable<Frame>>> ResetFrames()
        {
            try
            {
                scorecardService.ResetScorecard();
                return Ok(scorecardService.Frames);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
