using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProje.Models;
using MyProje.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyProje.DAL.User;
using System.Text;
using System.Security.Cryptography;
using MyProje.Models.User;
using MyProje.Models.City;

namespace MyProje.Controllers
{
    public class UserController : Controller
    {
        private readonly UserBL _userBL;
        private readonly CityBL _cityBL;
        public UserController(UserBL userBL, CityBL cityBL)
        {
            _userBL = userBL;
            _cityBL = cityBL;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("IsLoggined") == "1")
            {
                return new LocalRedirectResult("/");
            }
            HttpContext.Session.SetString("IsLoggined", "0");
            HttpContext.Session.SetString("IsAdmin", "0");
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserRequest.List credentials)
        {
            if (credentials.Name == null || credentials.Password == null)
            {
                if (credentials.Name == null)
                {
                    ViewData["UserNameEmpty"] = "Kullanıcı adı boş bırakılamaz.";
                }

                if (credentials.Password == null)
                {
                    ViewData["UserPassEmpty"] = "Şifre boş bırakılamaz.";
                }
                return View();
            }

            credentials.Password = GetMd5Hash(credentials.Password);

            var response = _userBL.List(new UserRequest.List
            {
                Name = credentials.Name,
                Password = credentials.Password
            });

            var EntityList = (List<UserResponse.List>)response.Result;
            var Entity = EntityList.FirstOrDefault();

            if (!response.Success)
            {
                ViewData["UserNameError"] = response.Message;
                return View();
            }
            else if (response.Success && Entity.Name == credentials.Name && Entity.Password != credentials.Password)
            {
                ViewData["PasswordError"] = "Şifre hatalı!";
                return View();
            }

            if (Entity.IsAdmin == true)
            {
                HttpContext.Session.SetString("IsAdmin", "1");
            }

            HttpContext.Session.SetInt32("UserId", Entity.Id);
            HttpContext.Session.SetString("IsLoggined", "1");
            HttpContext.Session.SetString("UName", Entity.Name);

            return new LocalRedirectResult("/User/Profil");
        }
        public IActionResult Profil()
        {

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");

            if (ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var response = _userBL.List(new UserRequest.List { Id = ViewBag.UserId, IsInclude = true });
            var EntityList = (List<UserResponse.List>)response.Result;
            var Entity = EntityList.FirstOrDefault();

            ViewBag.Name = Entity.Name;
            ViewBag.Email = Entity.Email;
            ViewBag.City = Entity.District.City.Name;
            ViewBag.District = Entity.District.Name;


            return View();
        }
        public IActionResult List()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");

            if (ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var response = _userBL.List(new UserRequest.List { RowCount = 99999, GetChildrenCount = true, IsInclude = true });

            if (!response.Success)
            {
                ViewData["Error"] = response.Message;
                return View();
            }
             
            return View((List<UserResponse.List>)response.Result);
        }
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");
            if (ViewBag.IsLoggined == "1")
            {
                return new LocalRedirectResult("/");
            }
            var response = _cityBL.List(new CityRequest.List { RowCount = 999 });
            if (!response.Success)
            {
                ViewData["Error"] = response.Message;
                return View();
            }
            return View((List<CityResponse.List>)response.Result);
        }
        [HttpPost]
        public IActionResult Register(UserRequest.Create credentials)
        {
            var cityList = _cityBL.List(new CityRequest.List { RowCount = 999 });
            if (!cityList.Success)
            {
                ViewData["Error"] = cityList.Message;
                return View();
            }
            if (credentials.Name == null || credentials.Password == null ||
                credentials.Email == null || credentials.Title == null ||
                credentials.DistrictId == 0 || credentials.Neighborhood == null ||
                credentials.Street == null || credentials.BuildingNo == null || credentials.AparmentNo == null)
            {
                if (credentials.Name == null)
                {
                    ViewData["emptyUsername"] = "Kullanıcı adı boş bırakılmaz.";
                }
                if (credentials.Password == null)
                {
                    ViewData["emptyPassword"] = "Şifre boş bırakılmaz.";
                }
                if (credentials.Email == null)
                {
                    ViewData["emptyEmail"] = "E-mail adresi boş bırakılmaz.";
                }
                if (credentials.Title == null)
                {
                    ViewData["emptyTitle"] = "Başlık boş bırakılamaz.";
                }
                if (credentials.DistrictId == 0)
                {
                    ViewData["emptyDistrictName"] = "İlçe boş bırakılamaz.";
                }
                if (credentials.Neighborhood == null)
                {
                    ViewData["emptyNeighborhood"] = "Mahalle boş bırakılmaz.";
                }
                if (credentials.Street == null)
                {
                    ViewData["emptyStreet"] = "Sokak boş bırakılmaz.";
                }
                if (credentials.BuildingNo == null)
                {
                    ViewData["emptyBuildingNo"] = "Bina no boş bırakılmaz.";
                }
                if (credentials.AparmentNo == null)
                {
                    ViewData["emptyAparmentNo"] = "Daire no boş bırakılmaz.";
                }
                return View((List<CityResponse.List>)cityList.Result);
            }

            var UserListResponse = _userBL.List(new UserRequest.List
            {
                Name = credentials.Name,
                Email = credentials.Email
            });

            if (UserListResponse.Success)
            {
                var EntityList = (List<UserResponse.List>)UserListResponse.Result;
                var Entity = EntityList.FirstOrDefault();

                if (Entity.Name == credentials.Name)
                {
                    ViewData["RegisterControlUsername"] = "Kullanıcı adı kullanılmış.";
                    return View((List<CityResponse.List>)cityList.Result);
                }
                else if (Entity.Email == credentials.Email)
                {
                    ViewData["RegisterControlEmail"] = "E-mail adresi kullanılmış.";
                    return View((List<CityResponse.List>)cityList.Result);
                }

            }
            credentials.Password = GetMd5Hash(credentials.Password);
            var response = _userBL.Create(credentials);
            if (!response.Success)
            {
                ViewData["Error"] = response.Message;
                return View((List<CityResponse.List>)cityList.Result);
            }
            var userData = (UserResponse.List)response.Result;

            HttpContext.Session.SetInt32("UserId", userData.Id);
            HttpContext.Session.SetString("IsLoggined", "1");
            HttpContext.Session.SetString("UName", userData.Name);

            return new LocalRedirectResult("/User/Profil");
        }


