using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryLocator.Logic.Models
{
    public class LivingWage
    {
        [Column("fips_code")]
        public int Code { get; set; }


        [Column("one_adult")]
        public decimal OneAdult { get; set; }

        [Column("one_adult_one_child")]
        public decimal OneAdultOneChild { get; set; }

        [Column("one_adult_two_children")]
        public decimal OneAdultTwoChildren { get; set; }

        [Column("one_adult_three_children")]
        public decimal OneAdultThreeChildren { get; set; }

        [Column("two_adults_one_working")]
        public decimal TwoAdultsOneWorking { get; set; }

        [Column("two_adults_one_working_one_Child")]
        public decimal TwoAdultsOneWorkingOneChild { get; set; }

        [Column("two_adults_one_working_two_Children")]
        public decimal TwoAdultsOneWorkingTwoChildren { get; set; }

        [Column("two_adults_one_working_three_Children")]
        public decimal TwoAdultsOneWorkingThreeChildren { get; set; }

        [Column("two_adults")]
        public decimal TwoAdults { get; set; }

        [Column("two_adults_one_Child")]
        public decimal TwoAdultsOneChild { get; set; }

        [Column("two_adults_two_children")]
        public decimal TwoAdultsTwoChildren { get; set; }

        [Column("two_adults_three_children")]
        public decimal TwoAdultsThreeChildren { get; set; }
    }
}
