using System.ComponentModel.DataAnnotations;

namespace DbStructureEmployees.Models
{
    public class VacationPackage
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string? Name { get; set; }

        [Range(0, 365, ErrorMessage = "GrantedDays must be between 0 and 365")]
        public int GrantedDays { get; set; }

        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
        public int Year { get; set; }

        [Range(0, 365, ErrorMessage = "TotalDays must be between 0 and 365")]
        public int TotalDays { get; set; }
    }
}