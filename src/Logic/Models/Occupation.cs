using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryLocator.Logic.Models
{
    public class Occupation
    {
        [Column("occupation_code")]
        public string Code { get; set; }

        [Column("occupation_group")]
        public int GroupType { get; set; }

        [Column("title")]
        public string Title { get; set; }
    }
}
