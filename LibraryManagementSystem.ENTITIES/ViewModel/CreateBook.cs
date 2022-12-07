using LibraryManagementSystem.ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.ENTITIES.ViewModel
{
    public class CreateBook
    {
        public Book Book{ get; set; }
        public List<Category> Categories{ get; set; }
    }
}
