using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ConferenceApi.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ConferenceController : ControllerBase
    {
        private readonly IConferenceService _conferenceService;
        public ConferenceController(IConferenceService conferenceService)
        {
            _conferenceService = conferenceService;
        }

        // GET: api/v1/ConferenceApi/ 
        //or 
        //  GET: api/v1/ConferenceApi/SpeakerAndSessions
       
        [HttpGet("speakersandsessions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SpeakersAndSessionsAsync(string speakerName = null, string dayno = null, string keyword = null)
        {
            return new OkObjectResult(await _conferenceService.GetSpeakersAndSessionsAsync(speakerName, dayno, keyword));
        }

        // GET: api/v1/ConferenceApi/session/4
        [HttpGet("session/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            if (id < 0)
                return BadRequest();

            return new OkObjectResult(await _conferenceService.GetSessionByIdAsync(id));
        }

    }
}
