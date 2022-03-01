using MyProje.Models.District;
using MyProje.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.Models.Address
{
    public class AddressResponse
    {
        public class List
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Neighborhood { get; set; }
            public string Street { get; set; }
            public string BuildingNo { get; set; }
            public string AparmentNo { get; set; }
            public int DistrictId { get; set; }
            public int UserId { get; set; }
            public UserResponse.District District { get; set; }
        }
    }

}
