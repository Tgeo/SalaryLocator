using Microsoft.AspNetCore.Mvc;
using SalaryLocator.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryLocator.API.Controllers
{
    [Route("api/[controller]")]
    public class SalaryController : Controller
    {
        private readonly SalaryService _salaryService = null;

        public SalaryController(SalaryService salaryService)
        {
            _salaryService = salaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int areaCode, string occupationCode)
        {
            var salaryRecord = await _salaryService.GetSalaryRecordAsync(areaCode, occupationCode);
            if (salaryRecord == null)
                return NotFound();
            return Ok(salaryRecord);
        }
    }
}
