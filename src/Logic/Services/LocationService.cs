using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SalaryLocator.Logic.Helpers;
using SalaryLocator.Logic.Models;
using SalaryLocator.Logic.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryLocator.Logic.Services
{
    public class LocationService
    {
        private readonly MSADbContext _dbContext;

        static LocationService()
        {
            DapperConfig.ConfigureColumnNameMapping<AreaSalaryDTO>();
        }

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

        public async Task<IEnumerable<AreaSalaryDTO>> GetAreasWithHighestSalariesAsync(string occupationCode)
        {
            // Not using EF here because of "greatest" in order by clause.
            using (var connection = _dbContext.Database.GetDbConnection())
            {
                const string sql = @"
                    select *
                    from salary
                    inner join area
                        on area.area_code = salary.area_code
                    where occupation_code = @OccupationCode
                    order by greatest(coalesce(annual_med_pct, -1), coalesce(hourly_med_pct * 40 * 52, -1)) desc
                    limit 10
                ";
                return await connection.QueryAsync<AreaSalaryDTO>(sql, new { OccupationCode = occupationCode });
            }
        }
    }
}
