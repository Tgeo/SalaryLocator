using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryLocator.Logic.Models
{
    public class SalaryRecord
    {
        [Column("prim_state")]
        public string PrimaryState { get; set; }

        [Column("area")]
        public string AreaCode { get; set; }

        [Column("area_name")]
        public string AreaName { get; set; }

        [Column("occ_code")]
        public string OccupationCode { get; set; }

        [Column("occ_title")]
        public string OccupationTitle { get; set; }

        [Column("occ_group")]
        public string OccupationGroup { get; set; }

        [Column("tot_emp")]
        public int? TotalEmployment { get; set; }

        [Column("emp_prse")]
        public decimal? EmploymentStandardError { get; set; }

        [Column("jobs_1000")]
        public decimal? JobsPerThousand { get; set; }

        [Column("loc_quotient")]
        public decimal? LocationQuotient { get; set; }


        [Column("h_mean")]
        public decimal? MeanHourlyWage { get; set; }

        [Column("a_mean")]
        public int? MeanAnnualWage { get; set; }

        [Column("mean_prse")]
        public decimal? MeanStandardError { get; set; }


        [Column("h_pct10")]
        public decimal? Hourly10Percentile { get; set; }

        [Column("h_pct25")]
        public decimal? Hourly25Percentile { get; set; }

        [Column("h_median")]
        public decimal? HourlyMedianPercentile { get; set; }

        [Column("h_pct75")]
        public decimal? Hourly75Percentile { get; set; }

        [Column("h_pct90")]
        public decimal? Hourly90Percentile { get; set; }



        [Column("a_pct10")]
        public decimal? Annual10Percentile { get; set; }

        [Column("a_pct25")]
        public decimal? Annual25Percentile { get; set; }

        [Column("a_median")]
        public decimal? AnnualMedianPercentile { get; set; }

        [Column("a_pct75")]
        public decimal? Annual75Percentile { get; set; }

        [Column("a_pct90")]
        public decimal? Annual90Percentile { get; set; }



        [Column("annual")]
        public bool? AnnualWagesReleased { get; set; }

        [Column("hourly")]
        public bool? HourlyWagesReleased { get; set; }
    }
}
