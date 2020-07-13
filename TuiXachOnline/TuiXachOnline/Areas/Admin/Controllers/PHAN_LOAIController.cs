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
    public class PHAN_LOAIController : Controller
    {
        private QLTuiXachEntities db = new QLTuiXachEntities();

        // GET: Admin/PHAN_LOAI
        public async Task<ActionResult> Index()
        {
            return View(await db.PHAN_LOAI.ToListAsync());
        }

        // GET: Admin/PHAN_LOAI/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHAN_LOAI pHAN_LOAI = await db.PHAN_LOAI.FindAsync(id);
            if (pHAN_LOAI == null)
            {
                return HttpNotFound();
            }
            return View(pHAN_LOAI);
        }

        // GET: Admin/PHAN_LOAI/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/PHAN_LOAI/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaLoai,TenLoai")] PHAN_LOAI pHAN_LOAI)
        {
            if (ModelState.IsValid)
            {
                db.PHAN_LOAI.Add(pHAN_LOAI);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pHAN_LOAI);
        }

        // GET: Admin/PHAN_LOAI/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHAN_LOAI pHAN_LOAI = await db.PHAN_LOAI.FindAsync(id);
            if (pHAN_LOAI == null)
            {
                return HttpNotFound();
            }
            return View(pHAN_LOAI);
        }

        // POST: Admin/PHAN_LOAI/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaLoai,TenLoai")] PHAN_LOAI pHAN_LOAI)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pHAN_LOAI).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pHAN_LOAI);
        }

        // GET: Admin/PHAN_LOAI/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHAN_LOAI pHAN_LOAI = await db.PHAN_LOAI.FindAsync(id);
            if (pHAN_LOAI == null)
            {
                return HttpNotFound();
            }
            return View(pHAN_LOAI);
        }

        // POST: Admin/PHAN_LOAI/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PHAN_LOAI pHAN_LOAI = await db.PHAN_LOAI.FindAsync(id);
            db.PHAN_LOAI.Remove(pHAN_LOAI);
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
