using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.Models.User
{
    public class UserRequest
    {
        public class Login
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }

        public class Create
        {
            public string Name { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public int DistrictId { get; set; }
            public string Title { get; set; }
            public string Neighborhood { get; set; }
            public string Street { get; set; }
            public string BuildingNo { get; set; }
            public string AparmentNo { get; set; }
        }

        public class Edit
        {
            public int Id { get; set; }
            public string Password { get; set; }
            public string NewPassword { get; set; }
            public string Email { get; set; }
            public string NewPasswordAgain { get; set; }
            public int DistrictId { get; set; }
            public bool ChangeAdmin { get; set; }
        }

        public class List: RequestBase.List
        {
            public int? Id { get; set; }
            public int? DistrictId { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public bool IsInclude { get; set; }
        }

        public class Delete
        {
            public int? Id { get; set; }
        }
    }
}