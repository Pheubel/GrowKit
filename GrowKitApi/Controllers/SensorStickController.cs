using GrowKitApi.Contexts;
using GrowKitApi.Services;
using GrowKitApiDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GrowKitApi.Controllers
{
    /// <summary> The api endpoint for sensorstick related methods.</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SensorStickController : ControllerBase
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IUserManagementService _userManagementService;

        // injects the services into the controller
        public SensorStickController(ApplicationContext applicationContext, IUserManagementService userManagementService)
        {
            _applicationContext = applicationContext;
            _userManagementService = userManagementService;
        }

        /// <summary> Updates the stick with the message attached to the request.</summary>
        /// <param name="updateMessage"> The message attached to the request.</param>
        [HttpPatch("UpdateStick")]
        public async Task<IActionResult> UpdateStickValue([FromBody] StickMessageDTO<StickUpdateDTO> updateMessage)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var stick = await _applicationContext.SensorSticks.FindAsync(updateMessage.ID_Stick);

            if (stick == null)
            {
                return NotFound();
            }

            // update the values of the stick
            stick.MasterStickId = updateMessage.ID_Master;
            stick.Light = updateMessage.Message.Light;
            stick.LightTime = updateMessage.Message.LightTime;
            stick.Moisture = updateMessage.Message.Moisture;
            stick.Temperature = updateMessage.Message.Temperature;
            stick.TimestampUpdate = updateMessage.Message.Timestamp;

            // store the new values in the database
            await _applicationContext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary> Gets the sticks related to the user requesting the stick.</summary>
        /// <param name="userId"> The id of the owner of the sticks.</param>
        /// <remarks> This endpoint is meant for debugging only, during production this should be removed.
        ///  and the method locked behind authorization should be used instead.</remarks>
        [HttpGet("GetSticks/{userId}")]
        public async Task<IActionResult> GetUserSticks(long userId)
        {
            var sticks = await _applicationContext.SensorSticks.Where(s => s.OwnerId == userId).Select(s => new { s.Id }).ToListAsync();

            return Ok(sticks);
        }

        /// <summary> Gets the sticks related to the user requesting the stick.</summary>
        [HttpGet("GetSticks")]
        [Authorize]
        public async Task<IActionResult> GetUserSticks()
        {
            long userId = _userManagementService.GetUserID(User);

            var sticks = await _applicationContext.SensorSticks.Where(s => s.OwnerId == userId).Select(s => new { s.Id }).ToListAsync();

            return Ok(sticks);
        }

        /// <summary> Gets the information about a specific stick.</summary>
        /// <param name="stickId"> The id of the stick.</param>
        [HttpGet("{stickId}")]
        [Authorize]
        public async Task<IActionResult> GetStick(long stickId)
        {
            var stick = await _applicationContext.SensorSticks.FindAsync(stickId);

            if (stick == null)
                return NotFound();

            if (stick.OwnerId != _userManagementService.GetUserID(User))
                return BadRequest();

            return Ok(new StickUpdateDTO()
            {
                Light = stick.Light,
                LightTime = stick.LightTime,
                Moisture = stick.Moisture,
                Temperature = stick.Temperature,
                Timestamp = stick.TimestampUpdate
            });
        }
    }
}