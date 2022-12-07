using LibraryManagementSystem.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.ENTITIES.ViewModel
{
    public class CreateRents
    {
        public List<RentBook> RentBook { get; set; }
        //public Book Books { get; set; }
        public User User { get; set; }
    }
}
