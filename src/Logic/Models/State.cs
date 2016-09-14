using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryLocator.Logic.Models
{
    public class State
    {
        [Column("state_code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
