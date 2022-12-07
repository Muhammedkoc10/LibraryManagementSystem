using LibraryManagementSystem.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.ENTITIES.ViewModel
{
    public class CreateRentAll
    {
        public List<RentBook> RentBooks{ get; set; }
        public List<Book> Books { get; set; }
        public List<User> Users { get; set; }
    }
}
