using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.ENTITIES.Entities
{
    public class RentBook : BaseClass
    {

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "Kiralama Başlama Tarihi")]
        public DateTime RentStartTime { get; set; }


        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "Kiralama Bitiş Tarihi")]
        public DateTime RentEndTime { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }


        [ForeignKey("Book")]
        public int BookID { get; set; }
        public Book Book { get; set; }
    }
}

