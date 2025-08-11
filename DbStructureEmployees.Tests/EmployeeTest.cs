using DbStructureEmployees.Models;
using Xunit;

namespace DbStructureEmployees.Tests
{
    public class EmployeeTest
    {
        [Fact]
        public void Employee_Creation_ShouldSetPropertiesCorrectly()
        {
            // arrange & act
            var superior = new Employee
            {
                Id = 1,
                Name = "Henryk Kowalski",
                SuperiorId = 0,
                Superior = null
            };

            var employee = new Employee
            {
                Id = 2,
                Name = "Jan Nowak",
                SuperiorId = 1,
                Superior = superior
            };

            // assert
            Assert.Equal(2, employee.Id);
            Assert.Equal("Jan Nowak", employee.Name);
            Assert.Equal(1, employee.SuperiorId);
            Assert.NotNull(employee.Superior);
            Assert.Equal("Henryk Kowalski", employee.Superior.Name);
        }

        [Fact]
        public void Employee_Superior_CanBeNull()
        {
            var employee = new Employee
            {
                Id = 1,
                Name = "Henryk Kowalski",
                SuperiorId = 0,
                Superior = null
            };

            Assert.Null(employee.Superior);
        }
    }
}
