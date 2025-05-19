
using QuatroCleanUpApi;
using Microsoft.AspNetCore.Mvc;
using QuatroCleanUpBackend;
using Microsoft.AspNetCore.Http.HttpResults;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuatroCleanUpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        // GET: api/<UserController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get()
        {
            var userList = await _userRepository.GetAllAsync();

            if (userList.Count == 0 || userList is null)
            {
                return NoContent();
            }
            else
            {
                return Ok();
            }
        }

        // GET api/<UserController>/5
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var e = await _userRepository.GetUserByIdAsync(id);
                return Ok(e);
            }
            catch (KeyNotFoundException KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST api/<UserController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] User newUser)
        {
            try
            {
                User e = await _userRepository.CreateUserAsync(newUser);
                return Created("api/Users" + e.UserId, e);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<UserController>/5
        [HttpPut]
        [Route("{UserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, User userUpdate)
        {
            try
            {
                User newUserUpdate = await _userRepository.UpdateUserAsync(userUpdate);
                return Ok(newUserUpdate);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException KeyNotFoundException) { return NotFound(); }
        }

        // DELETE api/<UserController>/5
        [HttpDelete]
        [Route("{UserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var returnUser = await _userRepository.DeleteUserAsync(id);
                return Ok(returnUser);
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}