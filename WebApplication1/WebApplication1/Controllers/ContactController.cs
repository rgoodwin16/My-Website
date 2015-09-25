using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    //[Authorize]
    [RequireHttps]
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
            if (!ModelState.IsValid)
            {
                if (contactForm.FromEmail == null || !contactForm.IsValidEmail(contactForm.FromEmail))
                {
                    TempData["Error"] = "Please enter a valid email address.";
                }

                ViewBag.EmailError = TempData["Error"];
                return View(contactForm);
            }
                
                
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


        public class RegexUtilities
        {
            bool invalid = false;

            public bool IsValidEmail(string strIn)
            {
                invalid = false;
                if (String.IsNullOrEmpty(strIn))
                    return false;

                // Use IdnMapping class to convert Unicode domain names.
                try
                {
                    strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                          RegexOptions.None, TimeSpan.FromMilliseconds(200));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }

                if (invalid)
                    return false;

                // Return true if strIn is in valid e-mail format.
                try
                {
                    return Regex.IsMatch(strIn,
                          @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                          @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                          RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }
            }

            private string DomainMapper(Match match)
            {
                // IdnMapping class with default property values.
                IdnMapping idn = new IdnMapping();

                string domainName = match.Groups[2].Value;
                try
                {
                    domainName = idn.GetAscii(domainName);
                }
                catch (ArgumentException)
                {
                    invalid = true;
                }
                return match.Groups[1].Value + domainName;
            }
        }

    }

}