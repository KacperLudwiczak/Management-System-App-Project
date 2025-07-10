using Microsoft.AspNetCore.Mvc;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Controllers
{
    public class TestController : Controller
    {
    public IActionResult Index()
    {
        var model = new TestViewModel
        {
            Name = "Test Model Name"
        };
        return View(model);
    }
    }
}