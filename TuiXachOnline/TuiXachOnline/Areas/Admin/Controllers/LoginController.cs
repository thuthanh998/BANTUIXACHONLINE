using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuiXachOnline.Models;
namespace TuiXachOnline.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdmin(ADMIN usermodel)
        {
            //New dbConnect
            using (QLTuiXachEntities db = new QLTuiXachEntities())
            {

                //Lấy username và password ở bản ghi đầu tiên
                var user = db.ADMINs.Where(x => x.TenDNAdmin == usermodel.TenDNAdmin && x.MatKhauAdmin == usermodel.MatKhauAdmin).FirstOrDefault();
                if (user == null)
                {

                    ViewBag.error = "Email or Password is fail";
                    return View("LoginAdmin", usermodel);
                }
                else
                {
                    //ViewBag.avatar = user.Avatar;
                    //ViewBag.Online = user.IsActive;
                    //Session["Online"] =user.IsActive;
                    //Session["Avatar"] = user.Avatar;
                    Session["EmailAdmin"] = user.EmailAdmin;
                    Session["TenDNAdmin"] = user.TenDNAdmin;
                    //return View(user)

                    return RedirectToAction("Index", "AminCRUD");
                }

            }
        }
        public ActionResult Test()
        {
            return View();
        }
    }
}