using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbStructureEmployees.Models
{
    public class Vacation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vacation start date is required")]
        public DateTime DateStart { get; set; }

        [Required(ErrorMessage = "Vacation end date is required")]
        public DateTime DateEnd { get; set; }

        [Range(0, 24, ErrorMessage = "NumberOfHours must be between 0 and 24")]
        public double NumberOfHours { get; set; }

        public bool IsPartialVacation { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "EmployeeId must be valid")]
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Associated employee is required")]
        public virtual required Employee Employee { get; set; }
    }
}