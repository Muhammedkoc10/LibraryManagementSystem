using LibraryManagementSystem.BUSINESS.Abstract;
using LibraryManagementSystem.ENTITIES.Entities;
using LibraryManagementSystem.ENTITIES.ViewModel;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.REPOSITORIES.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.UI.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IGenericService<Book> _book;
        private readonly IGenericService<Category> _category;
        private readonly IGenericService<RentBook> _rentbook;
        private readonly IGenericService<User> _user;
        private readonly LMSDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UserController(ILogger<UserController> logger, IGenericService<Book> book, IGenericService<Category> category, IGenericService<RentBook> rentbook, IGenericService<User> user, LMSDbContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _book = book;
            _category = category;
            _rentbook = rentbook;
            _user = user;
            _context = context;
            _env = env;
        }

        public IActionResult Index(int id)
        {
            User userEntrance = _context.Users.Where(x => x.ID == id).FirstOrDefault();
            return View(userEntrance);
        }

        public IActionResult GetBook()
        {
            return View(_book.GetAll());
        }

        public IActionResult RentBook(int id)
        {
            CreateRent createRent = new CreateRent();
            //createRent.User = _user.GetByID(id);
            createRent.Books = _book.GetAll();
            createRent.RentBook = new RentBook();
            return View(createRent);
        }

        [HttpPost]
        public IActionResult RentBook(CreateRent createRent, int id)
        {
            createRent.User = _user.GetByID(id);
            createRent.RentBook.UserID= id;
            int y = createRent.RentBook.BookID;
            createRent.Books =_context.Books.Where(x => x.ID == y).ToList();
            createRent.RentBook.RentStartTime = DateTime.Now;
            createRent.RentBook.RentEndTime = DateTime.Now.AddDays(14);
            createRent.RentBook.IsActive = true;
            _rentbook.Add(createRent.RentBook);
            return RedirectToAction("GetMyRentBook",new { id = id });
        }

        public IActionResult GetMyRentBook(int id)
        {
            CreateRents createRents = new CreateRents();
            createRents.RentBook = _context.RentBooks.Where(x => x.UserID == id).ToList();
            createRents.Books = _context.RentBooks.Where(x => x.UserID == id).Select(x=>x.Book).ToList();
            return View(createRents);
        }
    }
}
