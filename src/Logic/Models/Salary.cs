using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryLocator.Logic.Models
{
    public class Salary
    {
        [Column("area_code")]
        public int AreaCode { get; set; }

        [Column("occupation_code")]
        public string OccupationCode { get; set; }
        

        [Column("total_employment")]
        public int? TotalEmployment { get; set; }

        [Column("employment_std_err")]
        public decimal? EmploymentStandardError { get; set; }

        [Column("jobs_per_1000")]
        public decimal? JobsPerThousand { get; set; }

        [Column("location_quotient")]
        public decimal? LocationQuotient { get; set; }

        
        [Column("hourly_mean")]
        public decimal? HourlyMean { get; set; }

        [Column("annual_mean")]
        public int? AnnualMean { get; set; }

        [Column("mean_std_err")]
        public decimal? MeanStandardError { get; set; }

        
        [Column("hourly_10_pct")]
        public decimal? Hourly10Percentile { get; set; }

        [Column("hourly_25_pct")]
        public decimal? Hourly25Percentile { get; set; }

        [Column("hourly_med_pct")]
        public decimal? HourlyMedianPercentile { get; set; }

        [Column("hourly_75_pct")]
        public decimal? Hourly75Percentile { get; set; }

        [Column("hourly_90_pct")]
        public decimal? Hourly90Percentile { get; set; }


        [Column("annual_10_pct")]
        public int? Annual10Percentile { get; set; }

        [Column("annual_25_pct")]
        public int? Annual25Percentile { get; set; }

        [Column("annual_med_pct")]
        public int? AnnualMedianPercentile { get; set; }

        [Column("annual_75_pct")]
        public int? Annual75Percentile { get; set; }

        [Column("annual_90_pct")]
        public int? Annual90Percentile { get; set; }
    }
}
