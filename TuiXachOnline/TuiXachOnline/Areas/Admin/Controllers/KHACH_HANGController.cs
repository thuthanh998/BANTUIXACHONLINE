using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TuiXachOnline.Models;

namespace TuiXachOnline.Areas.Admin.Controllers
{
    public class KHACH_HANGController : Controller
    {
        private QLTuiXachEntities db = new QLTuiXachEntities();

        // GET: Admin/KHACH_HANG
        public async Task<ActionResult> Index()
        {
            return View(await db.KHACH_HANG.ToListAsync());
        }

        // GET: Admin/KHACH_HANG/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACH_HANG kHACH_HANG = await db.KHACH_HANG.FindAsync(id);
            if (kHACH_HANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACH_HANG);
        }

        // GET: Admin/KHACH_HANG/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/KHACH_HANG/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaKH,TenKH,NgaySinh,DiaChi,SDT,Email,TenDN,MatKhau")] KHACH_HANG kHACH_HANG)
        {
            if (ModelState.IsValid)
            {
                db.KHACH_HANG.Add(kHACH_HANG);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(kHACH_HANG);
        }

        // GET: Admin/KHACH_HANG/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACH_HANG kHACH_HANG = await db.KHACH_HANG.FindAsync(id);
            if (kHACH_HANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACH_HANG);
        }

        // POST: Admin/KHACH_HANG/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaKH,TenKH,NgaySinh,DiaChi,SDT,Email,TenDN,MatKhau")] KHACH_HANG kHACH_HANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kHACH_HANG).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(kHACH_HANG);
        }

        // GET: Admin/KHACH_HANG/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACH_HANG kHACH_HANG = await db.KHACH_HANG.FindAsync(id);
            if (kHACH_HANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACH_HANG);
        }

        // POST: Admin/KHACH_HANG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            KHACH_HANG kHACH_HANG = await db.KHACH_HANG.FindAsync(id);
            db.KHACH_HANG.Remove(kHACH_HANG);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
