using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.Models.Address
{
    public class AddressRequest
    {
        public class Create
        {
            public string Title { get; set; }
            public string Neighborhood { get; set; }
            public string Street { get; set; }
            public string BuildingNo { get; set; }
            public string AparmentNo { get; set; }
            public int DistrictId { get; set; }
            public int UserId { get; set; }
        }
        public class List : RequestBase.List
        {
            public int? Id { get; set; }
            public int? UserId { get; set; }
            public int? DistrictId { get; set; }
            public string Title { get; set; }
        }

        public class Edit
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Neighborhood { get; set; }
            public string Street { get; set; }
            public string BuildingNo { get; set; }
            public string AparmentNo { get; set; }
            public int DistrictId { get; set; }
            public int UserId { get; set; }
        }

        public class Delete
        {
            public int Id { get; set; }
        }
    }
}
