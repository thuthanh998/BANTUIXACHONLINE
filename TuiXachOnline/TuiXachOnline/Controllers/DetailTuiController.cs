using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuiXachOnline.Models;
namespace TuiXachOnline.Controllers
{
    public class DetailTuiController : Controller
    {
        // GET: DetailTui
        QLTuiXachEntities db = new QLTuiXachEntities();
        public ActionResult ChitietSP(int MaSP)
        {
            //SANPHAM b = (from p in db.SANPHAMs where p.MaSP == SanphamID select p).ToArray()[0];

            //return View(b);
            return View(db.SANPHAMs.SingleOrDefault(n => n.MaSP == MaSP));
        }
        public ActionResult ChudeSP(int? MaLoai)
        {
            ViewBag.name = db.PHAN_LOAI.SingleOrDefault(n => n.MaLoai == MaLoai).TenLoai;
            return View(db.SANPHAMs.Where(n => n.MaLoai == MaLoai).ToList());


        }

    }
}