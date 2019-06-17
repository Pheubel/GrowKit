﻿using GrowKitApi.Contexts;
using GrowKitApiDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GrowKitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorStickController : ControllerBase
    {
        private readonly ApplicationContext _applicationContext;

        public SensorStickController(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        [HttpPut("UpdateStick")]
        public async Task<IActionResult> UpdateStickValue([FromBody] StickMessageDTO<StickUpdateDTO> updateMessage)
        {
            if (!ModelState.IsValid)
                return NoContent();

            var stick = await _applicationContext.SensorSticks.FindAsync(updateMessage.ID_Stick);

            if (stick == null)
            {
                stick = new Entities.GrowKitStick();
                _applicationContext.Add(stick);

                stick.MasterStickId = updateMessage.ID_Master;
                stick.Light = updateMessage.Message.Light;
                stick.LightTime = updateMessage.Message.LightTime;
                stick.Moisture = updateMessage.Message.Moisture;
                stick.Temperature = updateMessage.Message.Temperature;
                stick.TimestampUpdate = updateMessage.Message.Timestamp;

                await _applicationContext.SaveChangesAsync();

                return CreatedAtAction("UpdateStick", stick);
            }

            stick.MasterStickId = updateMessage.ID_Master;
            stick.Light = updateMessage.Message.Light;
            stick.LightTime = updateMessage.Message.LightTime;
            stick.Moisture = updateMessage.Message.Moisture;
            stick.Temperature = updateMessage.Message.Temperature;
            stick.TimestampUpdate = updateMessage.Message.Timestamp;

            await _applicationContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("GetSticks")]
        //[Authorize]
        public async Task<IActionResult> GetUserSticks()
        {
            var sticks = await _applicationContext.SensorSticks.Where(s => s.OwnerId == 0).ToListAsync();

            return Ok(sticks);
        }

        [HttpGet("{stickId}")]
        //[Authorize]
        public async Task<IActionResult> GetStick(long stickId)
        {
            var stick = await _applicationContext.SensorSticks.FindAsync(stickId);

            if (stick == null)
                return NotFound();

            return Ok(stick);
        }
    }
}