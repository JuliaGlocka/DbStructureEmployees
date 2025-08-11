using System;
using System.ComponentModel.DataAnnotations;

namespace DbStructureEmployees.Models
{
    public class Vacation
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateSince { get; set; }
        public DateTime DateUntil { get; set; }
        public double NumberOfHours { get; set; }
        public bool IsPartialVacation { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;
    }
}