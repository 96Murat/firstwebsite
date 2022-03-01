using MyProje.Models.District;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.Models.User
{
    public class UserResponse
    {
        public class List
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public bool IsAdmin { get; set; }
            public int DistrictId { get; set; }
            public int? AddressCount { get; set; }
            public District District { get; set; }
        }
        public class District
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DistrictResponse.CityList City { get; set; }
        }

    }

}
