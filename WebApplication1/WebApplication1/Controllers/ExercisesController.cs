using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize]
    [RequireHttps]
    public class ExercisesController : Controller
    {
        // GET: Exercises
        public ActionResult Max()
        {
            return PartialView();
        }

        public ActionResult Factorial()
        {
            return PartialView();
        }

        public ActionResult Perfect()
        {
            return PartialView();
        }

        public ActionResult Happy()
        {
            return PartialView();
        }

        public ActionResult Armstrong()
        {
            return PartialView();
        }

        public ActionResult Palindrome()
        {
            return PartialView();
        }

        public ActionResult FizzBuzz()
        {
            return PartialView();
        }

        public ActionResult Longest()
        {
            return PartialView();
        }

        public ActionResult WordSearch()
        {
            return PartialView();
        }

        public ActionResult WordFilter()
        {
            return PartialView();
        }

        public ActionResult WordFreq()
        {
            return PartialView();
        }

        
    }
}