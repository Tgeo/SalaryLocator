using SalaryLocator.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryLocator.Logic.Services
{
    public class LocationService
    {
        private readonly MSADbContext _dbContext;

        public LocationService(MSADbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IOrderedQueryable<State> GetStates()
        {
            return _dbContext.States.OrderBy(s => s.Name);
        }

        public IOrderedQueryable<Area> GetAreas(string stateCode)
        {
            if (string.IsNullOrWhiteSpace(stateCode))
                throw new ArgumentOutOfRangeException(nameof(stateCode));
            return _dbContext.Areas
                .Where(a => a.PrimaryStateCode == stateCode)
                .OrderBy(a => a.Name);
        }
    }
}
