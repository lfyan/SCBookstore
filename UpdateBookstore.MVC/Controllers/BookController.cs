using UpdateBookstore.MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UpdateBookstore.MVC.Controllers
{
    public class BookController : Controller
    {
        // GET: Novel
        public ActionResult Show(int pageNumber, string categoryname)
        {
            List<GetBooksofPage_Result> Books = null;
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                int bookPerPage = 10;
                Books = dbContext.GetBooksofPage(bookPerPage, pageNumber, categoryname).ToList();
                ViewBag.MaxPageNumber = dbContext.Books.Where(b=>b.Category==categoryname).Count() / bookPerPage + 1;
                ViewBag.Categoryname = categoryname;
            }

            return View("ShowBook", Books);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Book targetBook = null;
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                targetBook = dbContext.Books.SingleOrDefault(n => n.Id == id);
            }

            return View("EditBook", targetBook);
        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                if (this.Request.Files != null && this.Request.Files.Count > 0 && this.Request.Files[0].ContentLength > 0 && this.Request.Files[0].ContentLength < 1024 * 100)
                {
                    string fileName = Path.GetFileName(this.Request.Files[0].FileName);
                    string filePathOfWebsite = "~/Images/" + fileName;
                    book.CoverImagePath = filePathOfWebsite;
                    this.Request.Files[0].SaveAs(this.Server.MapPath(filePathOfWebsite));
                }               
                dbContext.Books.Attach(book);
                dbContext.Entry(book).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
            }
            return RedirectToAction("Show", new { pageNumber = 1, categoryname = book.Category });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            string category;
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                Book targetBook = null;               
                targetBook = dbContext.Books.SingleOrDefault(n => n.Id == id);
                if(targetBook==null)
                {
                    return View("Warning");         
                }
                else
                {
                    category = targetBook.Category;
                    dbContext.Books.Remove(targetBook);
                    dbContext.SaveChanges();
                }                
            }
            return RedirectToAction("Show", new { pageNumber = 1, categoryname = category});
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("AddBook");
        }

        [HttpPost]
        public ActionResult Add(Book book)
        {
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                if (this.Request.Files != null && this.Request.Files.Count > 0 && this.Request.Files[0].ContentLength > 0 && this.Request.Files[0].ContentLength < 1024 * 100)
                {
                    string fileName = Path.GetFileName(this.Request.Files[0].FileName);
                    string filePathOfWebsite = "~/Images/" + fileName;
                    book.CoverImagePath = filePathOfWebsite;
                    this.Request.Files[0].SaveAs(this.Server.MapPath(filePathOfWebsite));
                }

                dbContext.Books.Add(book);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Show", new { pageNumber = 1, categoryname = book.Category});
        }

        [HttpGet]
        public ActionResult AddToCart(int id)
        {
            Book targetBook = null;
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                targetBook = dbContext.Books.SingleOrDefault(n => n.Id == id);
            }

            return View("OrderReview", targetBook);
        }

        [HttpPost]
        public ActionResult AddToCart(Order order)
        {
            User targetUser = null;
            using (BookstoreEntities dbContext = new BookstoreEntities())
            {
                targetUser = dbContext.Users.SingleOrDefault(u => u.Id == order.UserId);
            }

            if (targetUser != null)
            {
                using (BookstoreEntities dbContext = new BookstoreEntities())
                {
                    dbContext.Orders.Add(order);
                    dbContext.SaveChanges();
                    targetUser = dbContext.Users.Include("Orders").SingleOrDefault(u => u.Id == order.UserId);
                }
                return View("Cart", targetUser);
            }
            else
            {
                return View("Error");
            }
        }
    }
}