using LibraryManagementSystem.ENTITIES.Entities;
using LibraryManagementSystem.REPOSITORIES.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.REPOSITORIES
{
    public static class SeedData
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var serviseScope = app.ApplicationServices.CreateScope())
            {
                LMSDbContext context = serviseScope.ServiceProvider.GetService<LMSDbContext>();

                context.Database.Migrate();

                if (!context.Categories.Any()&&!context.Books.Any()&&!context.Users.Any())
                {
                    User User = new User()
                    {
                        FirstName = "Muhammed",
                        LastName = "Koç",
                        PhotoName = "mami.jpg",
                        Address = "Çanakkale",
                        Mail = "muhammedkoc10@gmail.com",
                        Password = "mami1234",
                        PersonelPhoneNumber = "5432917293",
                        Gender = ENTITIES.Enums.GenderEnum.Erkek,
                        RoleEnum = ENTITIES.Enums.RoleEnum.Admin,
                    };
                    User User2 = new User()
                    {
                        FirstName = "Kenan",
                        LastName = "Koç",
                        PhotoName = "mami.jpg",
                        Address = "Çanakkale",
                        Mail = "kenankoc17@gmail.com",
                        Password = "kenan123",
                        PersonelPhoneNumber = "5444212323",
                        Gender = ENTITIES.Enums.GenderEnum.Erkek,
                        RoleEnum = ENTITIES.Enums.RoleEnum.User
                    };
                    context.Users.AddRange(User, User2);

                    Category categoryScienceFiction = new Category()
                    {
                        CategoryName = "Bilim Kurgu",
                        IsActive = true
                    };
                    Category categoryActionAdventure = new Category()
                    {
                        CategoryName = "Aksiyon-Macera",
                        IsActive = true
                    };
                    Category categoryHistory = new Category()
                    {
                        CategoryName = "Tarih",
                        IsActive = true
                    };
                    Category categorySelfImprovement = new Category()
                    {
                        CategoryName = "Kişisel Gelişim",
                        IsActive = true
                    };
                    Category categoryHorrorThriller = new Category()
                    {
                        CategoryName = "Korku-Gerilim",
                        IsActive = true
                    };

                    context.Categories.AddRange(categoryScienceFiction, categoryActionAdventure, categoryHistory, categorySelfImprovement, categoryHorrorThriller);
                    context.SaveChanges();

                    Book bookSciFi = new Book()
                    {
                        BookName="Otostopçunun Galaksi Rehberi",
                        ISBN = "ISBN975-17-1627-6",
                        Author = "Douglas Adams",
                        PhotoName = "Otostop.jpg",
                        CategoryID = categoryScienceFiction.ID,
                        IsActive = true
                    };
                    Book bookActAd = new Book()
                    {
                        BookName="Da Vinci Şifresi",
                        ISBN = "ISBN976-17-1622-1",
                        Author = "Dan Brown",
                        PhotoName = "davinci.jpg",
                        CategoryID = categoryActionAdventure.ID,
                        IsActive = true
                    };
                    Book bookHist = new Book()
                    {
                        BookName = "Nutuk",
                        ISBN = "ISBN976-17-1881-1",
                        Author = "Mustafa Kemal Atatürk",
                        PhotoName = "nutuk.jpg",
                        CategoryID = categoryHistory.ID,
                        IsActive = true
                    };
                    Book bookSelfImp = new Book()
                    {
                        BookName = "Ikigai",
                        ISBN = "ISBN976-11-6512-6",
                        Author = "Francesc Miralles i Contijoch ve Hector Garcia",
                        PhotoName = "Ikigai.jpg",
                        CategoryID = categorySelfImprovement.ID,
                        IsActive = true
                    };
                    Book bookHorTri = new Book()
                    {
                        BookName = "Drakula",
                        ISBN = "ISBN126-44-0234-9",
                        Author = "Bram Stoker",
                        PhotoName = "drakula.jpg",
                        CategoryID = categoryHorrorThriller.ID,
                        IsActive = true
                    };

                    context.Books.AddRange(bookSciFi, bookActAd, bookHist, bookSelfImp, bookHorTri);
                    context.SaveChanges();

                    RentBook rentBook = new RentBook()
                    {
                        RentStartTime = DateTime.Now.Date,
                        RentEndTime = DateTime.Now.AddDays(14),
                        UserID = User2.ID,
                        BookID = bookActAd.ID
                    };

                    context.RentBooks.Add(rentBook);
                    context.SaveChanges();
                }
            }
        }
    }
}
