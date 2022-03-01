using MyProje.Models.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.Models.District
{
    public class DistrictResponse
    {
        public class List
        {
            public int Id { get; set; }
            public string DistrictName { get; set; }
            public int CityId { get; set; }
            public CityList City { get; set; }
            public int? UserCount { get; set; }
            public int? AddressCount { get; set; }
        }

        public class CityList
        {
            public int Id { get; set; }
            public int Code { get; set; }
            public string Name { get; set; }
        }
    }
}
