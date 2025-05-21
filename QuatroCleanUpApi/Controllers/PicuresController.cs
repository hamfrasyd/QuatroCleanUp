using Microsoft.AspNetCore.Mvc;
using QuatroCleanUpBackend;
using QuatroCleanUpBackend.Repos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuatroCleanUpApi.Controllers
{
    [Route("api/pictures")]
    [ApiController]
    public class PicuresController : ControllerBase
    {
        private ILogger _logger;
        private readonly PicturesRepository _repo;
        public PicuresController(PicturesRepository repo, ILogger<PicuresController> logger)
        {
            _repo = repo;
            _logger = logger;

        }


        // GET: api/<PicuresController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAsync()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);

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
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(); //404  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);

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
                _logger.LogError(ex.Message);

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);

            }
        }
    }
}
