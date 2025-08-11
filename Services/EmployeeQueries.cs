using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore; // for Include and EF Core features
using DbStructureEmployees.Data;
using DbStructureEmployees.Models;

namespace DbStructureEmployees.Services
{
    public class EmployeeQueries
    {
        private readonly AppDbContext _context;

        private static readonly DateTimeKind DefaultKind = DateTimeKind.Utc;

        public EmployeeQueries(AppDbContext context)
        {
            _context = context;
        }

        // helper - methods to get start and end of the year
        private static DateTime GetStartOfYear(int year) =>
    new DateTime(year, 1, 1, 0, 0, 0, DefaultKind);

        private static DateTime GetEndOfYear(int year) =>
            new DateTime(year, 12, 31, 23, 59, 59, DefaultKind);

        public List<Employee> GetEmployeesFromDotNetWithVacationIn2019()
        {
            var yearStart = GetStartOfYear(2019);
            var yearEnd = GetEndOfYear(2019);

            var query = _context.Employees
                .Include(e => e.Team)
                .Where(e => e.Team.Name == ".NET" &&
                            _context.Vacations.Any(v =>
                                v.EmployeeId == e.Id &&
                                v.DateStart <= yearEnd &&
                                v.DateEnd >= yearStart))
                .ToList();

            return query;
        }
        public List<(Employee employee, int usedVacationDays)> GetEmployeesVacationDaysUsedThisYear()
        {
            var yearStart = GetStartOfYear(2019);

            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DefaultKind);

            var query = _context.Employees
                .Select(e => new
                {
                    Employee = e,
                    UsedDays = _context.Vacations
                        .Where(v => v.EmployeeId == e.Id &&
                                    v.DateStart >= yearStart &&
                                    v.DateEnd <= today)
                        .Sum(v => (v.DateEnd - v.DateStart).Days + 1)
                })
                .AsEnumerable()
                .Select(x => (x.Employee, x.UsedDays))
                .ToList();

            return query;
        }
        public List<Team> GetTeamsWithoutVacationIn2019()
        {
            var yearStart = GetStartOfYear(2019);
            var yearEnd = GetEndOfYear(2019);

            var teams = _context.Teams
                .Where(team => !_context.Employees
                    .Where(e => e.TeamId == team.Id)
                    .Any(e => _context.Vacations
                        .Any(v =>
                            v.EmployeeId == e.Id &&
                            v.DateStart <= yearEnd &&
                            v.DateEnd >= yearStart)))
                .ToList();

            return teams;
        }

        public static int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            var year = DateTime.UtcNow.Year;
            var yearStart = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var yearEnd = new DateTime(year, 12, 31, 23, 59, 59, DateTimeKind.Utc);

            var usedDays = vacations
                .Where(v => v.EmployeeId == employee.Id &&
                            v.DateStart >= yearStart &&
                            v.DateEnd <= yearEnd)
                .Sum(v => (v.DateEnd - v.DateStart).Days + 1);

            var freeDays = vacationPackage.TotalDays - usedDays;

            return freeDays > 0 ? freeDays : 0;
        }

        public static bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            return CountFreeDaysForEmployee(employee, vacations, vacationPackage) > 0;
        }

    }
}
