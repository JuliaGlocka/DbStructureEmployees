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

        public List<Employee> GetEmployeesFromDotNetWithVacationInYear(int year)
{
    if (year < 1900 || year > 2100)
        throw new ArgumentException($"Invalid year: {year}. Must be between 1900 and 2100.", nameof(year));

    var yearStart = GetStartOfYear(year);
    var yearEnd = GetEndOfYear(year);

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

// Keep old method for backward compatibility, add [Obsolete]
[Obsolete("Use GetEmployeesFromDotNetWithVacationInYear(int year) instead")]
public List<Employee> GetEmployeesFromDotNetWithVacationIn2019()
{
    return GetEmployeesFromDotNetWithVacationInYear(2019);
}

        public List<(Employee employee, int usedVacationDays)> GetEmployeesVacationDaysUsedInYear(int year)
        {
            if (year < 1900 || year > 2100)
                throw new ArgumentException($"Invalid year: {year}", nameof(year));

            var yearStart = GetStartOfYear(year);
            var today = DateTime.UtcNow;

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

        [Obsolete("Use GetEmployeesVacationDaysUsedInYear(int year) instead")]
        public List<(Employee employee, int usedVacationDays)> GetEmployeesVacationDaysUsedThisYear()
        {
            return GetEmployeesVacationDaysUsedInYear(DateTime.UtcNow.Year);
        }

        public List<Team> GetTeamsWithoutVacationInYear(int year)
        {
            if (year < 1900 || year > 2100)
                throw new ArgumentException($"Invalid year: {year}", nameof(year));

            var yearStart = GetStartOfYear(year);
            var yearEnd = GetEndOfYear(year);

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

        [Obsolete("Use GetTeamsWithoutVacationInYear(int year) instead")]
        public List<Team> GetTeamsWithoutVacationIn2019()
        {
            return GetTeamsWithoutVacationInYear(2019);
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