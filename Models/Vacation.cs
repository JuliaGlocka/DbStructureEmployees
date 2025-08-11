using System;
using System.ComponentModel.DataAnnotations;

namespace DbStructureEmployees.Models
{
    public class Vacation
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public double NumberOfHours { get; set; }
        public bool IsPartialVacation { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;
    }
}