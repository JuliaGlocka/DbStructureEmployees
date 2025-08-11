using System.ComponentModel.DataAnnotations;

namespace DbStructureEmployees.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public required String Name { get; set; }
        public int ?SuperiorId { get; set; }
        public virtual Employee? Superior { get; set; }

        public int TeamId { get; set; }
        public int PositionId { get; set; }
        public int VacationPackageId { get; set; }

        public virtual required Team Team { get; set; }
        public virtual required ICollection<Vacation> Vacations { get; set; }
    }
}
