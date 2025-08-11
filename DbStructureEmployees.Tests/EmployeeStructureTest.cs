using System.Collections.Generic;
using System.Linq;
using DbStructureEmployees.Models;
using DbStructureEmployees.Services;
using Xunit;

namespace DbStructureEmployees.Tests
{
    public class EmployeeStructureTest
    {
        [Fact]
        public void FillEmployeesStructure_ShouldReturnCorrectStructure()
        {
            // arrange
            var employees = CreateTestEmployees();
            var employeeStructure = new EmployeeStructure();

            // act
            var result = employeeStructure.FillEmployeesStructure(employees);

            // assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            var firstRelation = result.FirstOrDefault();
            Assert.NotNull(firstRelation);
            Assert.True(firstRelation.EmployeeId > 0);
            Assert.True(firstRelation.SuperiorId > 0);
            Assert.True(firstRelation.SuperiorLevel > 0);
        }

        [Fact]
        public void GetSuperiorRowOfEmployee_ShouldReturnCorrectLevel()
        {
            // arrange
            var employees = CreateTestEmployees();
            var employeeStructure = new EmployeeStructure();
            var structure = employeeStructure.FillEmployeesStructure(employees);

            // act
            var result1 = EmployeeStructure.GetSuperiorRowOfEmployee(structure, 2, 1);
            var result2 = EmployeeStructure.GetSuperiorRowOfEmployee(structure, 4, 3);
            var result3 = EmployeeStructure.GetSuperiorRowOfEmployee(structure, 4, 1);

            // assert
            Assert.Equal(1, result1);  // Id2 has superior Id1 at level 1
            Assert.Null(result2);      // Id4 does not exist in the structure
            Assert.Null(result3);      // Id4 does not exist in the structure
        }

        [Fact]
        public void FillEmployeesStructure_WithNullSuperior_ShouldHandle()
        {
            // arrange
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = "Jan Kowalski",
                    SuperiorId = null,
                    Team = new Team { Id = 1, Name = "Team A" },
                    Vacations = new List<Vacation>()
                } // no superior
            };
            var employeeStructure = new EmployeeStructure();

            // act
            var result = employeeStructure.FillEmployeesStructure(employees);

            // assert
            Assert.NotNull(result);
            Assert.Empty(result); // no level, since no superior
        }

        [Fact]
        public void FillEmployeesStructure_WithNullableSuperiorId_ShouldHandleNull()
        {
            // arrange
            var employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = "Jan Kowalski",
                    SuperiorId = null,
                    Team = new Team { Id = 1, Name = "Team A" },
                    Vacations = new List<Vacation>()
                }, // null oznacza brak przełożonego
                new Employee
                {
                    Id = 2,
                    Name = "Anna Nowak",
                    SuperiorId = 1,
                    Team = new Team { Id = 1, Name = "Team A" },
                    Vacations = new List<Vacation>()
                }
            };
            var employeeStructure = new EmployeeStructure();

            // act
            var result = employeeStructure.FillEmployeesStructure(employees);

            // assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains(result, r => r.EmployeeId == 2 && r.SuperiorId == 1);
        }

        [Fact]
        public void GetSuperiorRowOfEmployee_WithNonExistentRelation_ShouldReturnNull()
        {
            // arrange
            var structure = new List<EmployeeStructure>();

            // act
            var result = EmployeeStructure.GetSuperiorRowOfEmployee(structure, 999, 888);

            // assert
            Assert.Null(result);
        }

        private List<Employee> CreateTestEmployees()
        {
            var team = new Team { Id = 1, Name = "Team A" };

            return new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = "Henryk Kowalski",
                    SuperiorId = null,
                    Team = team,
                    Vacations = new List<Vacation>()
                },
                new Employee
                {
                    Id = 2,
                    Name = "Jan Nowak",
                    SuperiorId = 1,
                    Team = team,
                    Vacations = new List<Vacation>()
                },
                new Employee
                {
                    Id = 3,
                    Name = "Anna Wiśniewska",
                    SuperiorId = 2,
                    Team = team,
                    Vacations = new List<Vacation>()
                }
            };
        }
    }
}
