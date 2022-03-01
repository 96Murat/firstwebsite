using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.DAL
{
    public class AddressEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string Neighborhood { get; set; }
        [Required]
        [MaxLength(100)]
        public string Street { get; set; }
        [Required]
        [MaxLength(10)]
        [Column(TypeName = "varchar(10)")]
        public string BuildingNo { get; set; }
        [Required]
        [MaxLength(10)]
        [Column(TypeName ="varchar(10)")]
        public string AparmentNo { get; set; }
        public int DistrictId { get; set; }
        public int UserId { get; set; }

        public DistrictEntity District { get; set; }
        public UserEntity User { get; set; }
       
    }
}
