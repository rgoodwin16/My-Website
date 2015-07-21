using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CommentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Comment

        public ActionResult Index(int id)
        {
            var comment = db.Comments.Find(id);
            return View(comment);
        }
        
        public ActionResult Edit(int id)
        {
            var comment = db.Comments.Find(id);
            return View(comment);
        }

        public ActionResult Delete(int id)
        {
            var comment = db.Comments.Find(id);
            return View(comment);
        }


    }
}