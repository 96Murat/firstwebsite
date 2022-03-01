using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyProje.DAL;
using MyProje.DAL.City;
using MyProje.Models;
using MyProje.Models.City;

namespace MyProje.BusinessLogic
{

    public class CityBL
    {
        private readonly CityDA _city;
        public CityBL(CityDA city)
        {
            _city = city;
        }

        public Response List(CityRequest.List credentials)
        {
            return _city.List(credentials);
        }
        public Response Create(CityRequest.Create credentials)
        {
            return _city.Create(credentials);
        }
        public Response Edit(CityRequest.Edit credentials)
        {
            return _city.Edit(credentials);
        }
        public Response Delete(CityRequest.Delete credentials)
        {
            return _city.Delete(credentials);
        }


    }
}
