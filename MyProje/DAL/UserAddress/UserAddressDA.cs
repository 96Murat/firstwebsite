using Microsoft.EntityFrameworkCore;
using MyProje.BusinessLogic;
using MyProje.Models.Address;
using MyProje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MyProje.Models.User;
using MyProje.Models.District;

namespace MyProje.DAL.UserAddress
{
    public class UserAddressDA
    {
        private readonly Context _db;
        public UserAddressDA(Context db)
        {
            _db = db;
        }
        public Response List(AddressRequest.List credentials)
        {
            var response = new Response();
            try
            {
                var addressEntities = _db.UserAddresses.AsQueryable();
                var MaxCount = addressEntities.Count();

                if (credentials.UserId != null && credentials.Title != null)
                {
                    addressEntities = addressEntities.Where(a => a.UserId == credentials.UserId && a.Title == credentials.Title);
                }
                else if (credentials.UserId != null && credentials.DistrictId != null)
                {
                    addressEntities = addressEntities.Where(a => a.UserId == credentials.UserId && a.DistrictId == credentials.DistrictId);
                }
                else if (credentials.UserId != null)
                {
                    addressEntities = addressEntities.Where(a => a.UserId == credentials.UserId);
                }
                if (credentials.Id != null)
                {
                    addressEntities = addressEntities.Where(a => a.Id == credentials.Id);
                }
                if (credentials.IsOrderByAsc)
                    addressEntities = addressEntities.OrderBy(a => a.District.City.CityName);
                else
                    addressEntities = addressEntities.OrderByDescending(a => a.District.City.CityName);

                addressEntities = addressEntities.Skip(credentials.RowCount * (credentials.Page - 1)).Take(credentials.RowCount);

                var addressList = addressEntities.Select(a => new AddressResponse.List
                {
                    Id = a.Id,
                    Title = a.Title,
                    Neighborhood = a.Neighborhood,
                    AparmentNo = a.AparmentNo,
                    BuildingNo = a.BuildingNo,
                    Street = a.Street,
                    DistrictId = a.DistrictId,
                    UserId = a.UserId,
                    District = new UserResponse.District
                    {
                        Name = a.District.DistrictName,
                        City = new DistrictResponse.CityList
                        {
                            Id = a.District.City.Id,
                            Name = a.District.City.CityName
                        }
                    }
                }).ToList();

                var count = addressList.Count();
                if (count > 0)
                {
                    response.Success = true;
                    response.Count = count;
                    response.MaxCount = MaxCount;
                    response.Message = count + " kayıt bulundu.";
                    response.Result = addressList;
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

        public Response Create(AddressRequest.Create credentials)
        {
            var response = new Response();
            try
            {
                var Entity = _db.UserAddresses.Where(a => a.Title == credentials.Title).FirstOrDefault();

                if (Entity != null)
                {
                    response.Message = credentials.Title + " başlıklı kayıt mevcut";
                    return response;
                }

                var newEntity = new AddressEntity
                {
                    Title = credentials.Title,
                    Neighborhood = credentials.Neighborhood,
                    Street = credentials.Street,
                    AparmentNo = credentials.AparmentNo,
                    BuildingNo = credentials.BuildingNo,
                    DistrictId = credentials.DistrictId,
                    UserId = credentials.UserId
                };

                _db.UserAddresses.Add(newEntity);
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

        public Response Delete(AddressRequest.Delete credentials)
        {
            var response = new Response();
            try
            {
                var Entity = _db.UserAddresses.Where(a => a.Id == credentials.Id).FirstOrDefault();

                if (Entity == null)
                {
                    response.Message = "Kayıt bulunamadı";
                    return response;
                }

                _db.UserAddresses.Remove(Entity);
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

        public Response Edit(AddressRequest.Edit credentials)
        {
            var response = new Response();
            try
            {
                var Entity = _db.UserAddresses.Where(a => a.Id == credentials.Id).FirstOrDefault();

                if (Entity == null)
                {
                    response.Message = "Kayıt bulunamadı";
                    return response;
                }


                Entity.Id = credentials.Id;
                Entity.UserId = credentials.UserId;
                Entity.DistrictId = credentials.DistrictId;
                Entity.Title = credentials.Title;
                Entity.Neighborhood = credentials.Neighborhood;
                Entity.AparmentNo = credentials.AparmentNo;
                Entity.BuildingNo = credentials.BuildingNo;
                Entity.Street = credentials.Street;


                _db.UserAddresses.Update(Entity);
                _db.SaveChanges();

                response.Success = true;
                response.Message = "İşlem Başarılı";

            }
            catch (Exception error)
            {
                response.Message = "İşlem başarısız, sunucuda sorun oluştu.";
                response.ExceptionMessage = error.Message;
            }

            return response;
        }
    }
}
