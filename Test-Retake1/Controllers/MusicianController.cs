using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test_Retake1.Services;

namespace Test_Retake1.Controllers
{
    [Route("api/musician")]
    [ApiController]
    public class MusicianController : ControllerBase
    {
        private IDatabaseService _service;

        public MusicianController(IDatabaseService service)
        {
            _service = service;
        }

        [HttpDelete("{idMusician}")]
        public async Task<IActionResult> DeleteMusician([FromRoute] int idMusician)
        {
            if(!await _service.DoesMusicianExistAsync(idMusician))
            {
                return NotFound("Musician does not exist");
            }
            if(await _service.CanMusicianBeDeleted(idMusician))
            {
                return BadRequest("Musician has songs that are in albums");
            }
            await _service.DeleteMusicianAsync(idMusician);
            return NoContent();
        }
    }
}
