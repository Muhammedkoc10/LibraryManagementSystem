using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.ENTITIES.Entities
{
    public class Book : BaseClass
    {
        [Required]
        [Display(Name = "Kitap Adı")]
        public string BookName { get; set; }

        [Required]
        [Display(Name = "ISBN Numarası")]
        public string ISBN { get; set; }

        [Required]
        [Display(Name = "Yazar")]
        public string Author { get; set; }

        [Display(Name = "Fotoğraf")]
        public string PhotoName { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }


        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        //public List<RentBook> RentBooks { get; set; }
    }
}
