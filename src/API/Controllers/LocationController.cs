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
        public IActionResult Get()
        {
            return Ok(_locationService.GetStates());
        }

        [HttpGet("area/{stateCode}")]
        public async Task<IActionResult> Get(string stateCode)
        {
            var areas = await _locationService.GetAreas(stateCode).ToListAsync();
            if (areas == null || !areas.Any())
                return NotFound();
            return Ok(areas);
        }
    }
}
