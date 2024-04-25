using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeroHunger.DTOs;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class EmployeeController : Controller
    {
        ZeroHungerContext db = new ZeroHungerContext();

        [HttpGet]
        public IActionResult Login()
        {
            return View(new EmployeeDTO());
        }

        [HttpPost]
        public IActionResult Login(EmployeeDTO empLogin)
        {
            if (empLogin.Email == null || empLogin.Password == null)
            {
                ViewBag.ErrMsg = "Please fill up the form properly";
                return View(empLogin);
            }


            var employee = (from emp in db.Employees where emp.Email.Equals(empLogin.Email) && emp.Password.Equals(empLogin.Password) select emp).SingleOrDefault();

            if (employee == null)
            {
                ViewBag.ErrMsg = "Incorrect Email or Password. Please try again.";
                return View(empLogin);
            }

            HttpContext.Session.SetString("EmpId", employee.EmpId.ToString());
            return RedirectToAction("Dashboard");
        }

        [EmployeeAccess]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [EmployeeAccess]
        public IActionResult Dashboard()
        {
            var empId = Int32.Parse(HttpContext.Session.GetString("EmpId"));

            var requests = (from req in db.Requests where req.EmpId == empId select req).ToList();

            return View(RestaurantController.Convert(requests));
        }

        [EmployeeAccess]
        public IActionResult RequestDetails(int id)
        {
            Request request = db.Requests
                            .Include(r => r.Emp)
                            .Include(r => r.Res)
                            .Include(r => r.Foods)
                            .SingleOrDefault(r => r.ReqId == id);

            return View(request);
        }


        [HttpGet]
        [EmployeeAccess]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [EmployeeAccess]
        public IActionResult ChangePassword(string oldPass, string newPass)
        {
            if (oldPass == null || newPass == null)
            {
                ViewBag.ErrMsg = "Please fill up the form properly";
                return View();
            }

            var resId = Int32.Parse(HttpContext.Session.GetString("EmpId"));

            var employee = db.Employees.Find(resId);
            if (employee.Password == oldPass)
            {
                employee.Password = newPass;
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.ErrMsg = "Incorrect password";
                return View();
            }

        }

        [EmployeeAccess]
        public IActionResult Collected(int id)
        {
            var request = db.Requests.Find(id);
            request.Status = "Collected";
            request.CollectTime = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [EmployeeAccess]
        public IActionResult Completed(int reqId, float totalCost)
        {
            if(totalCost > 0)
            {
                var request = db.Requests.Find(reqId);
                request.Status = "Completed";
                request.TotalCost = totalCost;
                request.CompleteTime = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }


        public static Employee Convert(EmployeeDTO emp)
        {
            return new Employee
            {
                EmpId = emp.EmpId,
                EmpName = emp.EmpName,
                Email = emp.Email,
                Password = emp.Password,
                Salary = emp.Salary,
                Address = emp.Address,
            };
        }
    }
}
