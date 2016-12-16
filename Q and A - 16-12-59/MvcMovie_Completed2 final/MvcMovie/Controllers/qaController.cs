using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{

    public class qaController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        // GET: Movies/index/war
        public ActionResult Index(string movieGenre, string searchString)
        {

            var GenreLst = new List<string>();
            var GenreQry = from d in db.Movies
                           orderby d.Genre
                           select d.Genre;

            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.movieGenre = new SelectList(GenreLst);

            //LINQ
            var movies = from m in db.Movies select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(
                    s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            return View(movies);
        }

        [HttpPost]
        public string Index(FormCollection fc, string searchString)
        {
            return "<h3> From [HttpPost]Index: " + searchString + "</h3>";
        }

        // GET: Movies/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            var GenreLst = new List<string>();
            var GenreQry = from d in db.Movies
                           orderby d.Genre
                           select d.Genre;

            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.movieGenre = new SelectList(GenreLst);

            return View();

        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Genre,Status,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }
        

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            var GenreLst = new List<string>();
            var GenreQry = from d in db.Movies
                           orderby d.Genre
                           select d.Genre;

            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.movieGenre = new SelectList(GenreLst);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Genre,Status,Rating")] Movie movie)
        {
            var GenreLst = new List<string>();
            var GenreQry = from d in db.Movies
                           orderby d.Genre
                           select d.Genre;

            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.movieGenre = new SelectList(GenreLst);

            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("admin");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult admin(string movieStatus, string movieGenre, string searchString)
        {
            if(Session["adminlogin"] == null)
            {
                return RedirectToAction("adminLogin");
            }

            ViewBag.test = Session["adminlogin"];

            //genre
            var GenreLst = new List<string>();
                var GenreQry = from d in db.Movies
                               orderby d.Genre
                               select d.Genre;

                GenreLst.AddRange(GenreQry.Distinct());
                ViewBag.movieGenre = new SelectList(GenreLst);

            //status
            var GenreLst2 = new List<string>();
            var GenreQry2 = from d in db.Movies
                           orderby d.Status
                           select d.Status;
            GenreLst2.AddRange(GenreQry2.Distinct());
            ViewBag.movieStatus = new SelectList(GenreLst2);

            //LINQ
            var movies = from m in db.Movies select m;

            if (!String.IsNullOrEmpty(movieStatus))
            {
                movies = movies.Where(st => st.Status.Contains(movieStatus));
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            return View(movies);

        }

        //login admin
        public ActionResult adminLogin(string userId, string password)
        {
            
            if (Session["adminlogin"] != null)//login state
            {
                return RedirectToAction("admin");
            }
            if (userId == "admin2" && password == "admin2")//login
            {                
                Session["adminlogin"] = "login";   
                return RedirectToAction("admin");
            }            
            Session["adminlogin"] = null;
            return View();
        }

        public ActionResult logoutsession()
        {            
            Session["adminlogin"] = null;
            return RedirectToAction("admin");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
