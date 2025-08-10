using System.Collections.Generic;
using Xunit;
using DbStructureEmployees.Models;
using DbStructureEmployees.Services;

namespace DbStructureEmployees.Tests
{
    public class EmployeeStructureTest
    {
        [Fact]
        public void TestSuperiorLevels()
        {
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Jan Kowalski", Superior = null },
                new Employee { Id = 2, Name = "Kamil Nowak", Superior = null },
                new Employee { Id = 3, Name = "Antonii B³yskawica", Superior = null },
                new Employee { Id = 4, Name = "Andrzej Abacki", Superior = null }
            };

            employees.Find(e => e.Id == 2).Superior = employees.Find(e => e.Id == 1);
            employees.Find(e => e.Id == 4).Superior = employees.Find(e => e.Id == 2);

            var service = new EmployeeStructure();
            var structure = service.FillEmployeesStructure(employees);

            var row1 = EmployeeStructure.GetSuperiorRowOfEmployee(structure, 2, 1);
            var row2 = EmployeeStructure.GetSuperiorRowOfEmployee(structure, 4, 3);
            var row3 = EmployeeStructure.GetSuperiorRowOfEmployee(structure, 4, 1);

            Assert.Equal(1, row1);
            Assert.Null(row2);
            Assert.Equal(2, row3);
        }
    }
}
