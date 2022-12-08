using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.ENTITIES.Entities
{
    public class UserDataTypes
    {
        [Required(ErrorMessage = "Lütfen Adınızı Giriniz")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "3 ila 50 Karakter arası")]
        [RegularExpression(@"^[a-zA-Z\sşİıÜüÖöĞğÇçŞ]*$", ErrorMessage = "Sadece Harf Girişi Yapınız")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Lütfen Soyadınızı Giriniz")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "3 ila 50 Karakter arası")]
        [RegularExpression(@"^[a-zA-Z\sşİıÜüÖöĞğÇçŞ]*$", ErrorMessage = "Sadece Harf Girişi Yapınız")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Lütfen Adresinizi Giriniz")]
        [StringLength(250, MinimumLength = 10, ErrorMessage = "10 ila 250 Karakter arası")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Mail adresi boş bırakılamaz")]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Mail Formatını Doğru Giriniz")]
        public string Mail { get; set; }


        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Lütfen Telefon Numaranızı Giriniz")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Telefon Numaranız doğru formatta değildir")]
        public string PersonelPhoneNumber { get; set; }

        [Required(ErrorMessage = "Geçici şifre boş bırakılamaz")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Şifreniz en az 8 karakterden oluşmalıdır")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
