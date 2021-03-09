using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp01.Web.DTOs
{
    public class PersonForCreationDTO
    {
        [Required]
        [MaxLength(80)]
        public string Name { get; set; }
        [Required]
        public DateTime DateOfBith { get; set; }
        [Required]
        [MaxLength(12)]
        public string Nickname { get; set; }
        public int Weight { get; set; } = 0;
    }
}
