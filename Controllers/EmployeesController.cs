using Microsoft.AspNetCore.Mvc;

namespace DbStructureEmployees.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
