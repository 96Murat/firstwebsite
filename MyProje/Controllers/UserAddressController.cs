using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProje.BusinessLogic;
using MyProje.Models;
using MyProje.Models.Address;
using MyProje.Models.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.Controllers
{
    public class UserAddressController : Controller
    {
        private readonly UserAddressBL _userAddressBL;
        private readonly CityBL _cityBL;

        public UserAddressController(UserAddressBL userAddressBL, CityBL cityBL)
        {
            _userAddressBL = userAddressBL;
            _cityBL = cityBL;
        }
        public IActionResult UserAddress()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");
            if (ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }
            var response = _userAddressBL.List(new AddressRequest.List { UserId = ViewBag.UserId, RowCount = 99999 });

            if (!response.Success)
            {
                ViewData["Error"] = response.Message;
                return View();
            }

            return View((List<AddressResponse.List>)response.Result);
        }
        [HttpGet]
        public IActionResult List(int UserId)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");
            if (ViewBag.IsAdmin != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var response = _userAddressBL.List(new AddressRequest.List { UserId = UserId, RowCount=99999 });
            if (!response.Success)
            {
                ViewData["Error"] = response.Message;
                return View();
            }

            return View((List<AddressResponse.List>)response.Result);

        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");
            if (ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var response = _userAddressBL.List(new AddressRequest.List {Id=Id});

            if (!response.Success)
            {
                return new RedirectResult("~/UserAddress/UserAddress");
            }

            var EntityList = (List<AddressResponse.List>)response.Result;
            var Entity = EntityList.FirstOrDefault();

            ViewBag.AddressTitle = Entity.Title;
            ViewBag.DistrictName = Entity.District.Name;
            ViewBag.DistrictId = Entity.DistrictId;
            ViewBag.CityName = Entity.District.City.Name;
            ViewBag.CityId = Entity.District.City.Id;
            ViewBag.Neighborhood = Entity.Neighborhood;
            ViewBag.Street = Entity.Street;
            ViewBag.BuildingNo = Entity.BuildingNo;
            ViewBag.AparmentNo = Entity.AparmentNo;
            ViewBag.UserId = Entity.UserId;

            var cityList = _cityBL.List(new CityRequest.List {RowCount=999});
            if (!cityList.Success)
            {
                ViewData["Error"] = cityList.Message;
                return View();
            }

            return View((List<CityResponse.List>)cityList.Result);
        }
        [HttpPost]
        public IActionResult Edit(AddressRequest.Edit credentials)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");

            if (ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var response = _userAddressBL.List(new AddressRequest.List { Id = credentials.Id });

            if (!response.Success)
            {
                return new RedirectResult("~/UserAddress/UserAddress");
            }

            var EntityList = (List<AddressResponse.List>)response.Result;
            var Entity = EntityList.FirstOrDefault();

            ViewBag.AddressTitle = Entity.Title;
            ViewBag.DistrictName = Entity.District.Name;
            ViewBag.DistrictId = Entity.DistrictId;
            ViewBag.CityName = Entity.District.City.Name;
            ViewBag.CityId = Entity.District.City.Id;
            ViewBag.Neighborhood = Entity.Neighborhood;
            ViewBag.Street = Entity.Street;
            ViewBag.BuildingNo = Entity.BuildingNo;
            ViewBag.AparmentNo = Entity.AparmentNo;
            ViewBag.UserId = Entity.UserId;

            var cityList = _cityBL.List(new CityRequest.List { RowCount = 999 });
            if (!cityList.Success)
            {
                ViewData["Error"] = cityList.Message;
                return View();
            }

            if (credentials.Title == null || credentials.DistrictId <= 0 ||
                credentials.Neighborhood == null || credentials.Street == null ||
                credentials.BuildingNo == null || credentials.AparmentNo == null
                )
            {
                if (credentials.Title == null)
                {
                    ViewData["emptyTitle"] = "Başlık boş bırakılamaz.";
                }
                if (credentials.DistrictId <= 0)
                {
                    ViewData["emptyDistrictName"] = "İlçe seçimi yapınız.";
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

            var addressEdit = _userAddressBL.Edit(credentials);

            if (!addressEdit.Success)
            {
                ViewData["Error"] = addressEdit.Message;
                return View((List<CityResponse.List>)cityList.Result);
            }

            return new RedirectResult("~/UserAddress/UserAddress");
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");
            if (ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var response = _cityBL.List(new CityRequest.List {RowCount=999 });

            if (!response.Success)
            {
                ViewData["Error"] = response.Message;
                return View();
            }

            return View((List<CityResponse.List>)response.Result);
        }

        [HttpPost]
        public IActionResult Create(AddressRequest.Create credentials)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");
            if (ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var cityList = _cityBL.List(new CityRequest.List { RowCount = 999 });

            if (!cityList.Success)
            {
                ViewData["Error"] = cityList.Message;
                return View();
            }

            if (credentials.Title == null || credentials.DistrictId == 0 || credentials.Neighborhood == null
                || credentials.Street == null || credentials.BuildingNo == null || credentials.AparmentNo == null)
            {
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

            credentials.UserId = ViewBag.UserId;

            var response = _userAddressBL.Create(credentials);

            if (!response.Success)
            {
                ViewData["Success"] = response.Message;
                return View((List<CityResponse.List>)cityList.Result);
            }
            
            return new RedirectResult("~/UserAddress/UserAddress");
        }
        public IActionResult Delete(AddressRequest.Delete credentials)
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");

            if (ViewBag.IsLoggined != "1")
            {
                return new LocalRedirectResult("/User/Login");
            }

            var result = _userAddressBL.Delete(credentials);
            ViewData["Success"] = result.Message;

            return new RedirectResult("/UserAddress/UserAddress");
        }
    }
}
