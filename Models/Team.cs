using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbStructureEmployees.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 1,
            ErrorMessage = "Team name must be between 1 and 100 characters")]
        public string? Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}