using LibraryManagementSystem.ENTITIES.Enums;
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
    [ModelMetadataType(typeof(UserDataTypes))]
    public class User : BaseClass
    {
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        public string LastName { get; set; }
        
        [Display(Name = "Fotoğraf")]
        public string PhotoName { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }

        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Display(Name = "Mail")]
        public string Mail { get; set; }

        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Display(Name = "Telefon")]
        public string PersonelPhoneNumber { get; set; }

        [Required(ErrorMessage = "Lütfen Cinsiyetinizi Seçiniz")]
        [Display(Name = "Cinsiyet")]
        public GenderEnum Gender { get; set; }
        
        [Required]
        [Display(Name = "Rol")]
        public RoleEnum RoleEnum { get; set; }

        //public List<RentBook> RentBooks { get; set; }
    }
}