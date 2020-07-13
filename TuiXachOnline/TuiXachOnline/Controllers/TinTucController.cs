using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuiXachOnline.Models;
namespace TuiXachOnline.Controllers
{
    public class TinTucController : Controller
    {
        // GET: TinTuc
        QLTuiXachEntities db = new QLTuiXachEntities();
        public ActionResult Index()
        {
            return View(db.TIN_TUC.ToList());

        }
    }
}