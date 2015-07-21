using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;

namespace WebApplication1.Controllers
{
    //[Authorize]
    [RequireHttps]
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //GET: BlogPosts/Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            return View(db.Posts.OrderByDescending(p => p.Created));
        }
        
        // GET: BlogPosts
        public ActionResult Index()
        {
            return View(db.Posts.OrderByDescending(p => p.Created));
        }

        // GET: BlogPosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Created,Updated,Title,Slug,Body,MediaURL,Published")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {

                var slug = StringUtilities.UrlFriendly(blogPost.Title);

                if (String.IsNullOrWhiteSpace(slug))
                {
                    ModelState.AddModelError("Title", "Invalid title.");
                    return View(blogPost);
                }

                if (db.Posts.Any(p => p.Slug == slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique.");
                    return View(blogPost);
                }

                else
                {
                    blogPost.Created = DateTimeOffset.Now;
                    
                    blogPost.Slug = slug;

                    db.Posts.Add(blogPost);
                    db.SaveChanges();
                    return RedirectToAction("Index");


                }

            }

            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        [Authorize(Roles = "Moderator,Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Created,Updated,Title,Slug,Body,MediaURL,Published")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {

                blogPost.Updated = DateTimeOffset.Now;
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.Posts.Find(id);
            db.Posts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        
        // POST: Create Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "PostId,Body")]Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Created = DateTimeOffset.Now;
                comment.AuthorId = User.Identity.GetUserId();

                db.Comments.Add(comment);
                db.SaveChanges();

                
            }

            return RedirectToAction("Details", new { id = comment.PostId });
        }

        //POST: Edit Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator,Admin")]
        public ActionResult EditComment([Bind(Include = "Created,Id,PostId,Body")]Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Updated = DateTimeOffset.Now;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", new { id = comment.PostId});

            }

            return View(comment);
        }

        //POST: Delete Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator, Admin")]
        public ActionResult DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = comment.PostId });
        }

        
    }
}
