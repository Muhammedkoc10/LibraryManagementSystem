using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.ENTITIES.Entities
{
    public class BaseClass
    {
        [Required]
        public int ID { get; set; }

        [Display(Name = "Aktif Mi")]
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
