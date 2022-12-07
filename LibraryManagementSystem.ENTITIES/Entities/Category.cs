using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.ENTITIES.Entities
{
    public class Category : BaseClass
    {
        [Required]
        [Display(Name = "Kategori Adı")]
        public string CategoryName { get; set; }

        public List<Book> Books { get; set; }
    }
}
