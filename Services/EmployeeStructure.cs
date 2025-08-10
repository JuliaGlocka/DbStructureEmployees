using DbStructureEmployees.Models;

namespace DbStructureEmployees.Services
{
    public class EmployeeStructure
    {
        public int EmployeeId { get; set; }
        public int SuperiorId { get; set; }
        public int SuperiorLevel { get; set; }

        public List<EmployeeStructure> FillEmployeesStructure(List<Employee> employees)
        {
            var structure = new List<EmployeeStructure>();

            foreach (var emp in employees)
            {
                int level = 1;
                var currentSuperior = emp.Superior;

                while (currentSuperior != null)
                {
                    structure.Add(new EmployeeStructure
                    {
                        EmployeeId = emp.Id,
                        SuperiorId = currentSuperior.Id,
                        SuperiorLevel = level
                    });

                    currentSuperior = currentSuperior.Superior;
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
