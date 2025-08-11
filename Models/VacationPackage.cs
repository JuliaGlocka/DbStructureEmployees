using System.ComponentModel.DataAnnotations;

namespace DbStructureEmployees.Models
{
    public class VacationPackage
    {
        [Key]
        public int Id { get; set; }
        public string ?Name { get; set; }
        public int GrantedDays { get; set; }
        public int Year { get; set; }
    }
}