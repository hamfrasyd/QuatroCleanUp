﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QuatroCleanUpBackend.Models;
using QuatroCleanUpBackend.Repos;

namespace QuatroCleanUpApi.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        //ControllerBase uses Microsoft.AspNetCore.Mvc;

        private readonly EventRepository _eventRepository;

        private readonly ILogger<EventController> _logger;

        public EventController(EventRepository eventRepository, ILogger<EventController> logger) //Dependency Inject the repository class
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var eventList = await _eventRepository.GetAllAsync();

                if (eventList is null || eventList.Count == 0)
                {
                    return NoContent(); //204  - kan også bruge NotFound(); 404
                }
                else
                {
                    return Ok(eventList);
                }
            }
            catch(SqlException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var e = await _eventRepository.GetByIdAsync(id);
                return Ok(e);
            }
            catch (KeyNotFoundException keyNotFoundEx)
            {
                _logger.LogError(keyNotFoundEx.Message, "Error GetById event");
                return NotFound(keyNotFoundEx.Message);
            }
            catch (SqlException ex) //overvej at gøre til DbException?
            {
                _logger.LogError(ex.Message, "Error GetById event");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error GetById event");
                return BadRequest(ex.Message);
            }
        }


        //[HttpGet]
        //[Route("SortByDateAscending")]
        //Er det en metode vi vil have?

        //[HttpGet]
        //[Route("SortByDateDescending")]
        //Er det en metode vi vil have?

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Event newEvent)
        {
            try
            {
                Event e = await _eventRepository.CreateEventAsync(newEvent);
                return Created("api/Events" + e.EventId, e);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex.Message, "Error fetching event");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error fetching event");
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("{id}")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] Event eventUpdate)
        {
            try
            {
                Event newEventUpdate = await _eventRepository.UpdateEventAsync(eventUpdate);
                if (eventUpdate == null)
                {
                    return NotFound($"Event with ID {id} not found.");
                }
                return Ok(newEventUpdate);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message, "Error update event");
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex.Message, "Error update event");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error update event");
                return BadRequest(ex.Message);
            }

        }

        //[HttpPut]
        //[Route("{id/status}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> Put(int id, [FromBody] int newStatusId)
        //{
        //    try
        //    {

            
        //        var @event = await _eventRepository.GetByIdAsync(id);
        //        if (@event == null) return NotFound();

        //        @event.StatusId = newStatusId;
        //        await _eventRepository.UpdateEventStatusAsync(id, newStatusId);

        //        if (newStatusId == 3) // Completed
        //        {
        //            var attendees = await _eventRepository.EventAttendances
        //                .Where(ea => ea.EventId == id)
        //                .Select(ea => ea.UserId)
        //                .ToListAsync();

        //            var updates = new List<Dictionary<string, object>>();
        //            foreach (var userId in attendees)
        //            {
        //                var user = await _eventRepository.GetByIdAsync(userId);
        //                if (user != null && !string.IsNullOrEmpty(user.PlayFabPlayerId))
        //                {
        //                    updates.Add(new Dictionary<string, object>
        //            {
        //                { "PlayerId", user.PlayFabPlayerId },
        //                { "Amount", @event.TrashCollected }
        //            });
        //                }
        //            }

        //            var request = new ExecuteCloudScriptServerRequest
        //            {
        //                FunctionName = "AddTrashCollectedBatch",
        //                FunctionParameter = new { Updates = updates },
        //                GeneratePlayStreamEvent = true
        //            };

        //            await PlayFabServerAPI.ExecuteCloudScriptAsync(request);
        //        }

        //        return Ok();
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        _logger.LogError(ex.Message, "Error update event");
        //        return BadRequest(ex.Message);
        //    }
        //    catch (SqlException ex)
        //    {
        //        _logger.LogError(ex.Message, "Error update event");
        //        return BadRequest(ex.Message);
        //    }
        //    catch(Exception ex)
        //    {
        //        _logger.LogError(ex.Message, "Error update event");
        //        return BadRequest(ex.Message);
        //    }       

        //}

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var returnEvent = await _eventRepository.DeleteEventAsync(id);
                return Ok(returnEvent);
            }
            catch(ArgumentNullException ex)
            {
                _logger.LogError(ex.Message, "Error on delete event");
                return NotFound(ex.Message);
            }
            catch(SqlException ex)
            {
                _logger.LogError(ex.Message, "Error on delete event");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Error on delete event.");
                return BadRequest(ex.Message);

            }
        }


    }
}