        public IActionResult Exit()
        {
            HttpContext.Session.SetString("IsLoggined", "0");
            HttpContext.Session.SetString("IsAdmin", "0");

            return RedirectToAction("Index", "Home");
        }


        public IActionResult Edit()
        {

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");

            if (ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            return View();
        }
        [HttpPost]
        public IActionResult Edit(UserRequest.Edit credentials)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");


            if (credentials.Password == null || credentials.NewPassword == null || credentials.NewPasswordAgain == null)
            {
                if (credentials.Password == null)
                {
                    ViewData["emptyPassword"] = " Mevcut şifre boş bırakalamaz.";
                }

                if (credentials.NewPassword == null || credentials.NewPasswordAgain == null)
                {
                    ViewData["emptynewPassword"] = "Yeni şifre boş bırakılamaz.";
                }
                return View();
            }

            var userData = _userBL.List(new UserRequest.List
            {
                Id = ViewBag.UserId,
                Password = GetMd5Hash(credentials.Password)
            });

            if (!userData.Success)
            {
                ViewData["errorPassword"] = "Mevcut şifre hatalı."; return View();
            }

            if (credentials.Password == credentials.NewPassword)
            {
                ViewData["errornewPassword"] = "Yeni şifreniz mevcut şifreniz ile aynı olamaz."; return View();
            }

            if (credentials.NewPassword != credentials.NewPasswordAgain)
            {
                ViewData["errorAgainnewPassword"] = "Yeni şifreniz tekrarı ile uyuşmuyor."; return View();
            }

            credentials.Password = GetMd5Hash(credentials.NewPassword);

            var response = _userBL.Edit(credentials);

            ViewData["Success"] = response.Message;

            return View();
        }

        [HttpPost]
        public IActionResult Delete(int UserId)
        {
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");

            if (ViewBag.IsAdmin != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var response = _userBL.Delete(new UserRequest.Delete { Id = UserId });

            if (!response.Success)
            {
                ViewData["Error"] = response.Message;
            }

            return new RedirectResult("~/User/List");
        }
        [HttpPost]
        public IActionResult MakeAdmin(int Id)
        {
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");

            if (ViewBag.IsAdmin != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var response = _userBL.Edit(new UserRequest.Edit { Id = Id, ChangeAdmin = true });

            if (!response.Success)
            {
                ViewData["Error"] = response.Message;
            }

            return new RedirectResult("~/User/List");
        }










        public string GetMd5Hash(string input)
        {
            // Convert the input string to a byte array and compute the hash.

            using MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
