using Microsoft.AspNetCore.Mvc;
using QuatroCleanUpBackend;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuatroCleanUpApi.Controllers
{
    [Route("api/PicturesController")]
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
        public IActionResult Get()
        {
            List<Picture> pictureList = _repo.GetAll();
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
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PicuresController>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<PicuresController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<PicuresController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
