using DbStructureEmployees.Models;
using System.Collections.Generic;
using Xunit;

namespace DbStructureEmployees.Tests
{
    public class EmployeeTest
    {
        [Fact]
        public void Employee_Creation_ShouldSetPropertiesCorrectly()
        {
            // arrange & act
            var team = new Team { Id = 1, Name = ".NET" };

            var superior = new Employee
            {
                Id = 1,
                Name = "Henryk Kowalski",
                SuperiorId = 0,
                Superior = null,
                TeamId = team.Id,
                Team = team,
                PositionId = 1,
                VacationPackageId = 1,
                Vacations = new List<Vacation>()
            };

            var employee = new Employee
            {
                Id = 2,
                Name = "Jan Nowak",
                SuperiorId = 1,
                Superior = superior,
                TeamId = team.Id,
                Team = team,
                PositionId = 2,
                VacationPackageId = 1,
                Vacations = new List<Vacation>()
            };

            // assert
            Assert.Equal(2, employee.Id);
            Assert.Equal("Jan Nowak", employee.Name);
            Assert.Equal(1, employee.SuperiorId);
            Assert.NotNull(employee.Superior);
            Assert.Equal("Henryk Kowalski", employee.Superior.Name);
            Assert.NotNull(employee.Team);
            Assert.Equal(".NET", employee.Team.Name);
            Assert.NotNull(employee.Vacations);
            Assert.Empty(employee.Vacations);
        }

        [Fact]
        public void Employee_Superior_CanBeNull()
        {
            var team = new Team { Id = 1, Name = ".NET" };

            var employee = new Employee
            {
                Id = 1,
                Name = "Henryk Kowalski",
                SuperiorId = 0,
                Superior = null,
                TeamId = team.Id,
                Team = team,
                PositionId = 1,
                VacationPackageId = 1,
                Vacations = new List<Vacation>()
            };

            Assert.Null(employee.Superior);
            Assert.NotNull(employee.Team);
            Assert.Equal(".NET", employee.Team.Name);
            Assert.NotNull(employee.Vacations);
            Assert.Empty(employee.Vacations);
        }
    }
}
