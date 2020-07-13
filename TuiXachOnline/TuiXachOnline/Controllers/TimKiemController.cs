using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuiXachOnline.Models;
namespace TuiXachOnline.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        public ActionResult Index(string SearchText)
        {
            QLTuiXachEntities db = new QLTuiXachEntities();
            if (!string.IsNullOrEmpty(SearchText))
            {
                var result = db.SANPHAMs.Where(s => s.TenSP.Contains(SearchText));
                return View(result.ToList());
            }
           
            else
             return RedirectToAction("Error", "TimKiem");
            return View(db);
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}