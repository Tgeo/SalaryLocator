using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryLocator.Logic.Models.DTOs
{
    /// <summary>
    /// Represents the salary stats for a specific area.
    /// </summary>
    public class AreaSalaryDTO
    {
        public int AreaCode { get; set; }
        public string StateCode { get; set; }        
        public string Name { get; set; }
        
        public string OccupationCode { get; set; }
        public int? TotalEmployment { get; set; }
                
        public decimal? HourlyMean { get; set; }
        public int? AnnualMean { get; set; }
                
        public decimal? Hourly10Percentile { get; set; }
        public decimal? Hourly25Percentile { get; set; }
        public decimal? HourlyMedianPercentile { get; set; }
        public decimal? Hourly75Percentile { get; set; }
        public decimal? Hourly90Percentile { get; set; }
        
        public int? Annual10Percentile { get; set; }
        public int? Annual25Percentile { get; set; }
        public int? AnnualMedianPercentile { get; set; }
        public int? Annual75Percentile { get; set; }
        public int? Annual90Percentile { get; set; }
    }
}
