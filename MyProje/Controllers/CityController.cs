using Microsoft.AspNetCore.Mvc;
using MyProje.BusinessLogic;
using MyProje.Models.City;
using MyProje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MyProje.Controllers
{
    public class CityController : Controller
    {

        private readonly CityBL _city;

        public CityController(CityBL city)
        {
            _city = city;
        }

        [HttpGet]
        public IActionResult List()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");

            if (ViewBag.IsAdmin != "1" && ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            _ = ApiRequestAsync();



            var Response = _city.List(new CityRequest.List
            {
                RowCount = 99999,
                GetChildrenCount = true
            });

            if (!Response.Success)
            {
                return View(Response.Message);
            }
            return View((List<CityResponse.List>)Response.Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");
            if (ViewBag.IsAdmin != "1" && ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(CityRequest.Create credentials)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");

            if (ViewBag.IsAdmin != "1" && ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            if (credentials.Code == 0 || credentials.Name == null)
            {
                if (credentials.Code == 0)
                {
                    ViewData["emptyCityCode"] = "Şehir kodu boş bırakılmaz.";
                }
                if (credentials.Name == null)
                {
                    ViewData["emptyCityName"] = "Şehir adı boş bırakılmaz.";
                }

                return View();
            }

            var response = _city.Create(credentials);
            if (!response.Success)
            {
                ViewData["Success"] = response.Message;
                return View();
            }

            return new LocalRedirectResult("/City/List");
        }

        [HttpPost]
        public IActionResult Delete(CityRequest.Delete credentials)
        {
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");

            if (ViewBag.IsAdmin != "1" && ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var response = _city.Delete(credentials);
            if (!response.Success)
            {
                ViewData["Success"] = response.Message;
            }

            return new LocalRedirectResult("/City/List");
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");
            if (ViewBag.IsAdmin != "1" && ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var response = _city.List(new CityRequest.List { Id = Id });

            if (!response.Success)
            {
                ViewData["Success"] = response.Message;
                return new RedirectResult("~/City/List");
            }

            var EntityList = (List<CityResponse.List>)response.Result;
            var Entity = EntityList.FirstOrDefault();

            ViewBag.CityCode = Entity.CityCode;
            ViewBag.CityName = Entity.CityName;

            return View();
        }

        [HttpPost]
        public IActionResult Edit(CityRequest.Edit credentials)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");

            if (ViewBag.IsAdmin != "1" && ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            if (credentials.Code == 0 || credentials.Name == null)
            {
                if (credentials.Code == 0)
                {
                    ViewData["CityCodeInfo"] = "Şehir kodu boş bırakılmaz.";
                }
                if (credentials.Name == null)
                {
                    ViewData["CityCodeInfo"] = "Şehir adı boş bırakılmaz.";
                }
                return View();
            }

            var result = _city.Edit(credentials);
            if (!result.Success)
            {
                ViewData["Success"] = result.Message;
                return View();
            }
            return new LocalRedirectResult("~/City/List");
        }




        public async Task<Response> ApiRequestAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:44336/city/list"),
                Content = new StringContent("{\n  \"GetChildrenCount\":true\n}")
                {
                    Headers =
                            {
                            ContentType = new MediaTypeHeaderValue("application/json")
                                }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
            }


            return null;
        }
    }
}
