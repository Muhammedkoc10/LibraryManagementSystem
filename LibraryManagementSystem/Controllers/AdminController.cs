using LibraryManagementSystem.BUSINESS.Abstract;
using LibraryManagementSystem.ENTITIES.Entities;
using LibraryManagementSystem.ENTITIES.ViewModel;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.REPOSITORIES.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace LibraryManagementSystem.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IGenericService<Book> _book;
        private readonly IGenericService<Category> _category;
        private readonly IGenericService<RentBook> _rentbook;
        private readonly IGenericService<User> _user;
        private readonly LMSDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(ILogger<AdminController> logger, IGenericService<Book> book, IGenericService<Category> category, IGenericService<RentBook> rentbook, IGenericService<User> user, LMSDbContext context, IWebHostEnvironment env)
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
        public IActionResult AddBook()
        {
            CreateBook createBook = new CreateBook();
            createBook.Categories = _category.GetAll();
            createBook.Book = new Book();
            return View(createBook);
        }

        [HttpPost]
        public IActionResult AddBook(CreateBook createBook)
        {
            var book = createBook.Book;
            if (book.Photo == null)
                book.PhotoName = "\\img\\3586f868_94a2_4fd2_8933_17cd1ff7605e.jpeg";
            else
                book.PhotoName = SaveThePicture(book.Photo);
            _book.Add(book);
            return RedirectToAction("GetBook");
        }

        public IActionResult GetBook()
        {
            return View(_book.GetAll());
        }

        public IActionResult EditBook(int id)
        {
            Book updateBook = _book.GetByID(id);
            return View(updateBook);
        }

        [HttpPost]
        public IActionResult EditBook(Book book,int id)
        {
            Book updatedBook = _book.GetByID(id);
            updatedBook.BookName = book.BookName;
            updatedBook.Author = book.Author;
            if (updatedBook.Photo != null)
                updatedBook.PhotoName = SaveThePicture(book.Photo);
            updatedBook.ISBN = book.ISBN;
            _book.Update(updatedBook);
            return RedirectToAction("GetBook");
        }

        public IActionResult RemoveBook(int id)
        {
            var deleteBook = _book.GetByID(id);
            _book.Remove(deleteBook);
            return RedirectToAction("GetBook");
        }


        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            user.RoleEnum = ENTITIES.Enums.RoleEnum.User;
            if (user.Photo == null)
                user.PhotoName = "\\img\\3586f868_94a2_4fd2_8933_17cd1ff7605e.jpeg";
            else
                user.PhotoName = SaveThePicture(user.Photo);

            _user.Add(user);
            return RedirectToAction("GetUser");
        }

        public IActionResult GetUser()
        {
            return View(_user.GetAll());
        }


        private string SaveThePicture(IFormFile img)
        {
            string filePath = Path.Combine(_env.WebRootPath, "img"); // ~/img

            string uniqueName = $"{Guid.NewGuid().ToString().Replace("-", "_").ToLower()}.{img.ContentType.Split('/')[1]}"; // Benzersiz isim oluşturma. İsimler Guid oluşturulacak. Küçük harf olacak ve - işaretleri yerine _ işareti olacak.

            string newFilePath = Path.Combine(filePath, uniqueName); //~/img/Dosyadı

            using (FileStream fs = new FileStream(newFilePath, FileMode.Create))
            {
                img.CopyTo(fs);
                return newFilePath.Substring(newFilePath.IndexOf("\\img\\")); // burada da dosya yolunun tammaı yerine \img\ kısmını substirng olarak alsın ve return etsin istiyorum. 
            }
        }
    }
}
