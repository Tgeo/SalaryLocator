using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryLocator.Logic.Models
{
    public class Area
    {
        [Column("area_code")]
        public int Code { get; set; }

        [Column("primary_state_code")]
        public string PrimaryStateCode { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
