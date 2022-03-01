using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.Models.City
{
    public class CityRequest
    {
        public class Create
        {
            public int Code { get; set; }
            public string Name { get; set; }
        }

        public class Edit
        {
            public int Id { get; set; }
            public int Code { get; set; }
            public string Name { get; set; }
        }

        public class List: RequestBase.List
        {
            public int? Id { get; set; }
            public int? Code { get; set; }
            public string Name { get; set; }
        }

        public class Delete
        {
            public int Id { get; set; }
        }
    }

}