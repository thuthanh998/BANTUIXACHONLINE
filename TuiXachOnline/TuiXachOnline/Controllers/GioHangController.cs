using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.Configuration;
using System.IO;

using TuiXachOnline.Models;
namespace TuiXachOnline.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        QLTuiXachEntities db = new QLTuiXachEntities();
        public List<SPGH> LayGioHang()
        {
            List<SPGH> lstSP = Session["GioHang"] as List<SPGH>;
            if (lstSP == null)
            {
                lstSP = new List<SPGH>();
                Session["GioHang"] = lstSP;
            }
            return lstSP;
        }
        public ActionResult GioHang()
        {
            List<SPGH> listSP = LayGioHang();
            int TongSL = 0;
            double TongTien = 0;
            foreach (var item in listSP)
            {
                TongSL += item.SoLuong;
                TongTien += item.TongTien;
            }
            Session["TongSL"] = TongSL.ToString();
            Session["TongTien"] = TongTien.ToString();
            if (User.Identity.Name == null || User.Identity.Name.ToString() == "")
            {
                return RedirectToAction("Index", "Account");
            }
            int y = int.Parse(Session["Makh"].ToString());
            List<KHACH_HANG> kh = db.KHACH_HANG.Where(n => n.MaKH == y).ToList();
            ViewData["lstkh"] = kh;
            return View(listSP);
        }
        [HttpPost]
        public ActionResult ThemGioHang(int iMaSP, int? SL)
        {
            List<SPGH> lstSP = LayGioHang();
            SPGH SP = lstSP.Find(n => n.MaSPham == iMaSP);
            if (SP == null)
            {
                SP = new SPGH();
                SANPHAM tui = db.SANPHAMs.Single(n => n.MaSP == iMaSP);
                SP.MaSPham = iMaSP;
                SP.TenSPham = tui.TenSP;
                SP.AnhSP = tui.HinhMinhHoa;
                SP.GiaSP = double.Parse(tui.DonGia.ToString());
                if (SL == null)
                {
                    SP.SoLuong = 1;
                }
                else
                {
                    SP.SoLuong = int.Parse(SL.ToString());
                }
                lstSP.Add(SP);

                Session["GioHang"] = lstSP;
                return Json(lstSP, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (SL == null)
                {
                    SP.SoLuong++;
                }
                else
                {
                    SP.SoLuong = int.Parse(SL.ToString());
                }
                Session["GioHang"] = lstSP;
                return Json(lstSP, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult XoaSP(int iMaSP)
        {
            List<SPGH> lstSP = LayGioHang();
            SPGH SP = lstSP.Find(n => n.MaSPham == iMaSP);
            lstSP.Remove(SP);
            Session["GioHang"] = lstSP;
            return Json(lstSP, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public ActionResult ThanhToan()
        //{
        //   if (User.Identity.Name == null || User.Identity.Name.ToString() == "")
        //    {
        //        return RedirectToAction("Index", "Account");
        //    }
        //   if(Session["GioHang"] == null)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    List<SPGH> listSP = LayGioHang();
        //    int TongSL = 0;
        //    double TongTien = 0;
        //    foreach (var item in listSP)
        //    {
        //        TongSL += item.SoLuong;
        //        TongTien += item.TongTien;
        //    }
        //    Session["TongSL"] = TongSL.ToString();
        //    Session["TongTien"] = TongTien.ToString();
        //    return View(listSP);

        //}
        [HttpPost]
        public ActionResult GioHang(FormCollection frm, DON_DAT_HANG donhang)
        {
            if (Session["Tenkh"] == null)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {

                int x = int.Parse(Session["Makh"].ToString());
                var user = db.KHACH_HANG.FirstOrDefault(n => n.MaKH == x);
                if (user.DiaChi == null || user.SDT == null)
                {
                    user = db.KHACH_HANG.Find(x);
                    {
                        user.SDT = frm["dienthoainhanhang"];
                        user.DiaChi = frm["diachinhanhang"];

                    };
                    db.Entry(user);

                    donhang.MaKH = int.Parse(Session["Makh"].ToString());
                    donhang.NgayDatHang = DateTime.Parse(DateTime.Now.ToString());
                    //donhang.NgayGiaoHang = DateTime.Parse(frm["ngaynhanhang"].ToString());
                    donhang.TriGiaDH = decimal.Parse(Session["TongTien"].ToString());
                    donhang.Dagiao = "danggiao";
                    donhang.TenKH = frm["tennguoinhan"];
                    donhang.SDT = frm["dienthoainhanhang"];
                    donhang.DiaChi = frm["diachinhanhang"];
                    donhang.PTTT = frm["pttt"];
                    db.DON_DAT_HANG.Add(donhang);
                    db.SaveChanges();
                    List<SPGH> listSP = LayGioHang();
                    foreach (var item in listSP)
                    {
                        CTDON_HANG ctdh = new CTDON_HANG();
                        ctdh.MaDH = donhang.MaDH;
                        ctdh.MaSP = item.MaSPham;
                        ctdh.SoLuong = item.SoLuong;
                        ctdh.DonGia = (decimal)item.GiaSP;
                        db.CTDON_HANG.Add(ctdh);
                       
                        db.SaveChanges();
                    }
                    Session["GioHang"] = null;
                    return RedirectToAction("ThanhToanThanhCong", "GioHang");
                }
                else
                {

                    donhang.MaKH = int.Parse(Session["Makh"].ToString());
                    donhang.NgayDatHang = DateTime.Parse(DateTime.Now.ToString());
                    //donhang.NgayGiaoHang = DateTime.Parse(frm["ngaynhanhang"].ToString());
                    donhang.TriGiaDH = decimal.Parse(Session["TongTien"].ToString());
                    donhang.Dagiao = "danggiao";
                    donhang.TenKH = frm["tennguoinhan"];
                    donhang.SDT = frm["dienthoainhanhang"];
                    donhang.DiaChi = frm["diachinhanhang"];
                    donhang.PTTT = frm["pttt"];
                    db.DON_DAT_HANG.Add(donhang);
                    db.SaveChanges();
             
                    List<SPGH> listSP = LayGioHang();

                    string content = System.IO.File.ReadAllText(Server.MapPath("~/client/neworder.html"));

                    content = content.Replace("{{CustomerName}}", donhang.TenKH);
                    content = content.Replace("{{Phone}}", donhang.SDT);
                    content = content.Replace("{{Email}}", donhang.Email);
                    content = content.Replace("{{Address}}", donhang.DiaChi);
                    content = content.Replace("{{Total}}", Session["TongTien"].ToString());
                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                    new MailHelper().SendMail(donhang.Email, "Đơn hàng mới từ OnlineShop", content);
                    new MailHelper().SendMail(toEmail, "Đơn hàng mới từ OnlineShop", content);
                    foreach (var item in listSP)
                    {
                        CTDON_HANG ctdh = new CTDON_HANG();
                        ctdh.MaDH = donhang.MaDH;
                        ctdh.MaSP = item.MaSPham;
                        ctdh.SoLuong = item.SoLuong;
                        ctdh.DonGia = (decimal)item.GiaSP;
                        db.CTDON_HANG.Add(ctdh);       
                        db.SaveChanges();

                       
                    }
                    Session["Madh"] = donhang.MaDH;
                    return RedirectToAction("ThanhToanThanhCong", "GioHang");
                }
            }
        }

        public ActionResult ThanhToanThanhCong()
        {
            return View();

        }
    }
}