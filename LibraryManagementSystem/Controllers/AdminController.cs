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
using System.Net;
using System.Net.Mail;
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

        [HttpGet]
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
            var tryMail = _context.Users.Where(x => x.Mail == user.Mail).Select(y => y.Mail).FirstOrDefault();
            if (tryMail != null)
            {
                ViewBag.AllReadyAdded = "Mail adresi Sistemde Kayıtlı";
                return View();
            }
            if (ModelState.IsValid)
            {
                user.RoleEnum = ENTITIES.Enums.RoleEnum.User;
                if (user.Photo == null)
                    user.PhotoName = "\\img\\3586f868_94a2_4fd2_8933_17cd1ff7605e.jpeg";
                else
                    user.PhotoName = SaveThePicture(user.Photo);
                _user.Add(user);

                var fromAddress = new MailAddress("i_am_hr@outlook.com");
                var toAddress = new MailAddress(user.Mail);
                var Link = "Şifrenizi Oluşturmak İçin Linke Tıklayınız<a href= http://imhere.azurewebsites.net/Home/ResetPass/" + user.Mail + ">Buraya Tıklayınız</a>.";

                string resetPass = "Şifre Oluşturma Bağlantınız";
                using (var smtp = new SmtpClient
                {
                    Host = "smtp-mail.outlook.com",
                    /**/
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,

                    Credentials = new NetworkCredential(fromAddress.Address, "ik-123456")
                })
                    try
                    {
                        using (var message = new MailMessage(fromAddress, toAddress) { Subject = resetPass, Body = Link, IsBodyHtml = true })
                        {
                            smtp.Send(message);
                        }
                        ViewBag.SonucPozitif = "Mail Başarıyla Gönderildi.";
                    }
                    catch (Exception)
                    {
                        ViewBag.SonucNegatif = "Mail Gönderiminde Hata Oluştu.";
                    }
                return RedirectToAction("GetUser");
            }
            return View();
        }

        public IActionResult GetUser()
        {
            return View(_user.GetAll());
        }

        public IActionResult EditUser(int id)
        {
            User updateUser = _user.GetByID(id);
            return View(updateUser);
        }

        [HttpPost]
        public IActionResult EditUser(User user, int id)
        {
            User updatedUser = _user.GetByID(id);
            updatedUser.Address = user.Address;
            updatedUser.FirstName = user.FirstName;
            updatedUser.LastName = user.LastName;
            updatedUser.Mail = user.Mail;
            updatedUser.PersonelPhoneNumber = user.PersonelPhoneNumber;
            if (updatedUser.Photo != null)
                updatedUser.PhotoName = SaveThePicture(user.Photo);
            _user.Update(updatedUser);
            return RedirectToAction("GetUser");
        }

        public IActionResult RemoveUser(int id)
        {
            var deleteUser = _user.GetByID(id);
            _user.Remove(deleteUser);
            return RedirectToAction("GetUser");
        }
        public IActionResult GetAllRentBooks(int id)
        {
            CreateRentAll createRentAll = new CreateRentAll();
            createRentAll.Books = _book.GetAll();
            createRentAll.RentBooks = _rentbook.GetAll();
            createRentAll.Users = _user.GetAll();
            return View(createRentAll);
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
