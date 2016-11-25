using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalaryLocator.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryLocator.API.Controllers
{
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        private readonly LocationService _locationService;

        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("state")]
        public IActionResult GetStates()
        {
            return Ok(_locationService.GetStates());
        }

        [HttpGet("area")]
        public async Task<IActionResult> GetAreas(string stateCode)
        {
            var areas = await _locationService.GetAreas(stateCode).ToListAsync();
            if (areas == null || !areas.Any())
                return NotFound();
            return Ok(areas);
        }

        [HttpGet("area/{occupationCode}")]
        public async Task<IActionResult> GetAreasWithOccupation(string occupationCode)
        {
            var areas = await _locationService.GetAreasWithOccupation(occupationCode).ToListAsync();
            if (areas == null || !areas.Any())
                return NotFound();
            return Ok(areas);
        }

        [HttpGet("area.HighestSalaries")]
        public async Task<IActionResult> GetAreasWithHighestSalaries(string occupationCode, bool adjustForCostOfLiving = false)
        {
            var areas = adjustForCostOfLiving ?
                await _locationService.GetAreasWithHighestSalariesAdjustForCostOfLivingAsync(occupationCode) :
                await _locationService.GetAreasWithHighestSalariesAsync(occupationCode);
            if (areas == null || !areas.Any())
                return NotFound();
            return Ok(areas);
        }
    }
}
