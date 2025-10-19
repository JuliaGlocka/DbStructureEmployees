using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbStructureEmployees.Data;
using DbStructureEmployees.Models;

namespace DbStructureEmployees.Services
{
    public class EmployeeQueries
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EmployeeQueries> _logger;

        private static readonly DateTimeKind DefaultKind = DateTimeKind.Utc;

        public EmployeeQueries(AppDbContext context, ILogger<EmployeeQueries> logger)
        {
            _context = context;
            _logger = logger;
        }

        private static DateTime GetStartOfYear(int year) =>
            new DateTime(year, 1, 1, 0, 0, 0, DefaultKind);

        private static DateTime GetEndOfYear(int year) =>
            new DateTime(year, 12, 31, 23, 59, 59, DefaultKind);

        /// <summary>
        /// Gets all employees from the .NET team who took vacation in the specified year
        /// </summary>
        /// <param name="year">The year to query</param>
        /// <returns>List of employees with vacations in the specified year</returns>
        public List<Employee> GetEmployeesFromDotNetWithVacationInYear(int year)
        {
            _logger.LogInformation("Querying employees from .NET team with vacations in year {Year}", year);

            if (year < 1900 || year > 2100)
            {
                _logger.LogWarning("Invalid year provided: {Year}. Must be between 1900 and 2100", year);
                throw new ArgumentException($"Invalid year: {year}. Must be between 1900 and 2100.", nameof(year));
            }

            try
            {
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

                _logger.LogInformation("Found {EmployeeCount} employees from .NET team with vacations in year {Year}",
                    query.Count, year);

                return query;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying employees from .NET team for year {Year}", year);
                throw;
            }
        }

        /// <summary>
        /// Obsolete: Use GetEmployeesFromDotNetWithVacationInYear(int year) instead
        /// </summary>
        [Obsolete("Use GetEmployeesFromDotNetWithVacationInYear(int year) instead")]
        public List<Employee> GetEmployeesFromDotNetWithVacationIn2019()
        {
            _logger.LogWarning("Deprecated method GetEmployeesFromDotNetWithVacationIn2019() called. " +
                "Use parameterized version GetEmployeesFromDotNetWithVacationInYear(int year)");
            return GetEmployeesFromDotNetWithVacationInYear(2019);
        }

        /// <summary>
        /// Gets vacation days used by each employee in the specified year
        /// </summary>
        /// <param name="year">The year to calculate for</param>
        /// <returns>List of employees with their used vacation days count</returns>
        public List<(Employee employee, int usedVacationDays)> GetEmployeesVacationDaysUsedInYear(int year)
        {
            _logger.LogInformation("Calculating vacation days used for all employees in year {Year}", year);

            if (year < 1900 || year > 2100)
            {
                _logger.LogWarning("Invalid year provided: {Year}", year);
                throw new ArgumentException($"Invalid year: {year}. Must be between 1900 and 2100.", nameof(year));
            }

            try
            {
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

                _logger.LogInformation("Calculated vacation days for {EmployeeCount} employees in year {Year}",
                    query.Count, year);

                return query;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating vacation days used for year {Year}", year);
                throw;
            }
        }

        /// <summary>
        /// Obsolete: Use GetEmployeesVacationDaysUsedInYear(int year) instead
        /// </summary>
        [Obsolete("Use GetEmployeesVacationDaysUsedInYear(int year) instead")]
        public List<(Employee employee, int usedVacationDays)> GetEmployeesVacationDaysUsedThisYear()
        {
            _logger.LogWarning("Deprecated method GetEmployeesVacationDaysUsedThisYear() called. " +
                "Use parameterized version GetEmployeesVacationDaysUsedInYear(int year)");
            return GetEmployeesVacationDaysUsedInYear(DateTime.UtcNow.Year);
        }

        /// <summary>
        /// Gets all teams that have no employees on vacation in the specified year
        /// </summary>
        /// <param name="year">The year to query</param>
        /// <returns>List of teams without vacation in the specified year</returns>
        public List<Team> GetTeamsWithoutVacationInYear(int year)
        {
            _logger.LogInformation("Querying teams without vacation in year {Year}", year);

            if (year < 1900 || year > 2100)
            {
                _logger.LogWarning("Invalid year provided: {Year}", year);
                throw new ArgumentException($"Invalid year: {year}. Must be between 1900 and 2100.", nameof(year));
            }

            try
            {
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

                _logger.LogInformation("Found {TeamCount} teams without vacation in year {Year}", teams.Count, year);

                return teams;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying teams without vacation for year {Year}", year);
                throw;
            }
        }

        /// <summary>
        /// Obsolete: Use GetTeamsWithoutVacationInYear(int year) instead
        /// </summary>
        [Obsolete("Use GetTeamsWithoutVacationInYear(int year) instead")]
        public List<Team> GetTeamsWithoutVacationIn2019()
        {
            _logger.LogWarning("Deprecated method GetTeamsWithoutVacationIn2019() called. " +
                "Use parameterized version GetTeamsWithoutVacationInYear(int year)");
            return GetTeamsWithoutVacationInYear(2019);
        }

        /// <summary>
        /// Calculates free vacation days remaining for an employee in the current year
        /// </summary>
        /// <param name="employee">The employee</param>
        /// <param name="vacations">List of all vacations for the employee</param>
        /// <param name="vacationPackage">The vacation package with total days</param>
        /// <returns>Number of free days remaining (0 if none)</returns>
        public static int CountFreeDaysForEmployee(Employee employee,
            List<Vacation> vacations,
            VacationPackage vacationPackage)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            if (vacations == null)
                throw new ArgumentNullException(nameof(vacations));
            if (vacationPackage == null)
                throw new ArgumentNullException(nameof(vacationPackage));

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

        /// <summary>
        /// Determines if an employee can request a vacation based on remaining days
        /// </summary>
        /// <param name="employee">The employee</param>
        /// <param name="vacations">List of all vacations for the employee</param>
        /// <param name="vacationPackage">The vacation package with total days</param>
        /// <returns>True if employee has remaining vacation days</returns>
        public static bool IfEmployeeCanRequestVacation(Employee employee,
            List<Vacation> vacations,
            VacationPackage vacationPackage)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            if (vacations == null)
                throw new ArgumentNullException(nameof(vacations));
            if (vacationPackage == null)
                throw new ArgumentNullException(nameof(vacationPackage));

            return CountFreeDaysForEmployee(employee, vacations, vacationPackage) > 0;
        }
    }
}