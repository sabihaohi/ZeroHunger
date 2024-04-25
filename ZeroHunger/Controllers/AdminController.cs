using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeroHunger.DTOs;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{

    public class AdminController : Controller
    {
        ZeroHungerContext db = new ZeroHungerContext();


        [HttpGet]
        public IActionResult Login()
        {
            return View(new AdminDTO());
        }

        [HttpPost]
        public IActionResult Login(AdminDTO adminLogin)
        {
            if (adminLogin.Email == null || adminLogin.Password == null)
            {
                ViewBag.ErrMsg = "Please fill up the form properly";
                return View(adminLogin);
            }

            var admin = (from a in db.Admins where a.Email.Equals(adminLogin.Email) && a.Password.Equals(adminLogin.Password) select a).SingleOrDefault();

            if (admin == null)
            {
                ViewBag.ErrMsg = "Incorrect Email or Password. Please try again.";
                return View(adminLogin);
            }

            HttpContext.Session.SetString("AdminId", admin.AdminId.ToString());
            return RedirectToAction("Dashboard");
        }

        [AdminAccess]
        public IActionResult Dashboard()
        {
            return View();
        }


        [HttpGet]
        [AdminAccess]
        public IActionResult AddEmployee()
        {
            return View(new EmployeeDTO());
        }

        [HttpPost]
        [AdminAccess]
        public IActionResult AddEmployee(EmployeeDTO emp)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(EmployeeController.Convert(emp));
                db.SaveChanges();

                return RedirectToAction("AllEmployees");
            }

            return View(emp);
        }

        [AdminAccess]
        public IActionResult AllEmployees()
        {
            var employees = db.Employees.Include(e => e.Requests).ToList();
            return View(employees);
        }

        [AdminAccess]
        public IActionResult AllRestaurants()
        {
            var restaurants = db.Restaurants.Include(r => r.Requests).ToList();
            return View(restaurants);
        }

        [AdminAccess]
        public IActionResult AllRequests()
        {
            var requests = db.Requests.Include(r => r.Res).Include(r => r.Emp).ToList();
            return View(requests);
        }

        [AdminAccess]
        public IActionResult RequestDetails(int id)
        {
            var employees = db.Employees.ToList();
            ViewBag.Employees = employees;

            Request request = db.Requests
                            .Include(r => r.Emp)
                            .Include(r => r.Res)
                            .Include(r => r.Foods)
                            .SingleOrDefault(r => r.ReqId == id);

            return View(request);
        }

        [HttpPost]
        [AdminAccess]
        public IActionResult AssignEmp(int reqId, int empId)
        {
            if(empId != 1)
            {
                var req = db.Requests.Find(reqId);

                req.EmpId = empId;
                req.Status = "Assigned";

                db.SaveChanges();
            }
            return RedirectToAction("AllRequests");
        }

        [AdminAccess]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
