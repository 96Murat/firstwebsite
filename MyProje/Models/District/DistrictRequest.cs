using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.Models.District
{
    public class DistrictRequest
    {
        public class Create
        {
            public int CityId { get; set; }
            public string Name { get; set; }
        }

        public class Edit
        {
            public int Id { get; set; }
            public int CityId { get; set; }
            public string Name { get; set; }
        }

        public class List: RequestBase.List
        {
            public int? Id { get; set; }
            public int? CityId { get; set; }
            public string Name { get; set; }
        }

        public class Delete
        {
            public int Id { get; set; }
            public int CityId { get; set; }

        }

    }
}
