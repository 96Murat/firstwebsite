using Microsoft.EntityFrameworkCore;
using MyProje.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MyProje.Models;
using MyProje.Models.User;
using MyProje.Models.District;

namespace MyProje.DAL.User
{
    public class UserDA
    {
        private readonly Context _db;
        public UserDA(Context db)
        {
            _db = db;
        }
        public Response List(UserRequest.List credentials)
        {
            var response = new Response();

            try
            {
                var UserEntities = _db.Users.AsQueryable();
                var MaxCount = UserEntities.Count();

                if (credentials.Name != null && credentials.Password != null)
                {
                    UserEntities = UserEntities.Where(u => u.Name == credentials.Name || u.Password == credentials.Password);
                }
                else if (credentials.Name != null && credentials.Email != null)
                {
                    UserEntities = UserEntities.Where(u => u.Name == credentials.Name || u.Email == credentials.Email);
                }
                else if (credentials.Id != null && credentials.Password != null)
                {
                    UserEntities = UserEntities.Where(u => u.Id == credentials.Id && u.Password == credentials.Password);
                }
                else if (credentials.Name != null)
                {
                    UserEntities = UserEntities.Where(u => u.Name == credentials.Name);
                }
                else if (credentials.Id != null)
                {
                    UserEntities = UserEntities.Where(u => u.Id == credentials.Id);
                }
                if (credentials.DistrictId != null)
                {
                    UserEntities = UserEntities.Where(u => u.DistrictId == credentials.DistrictId);
                }
                if (credentials.IsOrderByAsc)
                    UserEntities = UserEntities.OrderBy(u => u.Name).OrderByDescending(u=>u.IsAdmin);
                else
                    UserEntities = UserEntities.OrderByDescending(u => u.Name).OrderBy(u => u.IsAdmin);

                UserEntities = UserEntities.Skip(credentials.RowCount * (credentials.Page - 1)).Take(credentials.RowCount);

                var userList = UserEntities.Select(u => new UserResponse.List
                {
                    Id = u.Id,
                    Name = u.Name,
                    Password = u.Password,
                    Email = u.Email,
                    IsAdmin = u.IsAdmin,
                    DistrictId = u.DistrictId,
                    AddressCount= credentials.GetChildrenCount ? u.UserAddresses.Count() : (int?)null
                }).ToList();


                if (credentials.IsInclude)
                {
                    userList = UserEntities.Select(u => new UserResponse.List
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Password = u.Password,
                        Email = u.Email,
                        IsAdmin = u.IsAdmin,
                        DistrictId = u.DistrictId,
                        AddressCount = credentials.GetChildrenCount ? u.UserAddresses.Count() : (int?)null,
                        District = new UserResponse.District
                        {
                            Name = u.District.DistrictName,
                            City = new DistrictResponse.CityList
                            {
                                Id = u.District.CityId,
                                Name = u.District.City.CityName,
                                Code = u.District.City.CityCode
                            }
                        }
                    }).ToList();
                }

                var count = userList.Count();
                if (count > 0)
                {
                    response.Success = true;
                    response.Count = count;
                    response.MaxCount = MaxCount;
                    response.Message = count + " kayıt bulundu.";
                    response.Result = userList;
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

        public Response Create(UserRequest.Create credentials)
        {
            var response = new Response();
            try
            {
                var Entity = _db.Users.Where(a => a.Name == credentials.Name || a.Email == credentials.Email).FirstOrDefault();

                if (Entity != null)
                {
                    response.Message = "Kayıt mevcut";
                    return response;
                }
                var newEntity = new UserEntity
                {
                    Name = credentials.Name,
                    Email = credentials.Email,
                    DistrictId = credentials.DistrictId,
                    Password = credentials.Password,
                    UserAddresses = new List<AddressEntity>
                    {
                        new AddressEntity
                        {
                            Title = credentials.Title,
                            Neighborhood = credentials.Neighborhood,
                            Street = credentials.Street,
                            AparmentNo = credentials.AparmentNo,
                            BuildingNo = credentials.BuildingNo,
                            DistrictId = credentials.DistrictId
                        }
                    }
                };
                _db.Users.Add(newEntity);
                _db.SaveChanges();

                var dataEntity = _db.Users.Where(a => a.Name == credentials.Name || a.Email == credentials.Email).FirstOrDefault();

                response.Result = new UserResponse.List{ Id = dataEntity.Id, Name = dataEntity.Name };
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
        public Response Delete(UserRequest.Delete credentials)
        {
            var response = new Response();
            try
            {
                var Entity = _db.Users.Where(a => a.Id == credentials.Id).FirstOrDefault();

                if (Entity == null)
                {
                    response.Message = "Kayıt bulunamadı";
                    return response;
                }

                _db.Users.Remove(Entity);
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
        public Response Edit(UserRequest.Edit credentials)
        {
            var response = new Response();
            try
            {
                var Entity = _db.Users.Where(a => a.Id == credentials.Id).FirstOrDefault();

                if (Entity == null)
                {
                    response.Message = "Kayıt bulunamadı";
                    return response;
                }

                if (credentials.ChangeAdmin)
                {
                    Entity.IsAdmin = !Entity.IsAdmin;
                }
                else
                {
                    Entity.Password = credentials.Password;
                }

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
