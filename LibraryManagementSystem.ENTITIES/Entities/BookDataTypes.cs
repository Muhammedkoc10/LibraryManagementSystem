using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.ENTITIES.Entities
{
    public class BookDataTypes
    {
        [Required(ErrorMessage = "Lütfen Adınızı Giriniz")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "3 ila 50 Karakter arası")]
        public string BookName { get; set; }

        [Required(ErrorMessage = "Lütfen Yazar Adını Giriniz")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "3 ila 50 Karakter arası")]
        [RegularExpression(@"^[a-zA-Z\sşİıÜüÖöĞğÇçŞ]*$", ErrorMessage = "Sadece Harf Girişi Yapınız")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Lütfen ISBN Numarasını Giriniz")]
        public string ISBN { get; set; }
    }
}
