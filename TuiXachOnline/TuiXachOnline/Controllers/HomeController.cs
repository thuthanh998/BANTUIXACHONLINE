using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuiXachOnline.Models;
namespace TuiXachOnline.Controllers
{
    public class HomeController : Controller
    {
        QLTuiXachEntities db = new QLTuiXachEntities();
        public ActionResult Index()
        {
            return View(db.SANPHAMs.Take(8).ToList());
           
        }
        public ActionResult ChitietSP(int? MaSP)
        {

            return View(db.SANPHAMs.SingleOrDefault(n => n.MaSP == MaSP));
        }

    }
}