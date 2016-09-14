using Microsoft.EntityFrameworkCore;
using SalaryLocator.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryLocator.Logic.Services
{
    public class SalaryService
    {
        private readonly MSADbContext _dbContext;

        public SalaryService(MSADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<SalaryRecord> GetRecords()
        {
            return _dbContext.SalaryRecords;
        }

        public async Task<Salary> GetSalaryRecordAsync(int areaCode, string occupationCode)
        {
            if (areaCode <= 0)
                throw new ArgumentOutOfRangeException(nameof(areaCode));
            if (string.IsNullOrWhiteSpace(occupationCode))
                throw new ArgumentOutOfRangeException(nameof(occupationCode));
            
            return await _dbContext.Salaries.FirstOrDefaultAsync(s =>
                s.AreaCode == areaCode && s.OccupationCode == occupationCode);
        }
    }
}
