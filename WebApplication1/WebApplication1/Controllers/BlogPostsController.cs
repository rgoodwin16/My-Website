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
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebApplication1.Controllers
{
    //[Authorize]
    [RequireHttps]
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        // ==============================================
            //INDEX - ADMIN INDEX PAGE
        // ============================================== 

        //GET: BlogPosts/Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            
            return View(db.Posts.OrderByDescending(p => p.Created));
        }

        // ==============================================
            //INDEX - DEFAULT BLOG INDEX PAGE
        // ============================================== 

        // GET: BlogPosts
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult Index(int? page, string search, string category)
        {
            var blogList = from str in db.Posts
                           select str;
           
            if (search == null)
            {
                int pageSize = 3;
                int pageNumber = (page ?? 1);
                if (category != null)
                {
                    ViewBag.Category = category;
                    ViewBag.search = null;
                    var cat = db.Posts.Where(c => c.Category.Equals(category));
                    return View(cat.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
                }   

                return View(db.Posts.OrderByDescending(p=> p.Created).ToPagedList(pageNumber,pageSize));

            }
            else
            {
                ViewBag.search = search;
                blogList = db.Posts.Where(s => s.Title.Contains(search) || s.Body.Contains(search) || s.Category.Contains(search) || s.Comments.Any(p => p.Body.Contains(search)));

                return View(blogList.OrderByDescending(p => p.Created).ToPagedList(page ?? 1, 3));
            }

            
        }

        
        // ==============================================
            //POSTS - CREATE/EDIT/DELETE
        // ============================================== 
        
        // GET: BlogPosts/Details/5
        // GET: Blog/{Slug}
        public ActionResult Details(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.FirstOrDefault(p=> p.Slug == slug);
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
        public ActionResult Create([Bind(Include = "Id,Created,Updated,Title,Slug,Body,MediaURL,Published,Category")] BlogPost blogPost, HttpPostedFileBase image)
        {
            //Check if the image selected by the user isn't empty
            if(image!=null && image.ContentLength > 0) 
            {
                //check the file name to make sure its an image
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                {
                    ModelState.AddModelError("image", "Invalid Format");
                }
                
            }

            if (ModelState.IsValid)
            {

                if (image != null)
                {
                    //relative path
                    var filePath = "/Uploads/";
                    //path on physical drive on server
                    var absPath = Server.MapPath("~" + filePath);
                    Directory.CreateDirectory(absPath);
                    //media url for relative path
                    blogPost.MediaURL = filePath + image.FileName;
                    //save image
                    image.SaveAs(Path.Combine(absPath,image.FileName));
                }
                
                var slug = StringUtilities.UrlFriendly(blogPost.Title);
                
                //Make sure the Title Slug Box has some data in it
                if (String.IsNullOrWhiteSpace(slug))
                {
                    ModelState.AddModelError("Title", "Invalid title.");
                    return View(blogPost);
                }
                                
                //Check the Database to see if the Title already exists
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

        // ==============================================
           //COMMENTS - CREATE/EDIT/DELETE
        // ============================================== 

        
        // POST: BlogPosts/Comment/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "PostId,Body")]Comment comment)
        {
            var blogPost = db.Posts.Find(comment.PostId);
            if (ModelState.IsValid)
            {
                comment.Created = DateTimeOffset.Now;
                comment.AuthorId = User.Identity.GetUserId();

                db.Comments.Add(comment);
                db.SaveChanges();

                
            }

            return RedirectToAction("Details", new { slug  = blogPost.Slug });
        }

        //POST: BlogPosts/Comment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator,Admin")]
        public ActionResult EditComment([Bind(Include = "AuthorId,Created,Id,PostId,Body")]Comment comment)
        {
            if (ModelState.IsValid)
            {
                var blogPost = db.Posts.Find(comment.PostId);
                comment.Updated = DateTimeOffset.Now;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", new { slug = blogPost.Slug});

            }

            return View(comment);
        }

        //POST: BlogPosts/Comment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator, Admin")]
        public ActionResult DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            var blogPost = db.Posts.Find(comment.PostId);
            
            db.Comments.Remove(comment);
            db.SaveChanges();

            return RedirectToAction("Details", new { slug = blogPost.Slug });
        }

        
    }
}
