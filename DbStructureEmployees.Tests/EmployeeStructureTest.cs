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

            // assert - fix null reference warnings
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            // use null-conditional operators or null checks
            var firstRelation = result.FirstOrDefault();
            Assert.NotNull(firstRelation); // ensure it's not null before accessing properties
            Assert.True(firstRelation.EmployeeId > 0);
            Assert.True(firstRelation.SuperiorId > 0);
        }

        [Fact]
        public void GetSuperiorRowOfEmployee_ShouldReturnCorrectLevel()
        {
            // arrange
            var employees = CreateTestEmployees();
            var employeeStructure = new EmployeeStructure();
            var structure = employeeStructure.FillEmployeesStructure(employees);

            // act
            var result = EmployeeStructure.GetSuperiorRowOfEmployee(structure, 1, 2);

            // assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Value); // .Value since it's nullable
        }

        private List<Employee> CreateTestEmployees()
        {
            // create test data with proper initialization to avoid nulls
            var ceo = new Employee { Id = 3, Name = "CEO", Superior = null };
            var manager = new Employee { Id = 2, Name = "Manager", Superior = ceo };
            var employee = new Employee { Id = 1, Name = "Employee", Superior = manager };

            return new List<Employee> { employee, manager, ceo };
        }

        [Fact]
        public void FillEmployeesStructure_WithNullSuperior_ShouldHandle()
        {
            // arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "TopLevel", Superior = null }
            };
            var employeeStructure = new EmployeeStructure();

            // act
            var result = employeeStructure.FillEmployeesStructure(employees);

            // assert
            Assert.NotNull(result);
            Assert.Empty(result); // Should be empty since no superior chain exists
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
    }
}