using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.DAL
{
    public class UserEntity
    {

        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        public bool IsAdmin { get; set; } = false;
        public int DistrictId { get; set; }
        public DistrictEntity District { get; set; }
        public List<AddressEntity> UserAddresses { get; set; } = new List<AddressEntity>();

    }
}
