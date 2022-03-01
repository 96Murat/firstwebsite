using MyProje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.Models.City
{
    public class CityResponse
    {
        public class List
        {
            public int Id { get; set; }
            public int CityCode { get; set; }
            public string CityName { get; set; }
            public int? DistrictCount { get; set; }
        }

    }

}
