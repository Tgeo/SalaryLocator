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
                   orderby area.Name
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

        public async Task<IEnumerable<AreaSalaryDTO>> GetAreasWithHighestSalariesAdjustForCostOfLivingAsync(string occupationCode)
        {
            // Not using EF here because of "greatest" in order by clause.
            using (var connection = _dbContext.Database.GetDbConnection())
            {
                // PGSql does not support creating variables without functions.
                // There is an "anonymous code block" but you can't return any data from it.
                // Also, we don't want to setup Entity Framework to create this stored procedure just yet...
                // So, for now -- we create the function each time we use it.
                const string sql = @"
                    CREATE OR REPLACE FUNCTION getCitiesWithHighestSalariesAdjustedForCol(occupationCode text)
                    RETURNS table(name text, annual_med_pct DOUBLE precision, hourly_med_pct DOUBLE precision) AS $$
                    DECLARE
                        comparisonLivingWage money;
                    BEGIN

                    SELECT one_adult INTO comparisonLivingWage
                    FROM living_wage
                    WHERE fips_code = 1 -- national average
                    LIMIT 1;

                    RETURN query (
                        SELECT subQuery.name, subQuery.annual, subQuery.hourly
                        FROM (
                            SELECT
                                area.name,
                                salary.annual_med_pct / (one_adult / comparisonLivingWage) AS annual,
                                (salary.hourly_med_pct * 40 * 52) / (one_adult / comparisonLivingWage) AS hourly        
                            FROM salary
                            INNER JOIN area 
                                ON area.area_code = salary.area_code
                            INNER JOIN living_wage
                                ON living_wage.fips_code = area.msa_code
                            WHERE occupation_code = occupationCode AND jobs_per_1000 >= 6.5 -- magic number
                        ) subQuery
                        ORDER BY GREATEST(subQuery.annual, subQuery.hourly) DESC
                        LIMIT 5);

                    END;
                    $$ LANGUAGE plpgsql;

                    SELECT * FROM getCitiesWithHighestSalariesAdjustedForCol('15-1133');
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
