using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProje.BusinessLogic;
using MyProje.Models.District;
using MyProje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.Controllers
{
    public class DistrictController : Controller
    {
        private readonly DistrictBL _district;

        public DistrictController(DistrictBL district)
        {
            _district = district;
        }
        [HttpGet]
        public IActionResult List(int CityId)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");
            if (ViewBag.IsAdmin != "1" && ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }
            var response = _district.List(new DistrictRequest.List
            {
                CityId = CityId,
                GetChildrenCount = true,
                RowCount = 100
            });

            ViewBag.CityId = CityId;

            if (!response.Success)
            {
                if (response.Message=="Kayıt bulunamadı.")
                {
                    return View((List<DistrictResponse.List>)response.Result);
                }
                return new LocalRedirectResult("/City/List");
            }
           
            return View((List<DistrictResponse.List>)response.Result);
        }
        [HttpGet]
        public IActionResult Edit(int Id, int CityId)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");

            if (ViewBag.IsAdmin != "1" && ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var response = _district.List(new DistrictRequest.List { Id = Id });

            if (!response.Success)
            {
                return new RedirectResult("/District/List?CityId=" + CityId);
            }

            var EntityList = (List<DistrictResponse.List>)response.Result;
            var Entity = EntityList.FirstOrDefault();

            ViewBag.DistrictName = Entity.DistrictName;
            ViewBag.CityId = CityId;

            return View();
        }
        [HttpPost]
        public IActionResult Edit(DistrictRequest.Edit credentials)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");

            if (ViewBag.IsAdmin != "1" && ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var response = _district.List(new DistrictRequest.List { Id = credentials.Id });

            if (!response.Success)
            {
                return new RedirectResult("/District/List?CityId=" + credentials.CityId);
            }

            var EntityList = (List<DistrictResponse.List>)response.Result;
            var Entity = EntityList.FirstOrDefault();

            ViewBag.DistrictName = Entity.DistrictName;
            ViewBag.CityId = credentials.CityId;

            if (credentials.Name == null)
            {
                ViewData["DistrictNameInfo"] = "İlçe adı boş bırakılamaz.";
                return View();
            }

            var result = _district.Edit(credentials);
            if (!result.Success)
            {
                ViewData["Success"] = result.Message;
            }

            return new RedirectResult("/District/List?CityId=" + credentials.CityId);
        }
        [HttpGet]
        public IActionResult Create(int CityId)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");
            if (ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }
            ViewBag.CityId = CityId;
            return View();
        }
        [HttpPost]
        public IActionResult Create(DistrictRequest.Create credentials)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");

            if (ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }
            ViewBag.CityId = credentials.CityId;
            if (credentials.Name == null)
            {
                ViewData["emptyDistrictName"] = "İlçe adı boş bırakılamaz.";
                return View();
            }

            var response = _district.Create(credentials);
            ViewData["Success"] = response.Message;
            return View();

        }

        [HttpPost]
        public IActionResult Delete(DistrictRequest.Delete credentials)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");

            if (ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var result = _district.Delete(credentials);
            ViewData["Success"] = result.Message;

            return new RedirectResult("/District/List?CityId=" + credentials.CityId);
        }

        public List<DistrictResponse.List> DistrictByCityList(int CityId)
        {
            var list = _district.List(new DistrictRequest.List { CityId = CityId,RowCount=100 });

            return (List<DistrictResponse.List>)list.Result;
        }
    }
}
