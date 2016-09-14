using SalaryLocator.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryLocator.Logic.Services
{
    public class OccupationService
    {
        private readonly MSADbContext _dbContext;

        public OccupationService(MSADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IOrderedQueryable<Occupation> GetOccupations(int areaCode)
        {
            if (areaCode <= 0)
                throw new ArgumentOutOfRangeException(nameof(areaCode));

            var occupations = from salary in _dbContext.Salaries
                              join occupation in _dbContext.Occupations
                              on salary.OccupationCode equals occupation.Code
                              where salary.AreaCode == areaCode && occupation.GroupType != 1 && occupation.GroupType != 2
                              select occupation;
            // OrderBy clause is separate because query expression doesn't support IOrderedQueryable<T>.
            return occupations.OrderBy(o => o.Title);
        }
    }
}
