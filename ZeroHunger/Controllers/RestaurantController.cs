using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ZeroHunger.DTOs;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class RestaurantController : Controller
    {
        ZeroHungerContext db = new ZeroHungerContext();


        [HttpGet]
        public IActionResult Login()
        {
            return View(new RestaurantDTO());
        }

        [HttpPost]
        public IActionResult Login(RestaurantDTO resLogin)
        {
            if(resLogin.Email == null || resLogin.Password == null)
            {
                ViewBag.ErrMsg = "Please fill up the form properly";
                return View(resLogin);
            }

            var restaurant = (from res in db.Restaurants where res.Email.Equals(resLogin.Email) && res.Password.Equals(resLogin.Password) select res).SingleOrDefault();

            if(restaurant == null)
            {
                ViewBag.ErrMsg = "Incorrect Email or Password. Please try again.";
                return View(resLogin);
            }

            HttpContext.Session.SetString("ResId", restaurant.ResId.ToString());
            return RedirectToAction("Dashboard");
        }


        [HttpGet]
        public IActionResult Signup()
        {
            return View(new RestaurantDTO());
        }

        [HttpPost]
        public IActionResult Signup(RestaurantDTO resSignup)
        {
            if(ModelState.IsValid)
            {
                Restaurant res = Convert(resSignup);
                db.Restaurants.Add(res);
                db.SaveChanges();

                HttpContext.Session.SetString("ResId", res.ResId.ToString());
                return RedirectToAction("Dashboard");
            }

            return View(resSignup);
        }


        [RestaurantAccess]
        public IActionResult Dashboard()
        {
            var resId = Int32.Parse(HttpContext.Session.GetString("ResId"));

            var requests = (from req in db.Requests where req.ResId == resId select req).ToList();
            return View(Convert(requests));
        }


        [RestaurantAccess]
        public IActionResult RequestDetails(int id)
        {
            // Request request = db.Requests.Find(id);
            Request request = db.Requests
                            .Include(r => r.Emp)
                            .Include(r => r.Res)
                            .Include(r => r.Foods)
                            .SingleOrDefault(r => r.ReqId == id);

            return View(request);
        }

        [HttpGet]
        [RestaurantAccess]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [RestaurantAccess]
        public IActionResult ChangePassword(string oldPass, string newPass)
        {
            if (oldPass == null || newPass == null)
            {
                ViewBag.ErrMsg = "Please fill up the form properly";
                return View();
            }

            var resId = Int32.Parse(HttpContext.Session.GetString("ResId"));

            var restaurant = db.Restaurants.Find(resId);

            if(restaurant.Password == oldPass)
            {
                restaurant.Password = newPass;
                db.SaveChanges();
                ViewBag.SuccessMsg = "Password changed Successfully";
                return View();
            }
            else
            {
                ViewBag.ErrMsg = "Incorrect password";
                return View();
            }

        }


        [HttpGet]
        [RestaurantAccess]
        public IActionResult AddFood()
        {
            return View(new FoodDTO());
        }

        [HttpPost]
        [RestaurantAccess]
        public IActionResult AddFood(FoodDTO food)
        {
            if (ModelState.IsValid)
            {
                List<FoodDTO> foods = null;
                var sessionData = HttpContext.Session.GetString("Foods");

                if(sessionData == null)
                {
                    foods = new List<FoodDTO>();
                }
                else
                {
                    foods = JsonSerializer.Deserialize<List<FoodDTO>>(sessionData);
                }

                foods.Add(food);

                var jsonData = JsonSerializer.Serialize(foods);
                HttpContext.Session.SetString("Foods", jsonData);

                return RedirectToAction("CreateRequest");
            }

            return View(food);
        }

        [RestaurantAccess]
        public IActionResult RemoveFood(string foodName, int quantity)
        {
            var sessionData = HttpContext.Session.GetString("Foods");
            var foods = JsonSerializer.Deserialize<List<FoodDTO>>(sessionData);

            var food = (from f in foods where f.FoodName == foodName && f.Quantity == quantity select f).SingleOrDefault();

            foods.Remove(food);

            var jsonData = JsonSerializer.Serialize(foods);
            HttpContext.Session.SetString("Foods", jsonData);

            return RedirectToAction("CreateRequest");
        }

        [RestaurantAccess]
        public IActionResult CreateRequest()
        {
            List<FoodDTO> foods = null;
            var sessionData = HttpContext.Session.GetString("Foods");

            if (sessionData == null)
            {
                foods = new List<FoodDTO>();
            }
            else
            {
                foods = JsonSerializer.Deserialize<List<FoodDTO>>(sessionData);
            }

            return View(foods);
        }

        [HttpGet]
        [RestaurantAccess]
        public IActionResult ConfirmRequest()
        {
            return View();
        }

        [HttpPost]
        [RestaurantAccess]
        public IActionResult ConfirmRequest(DateTime preTime)
        {
            if(preTime >= DateTime.Now)
            {
                Request request = new Request();
                request.ResId = Int32.Parse(HttpContext.Session.GetString("ResId"));
                request.ReqTime = DateTime.Now;
                request.PreserveTime = preTime;
                request.EmpId = 1;
                request.Status = "Requested";
                request.TotalCost = 0;

                db.Requests.Add(request);
                db.SaveChanges();


                var sessionData = HttpContext.Session.GetString("Foods");
                var foods = JsonSerializer.Deserialize<List<FoodDTO>>(sessionData);

                foreach(var food in foods)
                {
                    var f = new Food();
                    f.FoodName = food.FoodName;
                    f.Quantity = food.Quantity;
                    f.ReqId = request.ReqId;

                    db.Foods.Add(f);
                }
                db.SaveChanges();

                HttpContext.Session.Remove("Foods");
                return RedirectToAction("Dashboard");
            }

            ViewBag.ErrMsg = "Please select a date time";
            return View();
        }



        public static Restaurant Convert (RestaurantDTO res)
        {
            return new Restaurant
            {
                ResId = res.ResId,
                ResName = res.ResName,
                Email = res.Email,
                Password = res.Password,
                Address = res.Address,
            };
        }

        public static Request Convert(RequestDTO req)
        {
            return new Request
            {
                ReqId = req.ReqId,
                ResId = req.ResId,
                ReqTime = req.ReqTime,
                PreserveTime = req.PreserveTime,
                EmpId = req.EmpId,
                CollectTime = req.CollectTime,
                CompleteTime = req.CompleteTime,
                Status = req.Status,
                TotalCost = req.TotalCost,
            };
        }

        public static RequestDTO Convert(Request req)
        {
            return new RequestDTO
            {
                ReqId = req.ReqId,
                ResId = req.ResId,
                ReqTime = req.ReqTime,
                PreserveTime = req.PreserveTime,
                EmpId = req.EmpId,
                CollectTime = req.CollectTime,
                CompleteTime = req.CompleteTime,
                Status = req.Status,
                TotalCost = req.TotalCost,
            };
        }

        public static List<RequestDTO> Convert(List<Request> data)
        {
            var list = new List<RequestDTO>();
            foreach (var item in data)
            {
                list.Add(Convert(item));
            }
            return list;
        }

    }
}
