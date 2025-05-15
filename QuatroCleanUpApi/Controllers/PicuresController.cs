using Microsoft.AspNetCore.Mvc;
using QuatroCleanUpBackend;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuatroCleanUpApi.Controllers
{
    [Route("api/pictures")]
    [ApiController]
    public class PicuresController : ControllerBase
    {

        private readonly PicturesRepository _repo;
        public PicuresController(PicturesRepository repo)
        {
            _repo = repo;
        }


        // GET: api/<PicuresController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAsync()
        {
            List<Picture> pictureList = await _repo.GetAllAsync();
            if (pictureList.Count == 0)
            {
                return NoContent(); //204 - kan også bruge NotFound(); 404
            }
            else
            {
                return Ok(pictureList); //ok returnere 200
            }
        }

        // GET api/<PicuresController>/5
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                Picture p = await _repo.GetByIdAsync(id);
                return Ok(p);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); //404  
            }
        }

        // POST api/<PicuresController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostAsync([FromBody]Picture picture)
        {
            try
            {
                Picture p = await _repo.AddAsync(picture);
                return Created("api/pictures/" + p.PictureId, p);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // DELETE api/<PicuresController>/5
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                Picture p = await _repo.DeleteAsync(id);
                return Ok(p);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); 
            }
        }
    }
}
