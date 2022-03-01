using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.DAL
{ 
    public class DistrictEntity
    {
        public int Id { get; set; }
        public CityEntity City { get; set; }
        [Required]
        [MaxLength(100)]
        public int CityId { get; set; }
        public string DistrictName { get; set; }
        public List<UserEntity> Users { get; set; } = new List<UserEntity>();
        public List<AddressEntity> UserAddresses { get; set; } = new List<AddressEntity>();

    }
}
