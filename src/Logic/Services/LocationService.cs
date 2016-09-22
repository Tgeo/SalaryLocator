using Npgsql;
using SalaryLocator.Logic.Models;
using SalaryLocator.Logic.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<IEnumerable<AreaSalaryDTO>> GetAreasWithHighestSalariesAsync(string occupationCode)
        {
            // Done using ADO.NET because the "greatest" in the "order by" clause
            // is not possible with EF.
            using (var connection = new NpgsqlConnection(_dbContext.GetConnectionString()))
            {
                connection.Open();
                const string sql = @"
                    select
	                    area.primary_state_code,
	                    area.name,
	                    area.area_code,	
	                    salary.occupation_code,
	                    salary.total_employment,	
	                    salary.hourly_mean,
	                    salary.annual_mean,	
	                    salary.hourly_10_pct,
	                    salary.hourly_25_pct,
	                    salary.hourly_med_pct,
	                    salary.hourly_75_pct,
	                    salary.hourly_90_pct,	
	                    salary.annual_10_pct,
	                    salary.annual_25_pct,
	                    salary.annual_med_pct,
	                    salary.annual_75_pct,
	                    salary.annual_90_pct
                    from salary
                    inner join area
                        on area.area_code = salary.area_code
                    where occupation_code = @occupationCode
                    order by greatest(coalesce(annual_med_pct, -1), coalesce(hourly_med_pct * 40 * 52, -1)) desc
                    limit 10
                ";
                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@occupationCode", occupationCode);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // .ToList() required because of lazy-eval on the IEnumerable<T>.
                        // (The reader will be disposed by the time it gets evaluated otherwise).
                        return getAreaSalaryDTOs(reader).ToList();
                    }
                }
            }
        }

        private IEnumerable<AreaSalaryDTO> getAreaSalaryDTOs(System.Data.Common.DbDataReader dataReader)
        {
            while (dataReader.Read())
            {
                yield return getAreaSalaryDTO(dataReader);
            }
        }

        private AreaSalaryDTO getAreaSalaryDTO(System.Data.Common.DbDataReader dataReader)
        {
            try
            {
                // This shouldn't be necessary... but AutoMapper wouldn't work.
                // Suspect it was something to do with mapping from a "Npgsql" db reader.
                return new AreaSalaryDTO()
                {
                    StateCode = (string)dataReader["primary_state_code"],
                    Name = (string)dataReader["name"],
                    AreaCode = (int)dataReader["area_code"],
                    OccupationCode = (string)dataReader["occupation_code"],
                    TotalEmployment = dataReader["total_employment"] as int?,
                    HourlyMean = dataReader["hourly_mean"] as decimal?,
                    AnnualMean = dataReader["annual_mean"] as int?,
                    Hourly10Percentile = dataReader["hourly_10_pct"] as decimal?,
                    Hourly25Percentile = dataReader["hourly_25_pct"] as decimal?,
                    HourlyMedianPercentile = dataReader["hourly_med_pct"] as decimal?,
                    Hourly75Percentile = dataReader["hourly_75_pct"] as decimal?,
                    Hourly90Percentile = dataReader["hourly_90_pct"] as decimal?,
                    Annual10Percentile = dataReader["annual_10_pct"] as int?,
                    Annual25Percentile = dataReader["annual_25_pct"] as int?,
                    AnnualMedianPercentile = dataReader["annual_med_pct"] as int?,
                    Annual75Percentile = dataReader["annual_75_pct"] as int?,
                    Annual90Percentile = dataReader["annual_90_pct"] as int?
                };
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
