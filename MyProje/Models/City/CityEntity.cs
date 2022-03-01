using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.DAL
{
    public class CityEntity
    {
        public int Id { get; set; }
        [Required]
        public int CityCode { get; set; }
        [Required]
        [MaxLength(100)]
        public string CityName { get; set; }
        public List<DistrictEntity> Districts { get; set; } = new List<DistrictEntity>();
    }
}
