using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test_Retake1.Services;

namespace Test_Retake1.Controllers
{
    [Route("api/album")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private IDatabaseService _service;

        public AlbumController(IDatabaseService service)
        {
            _service = service;
        }

        [HttpGet("{idAlbum}")]
        public async Task<IActionResult> GetAlbum([FromRoute] int idAlbum)
        {
            if(!await _service.DoesAlbumExistAsync(idAlbum))
            {
                return NotFound("Album does not exist");
            }

            return Ok( await _service.GetAlbum(idAlbum));

        }
    }
}
