using LibraryManagementSystem.BUSINESS.Abstract;
using LibraryManagementSystem.ENTITIES.Entities;
using LibraryManagementSystem.ENTITIES.ViewModel;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.REPOSITORIES.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericService<Book> _book;
        private readonly IGenericService<Category> _category;
        private readonly IGenericService<RentBook> _rentbook;
        private readonly IGenericService<User> _user;
        private readonly LMSDbContext _context;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IGenericService<Book> book, IGenericService<Category> category, IGenericService<RentBook> rentbook, IGenericService<User> user, LMSDbContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _book = book;
            _category = category;
            _rentbook = rentbook;
            _user = user;
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            ViewBag.ZeroSci = ViewBag.ZeroAct = ViewBag.ZeroHist = ViewBag.ZeroSelf= ViewBag.ZeroHor = 0;
            Random rnd = new Random();
            ViewBag.Rnd = rnd.Next(1, 3);
            BooksCategory booksCategory = new BooksCategory();
            booksCategory.Category = _category.GetAll();
            booksCategory.Books = _book.GetAll();
            return View(booksCategory);
        }
        List<Book> books;
        //[HttpPost]
        //public IActionResult Index()
        //{
        //    //var books = booksCategory.Books;
        //    //int y = _context.Categories.Where(x => x.ID == id).FirstOrDefault().ID;
        //    //int catID = category.ID;
            
            
        //}

        public IActionResult UsersSearch(string keywords, /*Category category*/ int katID)
        {
            if(keywords!=null)
                books = _context.Books.Where(x => x.CategoryID == katID && x.BookName.Contains(keywords)).ToList();
            else
                books = _context.Books.Where(x => x.CategoryID == katID ).ToList();
            //return RedirectToAction("UsersSearch", books);
            return View(books);
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            User userGetpass = _context.Users.Where(x => x.Mail == email || x.Password == password).FirstOrDefault();
            if (userGetpass == null)
            {
                ViewBag.WrongPassOrMail = "Kullanıcı Adınız veya Şifreniz Yanlıştır.!";
                return View();
            }
            else if (userGetpass != null)
            {
                try
                {
                    var passhased = userGetpass.Password;
                    byte[] hashBytes = Convert.FromBase64String(passhased);
                    byte[] salt = new byte[16];
                    Array.Copy(hashBytes, 0, salt, 0, 16);
                    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
                    byte[] hash = pbkdf2.GetBytes(20);

                    for (int i = 0; i < 20; i++)
                    {
                        try
                        {
                            if (hashBytes[i + 16] != hash[i])
                                throw new UnauthorizedAccessException();
                            else
                            {
                                if (userGetpass != null)
                                {
                                    var claims = new List<Claim>()
                                       {
                                        new Claim("ID", userGetpass.ID.ToString()),
                                        new Claim(ClaimTypes.Name, userGetpass.FirstName),
                                        new Claim(ClaimTypes.Surname, userGetpass.LastName),
                                        new Claim(ClaimTypes.Email, userGetpass.Mail),
                                        new Claim(ClaimTypes.Role, userGetpass.RoleEnum.ToString()),
                                        new Claim("Photo", userGetpass.PhotoName.ToString())
                                       };
                                    var userIdentity = new ClaimsIdentity(claims, "login");
                                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                                    await HttpContext.SignInAsync(principal);

                                    if (userGetpass != null)
                                    {
                                        if (userGetpass.RoleEnum == ENTITIES.Enums.RoleEnum.Admin)
                                        {
                                            return RedirectToAction("Index", "Admin", new { id = userGetpass.ID });
                                        }
                                        else if(userGetpass.RoleEnum == ENTITIES.Enums.RoleEnum.User)
                                        {
                                            return RedirectToAction("Index", "User", new { id = userGetpass.ID });
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.WrongPassOrMail = "Kullanıcı Adınız veya Şifreniz Yanlıştır.!";
                                        return View();
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            ViewBag.WrongPassOrMail = "Kullanıcı Adınız veya Şifreniz Yanlıştır.!";
                            return View();
                        }
                    }
                }
                catch (Exception)
                {
                    ViewBag.Empty = "Lütfen Tüm Alanları Doldurunuz.";
                }
            }
            else
            {
                ViewBag.Empty = "Lütfen Tüm Alanları Doldurunuz.";
            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Reset()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Reset(string email) 
        {
            User userEntrance = _context.Users.Where(x => x.Mail == email).FirstOrDefault();

            string uniqueName = $"{Guid.NewGuid().ToString().ToLower()}";

            if (userEntrance != null)
            {
                var fromAddress = new MailAddress("i_am_hr@outlook.com");
                var toAddress = new MailAddress(email);
                var Link = "Şifrenizi Yenilemek İçin Linki Tıklayınız<a href= https://localhost:44320/Home/ResetPass/" + email + ">Buraya Tıklayınız</a>.";
                string resetPass = "Şifre Yenileme Bağlantınız";
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
                        {
                            using (var message = new MailMessage(fromAddress, toAddress) { Subject = resetPass, Body = Link, IsBodyHtml = true })
                            {
                                smtp.Send(message);
                            }
                        }
                        ViewBag.Sonuc = "Mail Başarıyla Gönderildi.";
                    }
                    catch (Exception)
                    {
                        ViewBag.Sonuc = "Mail Gönderiminde Hata Oluştu.";
                    }
            }
            return View();
        }

        public IActionResult ResetPass()
        {
            return View();
        }


        [Route("Home/ResetPass/{email}/")]
        [HttpPost]
        public IActionResult ResetPass(string email, string password, string password2)
        {
            ViewBag.userResetEmail = Request.Query["email"];
            User userEntrance = _context.Users.Where(x => x.Mail == email).FirstOrDefault();
            if (password.Length >= 8)
            {
                if (userEntrance.Password != password && password == password2)
                {
                    byte[] salt;
                    new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
                    byte[] hash = pbkdf2.GetBytes(20);
                    byte[] hashBytes = new byte[36];
                    Array.Copy(salt, 0, hashBytes, 0, 16);
                    Array.Copy(hash, 0, hashBytes, 16, 20);
                    string savedPasswordHash = Convert.ToBase64String(hashBytes);
                    userEntrance.Password = savedPasswordHash;
                    _user.Update(userEntrance);
                    return RedirectToAction("Login");
                }
                else if (userEntrance == null)
                {
                    ViewBag.Empty = "Email adresiniz sistemde bulunamadı.";
                    return View();
                }
                else
                {
                    ViewBag.Hata1 = " Şifreler uyuşmuyor eski şifre kabul edilemez.";
                    return View();
                }
            }
            ViewBag.Hata = "Şifreniz en az 8 karakterden oluşmalıdır";
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
