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

        public EmployeeQueries(AppDbContext context)
        {
            _context = context;
        }

        // a) Get list of employees from ".NET" team with at least one vacation request in 2019
        public List<Employee> GetEmployeesFromDotNetWithVacationIn2019()
        {
            var yearStart = new DateTime(2019, 1, 1);
            var yearEnd = new DateTime(2019, 12, 31);

            var query = _context.Employees
                .Include(e => e.Team) // Include Team data if needed
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
            var yearStart = new DateTime(DateTime.Now.Year, 1, 1);
            var today = DateTime.Now.Date;

            var query = _context.Employees
                .Select(e => new
                {
                    Employee = e,
                    UsedDays = _context.Vacations
                        .Where(v => v.EmployeeId == e.Id &&
                                    v.DateStart >= yearStart &&
                                    v.DateEnd <= today)
                        .Sum(v => (v.DateEnd - v.DateStart).Days + 1) // +1 to count both start and end days
                })
                .AsEnumerable() // Bring to memory to be able to return tuple
                .Select(x => (x.Employee, x.UsedDays))
                .ToList();

            return query;
        }

        // c) Get list of teams whose employees have not taken any vacation days in 2019
        public List<Team> GetTeamsWithoutVacationIn2019()
        {
            var yearStart = new DateTime(2019, 1, 1);
            var yearEnd = new DateTime(2019, 12, 31);

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
