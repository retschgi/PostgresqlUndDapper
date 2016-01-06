using System.Collections.Generic;
using PostgresqlUndDapper.Repositories;
using Microsoft.AspNet.Mvc;
using PostgresqlUndDapper.Models;


namespace PostgresqlUndDapper.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Employee> result = _employeeRepository.Get();
            return View(result);
        }
    }
}
