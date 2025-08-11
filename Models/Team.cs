using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbStructureEmployees.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string ?Name { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}