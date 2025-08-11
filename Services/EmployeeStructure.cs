using System.Collections.Generic;
using System.Linq;
using DbStructureEmployees.Models;

namespace DbStructureEmployees.Services
{
    public class EmployeeStructure
    {
        public int EmployeeId { get; set; }
        public int SuperiorId { get; set; }
        public int SuperiorLevel { get; set; } // superior level - 3 is the highest, indirectly superior, 1 is direct superior

        public List<EmployeeStructure> FillEmployeesStructure(List<Employee> employees)
        {
            var structure = new List<EmployeeStructure>();

            foreach (var emp in employees)
            {
                int level = 1;
                var currentSuperior = employees.FirstOrDefault(e => e.Id == emp.SuperiorId);

                while (currentSuperior != null)
                {
                    structure.Add(new EmployeeStructure
                    {
                        EmployeeId = emp.Id,
                        SuperiorId = currentSuperior.Id,
                        SuperiorLevel = level
                    });

                    // iterate through the structure upwards
                    currentSuperior = employees.FirstOrDefault(e => e.Id == currentSuperior.SuperiorId);
                    level++;
                }
            }

            return structure;
        }

        public static int? GetSuperiorRowOfEmployee(
            List<EmployeeStructure> structure,
            int employeeId,
            int superiorId)
        {
            var rel = structure.FirstOrDefault(s =>
                s.EmployeeId == employeeId && s.SuperiorId == superiorId);

            return rel?.SuperiorLevel;
        }
    }
}
