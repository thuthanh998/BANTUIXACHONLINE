using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TuiXachOnline.Models;
namespace TuiXachOnline.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult DangNhap()
        {
            return View();
        }
        [Authorize] 
        public ActionResult DangXuat()
        {
            Session["TenDN"] = null;
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult DangNhap(KHACH_HANG usermodel)
        {
            
            using (QLTuiXachEntities db = new QLTuiXachEntities())
            {

                //Lấy username và password ở bản ghi đầu tiên
                var user = db.KHACH_HANG.Where(x => x.TenDN == usermodel.TenDN && x.MatKhau == usermodel.MatKhau).FirstOrDefault();
                if (user == null)
                {

                    ViewBag.error = "Email or Password is fail";
                    return View("DangNhap", usermodel);
                }
                else
                {
                    //ViewBag.avatar = user.Avatar;
                    //ViewBag.Online = user.IsActive;
                    //Session["Online"] =user.IsActive;
                    //Session["Avatar"] = user.Avatar;
                    //Session["Email"] = user.Email;
                    Session["TenDN"] = user.TenDN;
                    //return View(user)

                    return RedirectToAction("ThanhToan", "GioHang");
                }

            }
        }
    }
}