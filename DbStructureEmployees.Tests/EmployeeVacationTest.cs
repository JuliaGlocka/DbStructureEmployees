using DbStructureEmployees.Models;
using DbStructureEmployees.Services;
using System.Collections.Generic;
using Xunit;

namespace DbStructureEmployees.Tests
{
    public class EmployeeVacationTest
    {
        [Fact]
        public void Employee_Can_Request_Vacation()
        {
            // arrange
            var team = new Team { Id = 1, Name = ".NET" };
            var vacationPackage = new VacationPackage { Id = 1, TotalDays = 5 };
            var employee = new Employee
            {
                Id = 1,
                Name = "Jan Kowalski",
                TeamId = team.Id,
                Team = team,
                VacationPackageId = vacationPackage.Id,
                Vacations = new List<Vacation>() // total list of vacations
            };

            // act
            var result = EmployeeQueries.IfEmployeeCanRequestVacation(employee, employee.Vacations.ToList(), vacationPackage);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void Employee_Cannot_Request_Vacation_When_No_Remaining_Days()
        {
            // arrange
            var team = new Team { Id = 1, Name = ".NET" };
            var vacationPackage = new VacationPackage { Id = 1, TotalDays = 0 };
            var employee = new Employee
            {
                Id = 2,
                Name = "Anna Nowak",
                TeamId = team.Id,
                Team = team,
                VacationPackageId = vacationPackage.Id,
                Vacations = new List<Vacation>() // pusta lista urlopów
            };

            // act
            var result = EmployeeQueries.IfEmployeeCanRequestVacation(employee, employee.Vacations.ToList(), vacationPackage);

            // assert
            Assert.False(result);
        }
    }
}
