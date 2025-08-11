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



        // Helper do początku roku
        private static DateTime GetStartOfYear(int year) =>
    new DateTime(year, 1, 1, 0, 0, 0, DefaultKind);

        private static DateTime GetEndOfYear(int year) =>
            new DateTime(year, 12, 31, 23, 59, 59, DefaultKind);

        // a) Get list of employees from ".NET" team with at least one vacation request in 2019
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

        // b) Get list of employees along with the number of vacation days used this year
        // Count days only for vacations fully in the past (up to today)
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

        // c) Get list of teams whose employees have not taken any vacation days in 2019
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
    }
}
