
using Microsoft.AspNetCore.Mvc;
using Todo_list.Models;
using Todo_list.Services;


namespace Todo_list.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    

    public class AuthController : Controller
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        // GET LOGIN
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST LOGIN
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            try
            {
                var user = _service.Login(email, password);
                

                //LƯU SESSION
                HttpContext.Session.SetString("username", user.Username);
                

                return RedirectToAction("Index", "TodoTasks");
            }
            catch
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View();
            }
        }

        // GET REGISTER
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST REGISTER
        [HttpPost]
        public IActionResult Register(string username, string email, string password)
        {
            try
            {
                _service.Register(username, email, password);
                return RedirectToAction("Login");
            }
            catch
            {
                ViewBag.Error = "Email đã tồn tại";
                return View();
            }
        }
    }
}