using Microsoft.EntityFrameworkCore;
using MyProje.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MyProje.Models.City;
using MyProje.Models;

namespace MyProje.DAL.City
{
    public class CityDA
    {
        private readonly Context _db;
        public CityDA(Context db)
        {
            _db = db;
        }
        public Response List(CityRequest.List credentials)
        {
            var response = new Response();
            try
            {
                var cityEntities = _db.Citys.AsQueryable();
                var MaxCount = cityEntities.Count();

                if (credentials.Name != null && credentials.Code != null)
                {
                    cityEntities = cityEntities.Where(c => c.CityName == credentials.Name || c.CityCode == credentials.Code);
                }
                else if (credentials.Name != null)
                {
                    cityEntities = cityEntities.Where(c => c.CityName == credentials.Name);
                }
                else if (credentials.Id != null)
                {
                    cityEntities = cityEntities.Where(c => c.Id == credentials.Id);
                }

                if (credentials.IsOrderByAsc)
                    cityEntities = cityEntities.OrderBy(c => c.CityCode);
                else
                    cityEntities = cityEntities.OrderByDescending(c => c.CityCode);

                cityEntities = cityEntities.Skip(credentials.RowCount * (credentials.Page - 1)).Take(credentials.RowCount);

                var cityList = cityEntities.Select(c => new CityResponse.List
                {
                    Id = c.Id,
                    CityCode = c.CityCode,
                    CityName = c.CityName,
                    DistrictCount = credentials.GetChildrenCount ? c.Districts.Count() : (int?)null
                }).ToList();

                var count = cityList.Count();
                if (count > 0)
                {
                    response.Success = true;
                    response.Count = count;
                    response.MaxCount = MaxCount;
                    response.Message = count + " kayıt bulundu.";
                    response.Result = cityList;
                }
                else
                {
                    response.Message = "Kayıt bulunamadı.";
                }

            }
            catch (Exception error)
            {
                response.Message = "İşlem başarısız, sunucuda sorun oluştu.";
                response.ExceptionMessage = error.InnerException.Message;
            }

            return response;
        }
        public Response Create(CityRequest.Create credentials)
        {
            var response = new Response();
            try
            {
                var Entity = _db.Citys.Where(a => a.CityCode == credentials.Code || a.CityName == credentials.Name).FirstOrDefault();

                if (Entity != null)
                {
                    response.Message = "Kayıt mevcut";
                    return response;
                }

                var newEntity = new CityEntity
                {
                    CityName = credentials.Name,
                    CityCode = credentials.Code
                };

                _db.Citys.Add(newEntity);
                _db.SaveChanges();
                response.Success = true;
                response.Message = "Kayıt Başarılı";
            }
            catch (Exception error)
            {
                response.Message = "İşlem başarısız, sunucuda sorun oluştu.";
                response.ExceptionMessage = error.InnerException.Message;

            }

            return response;
        }
        public Response Delete(CityRequest.Delete credentials)
        {
            var response = new Response();
            try
            {
                var Entity = _db.Citys.Where(a => a.Id == credentials.Id).FirstOrDefault();

                if (Entity == null)
                {
                    response.Message = "Kayıt bulunamadı";
                    return response;
                }

                _db.Citys.Remove(Entity);
                _db.SaveChanges();
                response.Success = true;
                response.Message = "Kayıt başarıyla silindi";
            }
            catch (Exception error)
            {
                response.Message = "İşlem başarısız, sunucuda sorun oluştu.";
                response.ExceptionMessage = error.InnerException.Message;
            }
            return response;
        }

        public Response Edit(CityRequest.Edit credentials)
        {
            var response = new Response();
            try
            {
                var Entity = _db.Citys.Where(a => a.Id == credentials.Id).FirstOrDefault();

                if (Entity == null)
                {
                    response.Message = "Kayıt bulunamadı";
                    return response;
                }

                Entity.CityName = credentials.Name;
                Entity.CityCode = credentials.Code;

                _db.SaveChanges();


                response.Success = true;
                response.Message = "İşlem Başarılı";
            }
            catch (Exception error)
            {
                response.Message = "İşlem başarısız, sunucuda sorun oluştu.";
                response.ExceptionMessage = error.InnerException.Message;
            }

            return response;
        }
    }
}
