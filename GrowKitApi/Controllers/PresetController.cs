using GrowKitApi.Contexts;
using GrowKitApi.Entities;
using GrowKitApiDTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GrowKitApi.Controllers
{
    /// <summary> The api endpoint for preset related methods.</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PresetController : ControllerBase
    {
        private readonly ApplicationContext _appContext;

        // inject the dependencies
        public PresetController(ApplicationContext appContext)
        {
            _appContext = appContext;
        }

        /// <summary> Gets the preset values fromt rhe database</summary>
        /// <param name="presetId"> the preset id of the preset to fetch.</param>
        [HttpGet("{presetId}")]
        public async Task<IActionResult> GetPresetValues(int presetId)
        {
            var preset = await _appContext.PlantPresets.FindAsync(presetId);

            if (preset == null)
                return NotFound();

            return Ok(new PresetDTO()
            {
                Light = preset.Light,
                Moisture = preset.Moisture,
                Sunshine = preset.Sunshine,
                Temperature = preset.Temperature
            });
        }

        /// <summary> Sets the preset values in the database.</summary>
        /// <param name="presetId"> The preset id of the preset to update.</param>
        /// <param name="presetDTO"> The new values to insert into the preset.</param>
        /// <remarks> This method should be locked behind authorization making sure 
        /// only preset administrators can edit the values</remarks>
        [HttpPut("Set/{presetId}")]
        public async Task<IActionResult> SetPresetValues(int presetId, [FromBody] PresetDTO presetDTO)
        {
            bool createdNew = false;

            var preset = await _appContext.PlantPresets.FindAsync(presetId);

            if (preset == null)
            {
                preset = new PlantPreset();
                _appContext.PlantPresets.Add(preset);
                createdNew = true;
            }

            preset.Light = presetDTO.Light;
            preset.Moisture = presetDTO.Moisture;
            preset.Sunshine = presetDTO.Sunshine;
            preset.Temperature = presetDTO.Temperature;

            if (createdNew)
                return CreatedAtAction("set", preset);
            else
                return NoContent();
        }
    }
}