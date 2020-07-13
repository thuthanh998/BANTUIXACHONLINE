using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuiXachOnline.Models;
using System.ComponentModel;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using PagedList;
using System.Data.Entity;
namespace TuiXachOnline.Areas.Admin.Controllers
{
    public class AminCRUDController : Controller
    {
        QLTuiXachEntities db = new QLTuiXachEntities();
        // GET: Admin/AdminCRUD
        public class HttpParamActionAttribute : ActionNameSelectorAttribute
        {
            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                    return true;

                var request = controllerContext.RequestContext.HttpContext.Request;
                return request[methodInfo.Name] != null;
            }
        }
        [HttpGet]
        public ActionResult Index(int? size, int? page, string sortProperty, string sortOrder, string searchString)
        {
            ViewBag.searchValue = searchString;
            ViewBag.sortProperty = sortProperty;
            ViewBag.page = page;

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });

            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            ViewBag.size = items;
            ViewBag.currentSize = size;

            var links = from l in db.SANPHAMs select l;
            // 5. T?o thu?c tính s?p x?p m?c d?nh là "LinkID"
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "MaSP";

            // 5. S?p x?p tang/gi?m b?ng phuong th?c OrderBy s? d?ng trong thu vi?n Dynamic LINQ
            if (sortOrder == "desc") links = links.OrderBy(sortProperty + " desc");
            else if (sortOrder == "asc") links = links.OrderBy(sortProperty);
            else links = links.OrderBy("TenSP");

            if (!String.IsNullOrEmpty(searchString))
            {
                links = links.Where(s => s.TenSP.Contains(searchString));
            }

            page = page ?? 1;


            int pageSize = (size ?? 5);

            ViewBag.pageSize = pageSize;

            // 6. Toán t? ?? trong C# mô t? n?u page khác null thì l?y giá tr? page, còn
            // n?u page = null thì l?y giá tr? 1 cho bi?n pageNumber. --- dammio.com
            int pageNumber = (page ?? 1);

            // 6.2 L?y t?ng s? record chia cho kích thu?c d? bi?t bao nhiêu trang
            int checkTotal = (int)(links.ToList().Count / pageSize) + 1;
            // N?u trang vu?t qua t?ng s? trang thì thi?t l?p là 1 ho?c t?ng s? trang
            if (pageNumber > checkTotal) pageNumber = checkTotal;

            return View(links.ToPagedList(pageNumber, pageSize));

        }
        [HttpPost, HttpParamAction]
        public ActionResult Reset()
        {
            ViewBag.searchValue = "";
            Index(null, null, "", "", "");
            return View();

        }
        public ActionResult Edit(int? id)
        {

            ViewBag.imgurl = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id).HinhMinhHoa;
            // List<Category> lis = db.Categories.ToList();

            SANPHAM sp = db.SANPHAMs.Find(id);
            return View(sp);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,MaNSX,MaLoai,DonGia,HinhMinhHoa,SoLuong,NoiDung")] SANPHAM sp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sp);
        }
        public ActionResult Delete(int? id)
        {
            SANPHAM sp = db.SANPHAMs.Find(id);
            return View(sp);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SANPHAM sp = db.SANPHAMs.Find(id);
            db.SANPHAMs.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChitietBookAdmin(int? MaSP)
        {

            SANPHAM b = (from p in db.SANPHAMs where p.MaSP == MaSP select p).ToArray()[0];

            return View(b);
        }

        public ActionResult Create()
        {
            List<NHA_SAN_XUAT> pub = db.NHA_SAN_XUAT.ToList();
            SelectList ListPub = new SelectList(pub, "MaNSX", "TenNSX");
            List<PHAN_LOAI> cate = db.PHAN_LOAI.ToList();
            SelectList ListCate = new SelectList(cate, "MaLoai", "TenLoai");

 
            ViewBag.CategoryList = ListCate;
            ViewBag.PubList = ListPub;
            return View();
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            //xử lí upload
            file.SaveAs(Server.MapPath("~/images/" + file.FileName));
            return "/images/" + file.FileName;
        }
        public string UploadEdit(HttpPostedFileBase file)
        {
            //xử lí upload
            file.SaveAs(Server.MapPath("~/images" + file.FileName));
            return file.FileName;
        }
        [HttpPost]

        public ActionResult Tao(FormCollection frmTao, SANPHAM sp)
        {

            sp.TenSP = frmTao["title"];
            sp.NoiDung = frmTao["mota"];
            sp.MaLoai = Convert.ToInt32(frmTao["cate"]);
            sp.MaNSX = Convert.ToInt32(frmTao["pub"]);
            sp.DonGia = Convert.ToDecimal(frmTao["gia"]);
            sp.SoLuong = Convert.ToInt32(frmTao["SL"]);
            sp.HinhMinhHoa = frmTao["fileUpload"];
            db.SANPHAMs.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index", "AminCRUD");
        }

        //public ActionResult createAuthor()
        //{
        //    var f = from s in db.p select s;
        //    ViewBag.sklist = db.Authors.ToList();
        //    return View();

        //}
        //[HttpPost]
        //public ActionResult createAuthor(FormCollection frmCreate, Author a)
        //{

        //    a.AuthorName = frmCreate["AuthorName"];
        //    a.History = frmCreate["History"];

        //    db.Authors.Add(a);
        //    db.SaveChanges();
        //    return RedirectToAction("createAuthor", "AdminCRUD");
        //}
        public ActionResult createPub()
        {
            var f = from s in db.NHA_SAN_XUAT select s;
            ViewBag.sklist = db.NHA_SAN_XUAT.ToList();
            return View();

        }
        [HttpPost]
        public ActionResult createPub(FormCollection frmCreate, NHA_SAN_XUAT p)
        {

            p.TenNSX = frmCreate["TenNSX"];
            p.DiaChi = frmCreate["DiaChi"];

            db.NHA_SAN_XUAT.Add(p);
            db.SaveChanges();
            return RedirectToAction("createPub", "AminCRUD");
        }
        public ActionResult createCate()
        {
            var f = from s in db.PHAN_LOAI select s;
            ViewBag.sklist = db.PHAN_LOAI.ToList();
            return View();

        }
        [HttpPost]
        public ActionResult createCate(FormCollection frmCreate, PHAN_LOAI c)
        {

            c.TenLoai = frmCreate["CateName"];
            //c. = frmCreate["Description"];

            db.PHAN_LOAI.Add(c);
            db.SaveChanges();
            return RedirectToAction("createCate", "AminCRUD");
        }
        public ActionResult Hoadon()
        {
            var f = from s in db.DON_DAT_HANG select s;
            ViewBag.sklist = db.DON_DAT_HANG.ToList();
            return View();

        }
        public ActionResult EditHD(int? id)
        {
            DON_DAT_HANG sp = db.DON_DAT_HANG.Find(id);
            return View(sp);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHD([Bind(Include = "MaDH,MaKH,TenKH,DiaChi,SDT,Email,NgayDatHang,NgayGiaoHang,TriGiaDH,PTTT,HTGH,Dagiao")] DON_DAT_HANG sp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Hoadon", "AminCRUD");
            }
            return View(sp);
        }
        public ActionResult CTHoadon()
        {
            var f = from s in db.CTDON_HANG select s;
            ViewBag.sklist = db.CTDON_HANG.ToList();
            return View();

        }
        public ActionResult Khachhang()
        {
            var f = from s in db.KHACH_HANG select s;
            ViewBag.sklist = db.KHACH_HANG.ToList();
            return View();

        }
    }
}