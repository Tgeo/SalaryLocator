using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryLocator.Logic.Models
{
    public class Area
    {
        /// <summary>
        /// ID of the area. Directly maps to MSA
        /// codes and MSA division codes.
        /// </summary>
        /// <remarks>
        /// MSA division codes are basically sub-MSAs.
        /// Example: "San Francisco-Redwood City-South San Francisco, CA Metropolitan Division"
        /// is an "MSA division" of the "San Francisco-Oakland-Hayward, Calif. Metropolitan Statistical Area" MSA.
        /// </remarks>
        [Column("area_code")]
        public int Code { get; set; }

        [Column("primary_state_code")]
        public string PrimaryStateCode { get; set; }

        /// <summary>
        /// The MSA code for this area. Never an MSA
        /// "division" code.
        /// </summary>
        [Column("msa_code")]
        public string MsaCode { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
