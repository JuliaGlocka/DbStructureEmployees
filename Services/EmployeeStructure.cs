using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using DbStructureEmployees.Models;

namespace DbStructureEmployees.Services
{
    /// <summary>
    /// Service for building and navigating organizational hierarchy structures
    /// </summary>
    public class EmployeeStructure
    {
        private readonly ILogger<EmployeeStructure>? _logger;

        public int EmployeeId { get; set; }
        public int SuperiorId { get; set; }
        public int SuperiorLevel { get; set; }

        public EmployeeStructure() { }

        public EmployeeStructure(ILogger<EmployeeStructure>? logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// Builds a flat organizational structure mapping from a list of employees
        /// Creates relations for each employee to all of their superiors at each level
        /// </summary>
        /// <param name="employees">List of employees with hierarchy</param>
        /// <returns>Flat list of employee-superior relationships with levels</returns>
        public List<EmployeeStructure> FillEmployeesStructure(List<Employee> employees)
        {
            _logger?.LogInformation("Building organizational structure from {EmployeeCount} employees", employees.Count);

            if (employees == null || !employees.Any())
            {
                _logger?.LogWarning("No employees provided to build structure");
                return new List<EmployeeStructure>();
            }

            var structure = new List<EmployeeStructure>();

            try
            {
                foreach (var emp in employees)
                {
                    int level = 1;
                    var currentSuperior = employees.FirstOrDefault(e => e.Id == emp.SuperiorId);

                    // Traverse up the organizational hierarchy
                    while (currentSuperior != null)
                    {
                        structure.Add(new EmployeeStructure
                        {
                            EmployeeId = emp.Id,
                            SuperiorId = currentSuperior.Id,
                            SuperiorLevel = level
                        });

                        // Move to the next level up
                        currentSuperior = employees.FirstOrDefault(e => e.Id == currentSuperior.SuperiorId);
                        level++;

                        // Prevent infinite loops (safety check)
                        if (level > 100)
                        {
                            _logger?.LogError("Circular reference detected for employee {EmployeeId}. " +
                                "Hierarchy depth exceeds 100 levels", emp.Id);
                            break;
                        }
                    }
                }

                _logger?.LogInformation("Successfully built organizational structure with {RelationCount} relations",
                    structure.Count);

                return structure;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error building organizational structure");
                throw;
            }
        }

        /// <summary>
        /// Gets the hierarchy level of a superior-subordinate relationship
        /// </summary>
        /// <param name="structure">The organizational structure</param>
        /// <param name="employeeId">The employee ID</param>
        /// <param name="superiorId">The superior ID</param>
        /// <returns>The level of the superior (1=direct, 2=indirect, etc.) or null if relation not found</returns>
        public static int? GetSuperiorRowOfEmployee(
            List<EmployeeStructure> structure,
            int employeeId,
            int superiorId)
        {
            if (structure == null || !structure.Any())
                return null;

            if (employeeId <= 0 || superiorId <= 0)
                throw new ArgumentException("Employee and Superior IDs must be positive integers");

            try
            {
                var relation = structure.FirstOrDefault(s =>
                    s.EmployeeId == employeeId && s.SuperiorId == superiorId);

                return relation?.SuperiorLevel;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Error retrieving superior row for employee {employeeId} and superior {superiorId}", ex);
            }
        }
    }
}