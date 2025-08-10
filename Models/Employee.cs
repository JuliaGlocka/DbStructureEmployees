using System.ComponentModel.DataAnnotations;

namespace DbStructureEmployees.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public required String Name { get; set; }
        public int SuperiorId { get; set; }
        public virtual Employee? Superior { get; set; }

    }
}
