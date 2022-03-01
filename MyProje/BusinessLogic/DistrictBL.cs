using MyProje.DAL;
using MyProje.DAL.District;
using MyProje.Models;
using MyProje.Models.District;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.BusinessLogic
{
    public class DistrictBL
    {
        private readonly DistrictDA _district;

        public DistrictBL(DistrictDA district)
        {
            _district = district;
        }

        public Response List(DistrictRequest.List credentials)
        {
            return _district.List(credentials);
        }

        public Response Create(DistrictRequest.Create credentials)
        {
            return _district.Create(credentials);
        }

        public Response Edit(DistrictRequest.Edit credentials)
        {
            return _district.Edit(credentials);
        }

        public Response Delete(DistrictRequest.Delete credentials)
        {
            return _district.Delete(credentials);
        }

    }
}
