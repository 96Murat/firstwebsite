using MyProje.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MyProje.Models;
using MyProje.Models.District;

namespace MyProje.DAL.District
{
    public class DistrictDA
    {
        private readonly Context _db;
        public DistrictDA(Context db)
        {
            _db = db;
        }

        public Response List(DistrictRequest.List credentials)
        {
            var response = new Response();
            try
            {
                var districtEntities = _db.Districts.AsQueryable();
                var MaxCount = districtEntities.Count();

                if (credentials.Id != null && credentials.CityId != null)
                {
                    districtEntities = districtEntities.Where(d => d.CityId == credentials.CityId && d.Id == credentials.Id);
                }
                else if (credentials.Id != null)
                {
                    districtEntities = districtEntities.Where(d => d.Id == credentials.Id);
                }
                else if (credentials.Name != null)
                {
                    districtEntities = districtEntities.Where(d => d.DistrictName == credentials.Name);
                }
                else if (credentials.CityId != null)
                {
                    districtEntities = districtEntities.Where(d => d.CityId == credentials.CityId);
                }
                if (credentials.IsOrderByAsc)
                    districtEntities = districtEntities.OrderBy(d => d.DistrictName);
                else
                    districtEntities = districtEntities.OrderByDescending(d => d.DistrictName);

                districtEntities = districtEntities.Skip(credentials.RowCount * (credentials.Page - 1)).Take(credentials.RowCount);

                var districtList = districtEntities.Select(d => new DistrictResponse.List
                {
                    Id = d.Id,
                    DistrictName = d.DistrictName,
                    CityId = d.CityId,
                    UserCount=credentials.GetChildrenCount ? d.Users.Count() : (int?)null,
                    AddressCount= credentials.GetChildrenCount ? d.UserAddresses.Count() : (int?)null,
                    City = new DistrictResponse.CityList
                    {
                        Code = d.City.CityCode,
                        Name = d.City.CityName
                    }
                }).ToList();

                var count = districtList.Count();
                if (count > 0)
                {
                    response.Success = true;
                    response.Count = count;
                    response.MaxCount = MaxCount;
                    response.Message = count + " kayıt bulundu.";
                    response.Result = districtList;
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
        public Response Create(DistrictRequest.Create credentials)
        {
            var response = new Response();
            try
            {
                var Entity = _db.Districts.Where(a => a.DistrictName == credentials.Name).FirstOrDefault();

                if (Entity != null)
                {
                    response.Message = "Kayıt mevcut";
                    return response;
                }

                var newEntity = new DistrictEntity
                {
                    DistrictName = credentials.Name,
                    CityId=credentials.CityId
                };

                _db.Districts.Add(newEntity);
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
        public Response Delete(DistrictRequest.Delete credentials)
        {
            var response = new Response();
            try
            {
                var Entity = _db.Districts.Where(a => a.Id == credentials.Id).FirstOrDefault();

                if (Entity == null)
                {
                    response.Message = "Kayıt bulunamadı";
                    return response;
                }
                _db.Districts.Remove(Entity);
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
        public Response Edit(DistrictRequest.Edit credentials)
        {
            var response = new Response();
            try
            {
                var Entity = _db.Districts.Where(a => a.Id == credentials.Id).FirstOrDefault();

                if (Entity == null)
                {
                    response.Message = "Kayıt bulunamadı";
                    return response;
                }

                Entity.DistrictName = credentials.Name;
                Entity.CityId = credentials.CityId;

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