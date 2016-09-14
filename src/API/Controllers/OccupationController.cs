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
    public class OccupationController : Controller
    {
        private readonly OccupationService _occupationService = null;

        public OccupationController(OccupationService occupationService)
        {
            _occupationService = occupationService;
        }

        [HttpGet("{areaCode}")]
        public async Task<IActionResult> Get(int areaCode)
        {
            var occupations = await _occupationService.GetOccupations(areaCode).ToListAsync();
            if (occupations == null || !occupations.Any())
                return NotFound();
            return Ok(occupations);
        }
    }
}
