using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbStructureEmployees.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee name is required")]
        [StringLength(100, MinimumLength = 1,
            ErrorMessage = "Name must be between 1 and 100 characters")]
        public required string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "SuperiorId must be positive if provided")]
        [ForeignKey(nameof(Superior))]
        public int? SuperiorId { get; set; }

        public virtual Employee? Superior { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "TeamId must be valid")]
        public int TeamId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PositionId must be valid")]
        public int PositionId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "VacationPackageId must be valid")]
        public int VacationPackageId { get; set; }

        public virtual required Team Team { get; set; }
        public virtual required ICollection<Vacation> Vacations { get; set; }
    }
}