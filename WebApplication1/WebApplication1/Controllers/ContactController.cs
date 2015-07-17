using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            ViewBag.MessageSent = TempData["MessageSent"];
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index (ContactMessage contactForm)
        {
            if (!ModelState.IsValid) return View(contactForm);
            var emailer = new EmailService();

            var mail = new IdentityMessage
            {
                Subject = contactForm.Subject,
                Destination = ConfigurationManager.AppSettings["ContactMeEmail"],
                Body = "You have recieved a new contact form submission from " + contactForm.contactName +
                "( " + contactForm.FromEmail + " ) with the following contents<br/>" + contactForm.Message
            };

            emailer.SendAsync(mail);

            TempData["MessageSent"] = "Your message has been delivered successfully!";

            return RedirectToAction("Index");
        }
    }
}