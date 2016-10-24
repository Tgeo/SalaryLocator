using Dapper;
using Microsoft.EntityFrameworkCore;
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

        public IOrderedQueryable<Area> GetAreas()
        {
            return _dbContext.Areas
                .OrderBy(a => a.PrimaryStateCode)
                .ThenBy(a => a.Name);
        }

        public IOrderedQueryable<Area> GetAreas(string stateCode)
        {
            if (string.IsNullOrWhiteSpace(stateCode))
                throw new ArgumentOutOfRangeException(nameof(stateCode));
            return _dbContext.Areas
                .Where(a => a.PrimaryStateCode == stateCode)
                .OrderBy(a => a.Name);
        }

        /// <summary>
        /// Gets every <see cref="Area"/> that has occupations for <paramref name="occupationCode"/>.
        /// </summary>
        /// <remarks>
        /// For example, a small town might not have an CEO positions available.
        /// </remarks>
        public IQueryable<Area> GetAreasWithOccupation(string occupationCode)
        {
            if (string.IsNullOrWhiteSpace(occupationCode))
                throw new ArgumentOutOfRangeException(nameof(occupationCode));
            return from salary in _dbContext.Salaries
                   join area in _dbContext.Areas on salary.AreaCode equals area.Code
                   where salary.OccupationCode == occupationCode
                   select area;
        }

        /// <summary>
        /// Gets 5 areas with the highest salaries for <paramref name="occupationCode"/>
        /// </summary>
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
                    limit 5
                ";
                var results = await connection.QueryAsync<AreaSalaryDTO>(sql, new { OccupationCode = occupationCode });
                foreach (var result in results)
                {
                    result.Name = normalizeAreaName(result.Name);
                }
                return results;
            }
        }

        private string normalizeAreaName(string areaName)
        {
            // Strip unnecessary area info from the name.
            // Ex: 'San Francisco-Redwood City-South San Francisco, CA Metropolitan Division'
            // becomes: 'San Francisco-Redwood City-South San Francisco'
            int commaIndex = areaName.IndexOf(',');
            if (commaIndex >= 0)
                return areaName = areaName.Substring(0, commaIndex);
            return areaName;
        }
    }
}
