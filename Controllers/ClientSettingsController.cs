using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAPISuite.Models;

namespace WebAPISuite.Controllers
{
    public class ClientSettingsController : Controller
    {
        private ClientContext db = new ClientContext();

        // GET: ClientSettings
        public ActionResult Index()
        {
            var clientSettings = db.ClientSettings.Include(c => c.Client);
            return View(clientSettings.ToList());
        }

        // GET: ClientSettings/Details/5
        public ActionResult Details(Guid? id, string settingName)
        {
            if (string.IsNullOrWhiteSpace(settingName) || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ClientSetting clientSetting = db.ClientSettings.FirstOrDefault(c => c.Name.Equals(settingName, StringComparison.InvariantCultureIgnoreCase) && c.ClientId == id);
            if (clientSetting == null)
            {
                return HttpNotFound();
            }
            return View(clientSetting);
        }

        // GET: ClientSettings/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name");
            return View();
        }

        // POST: ClientSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientId,SubjectLine,AutoReplyToCustomer,EnableFileUpload,GoogleAdwordsEnabled,GoogleConversionId,GoogleConversionLanguage,GoogleConversionFormat,GoogleConversionColour,GoogleConversionLabel,GoogleConversionValue,GoogleConversionCurrency,GoogleRemarketingOnly,Name,AutoReplySubjectLine,AutoReplyBody")] ClientSetting clientSetting)
        {
            if (ModelState.IsValid)
            {
                db.ClientSettings.Add(clientSetting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", clientSetting.ClientId);
            return View(clientSetting);
        }

        // GET: ClientSettings/Edit/5
        public ActionResult Edit(Guid? id, string settingName)
        {
            if (string.IsNullOrWhiteSpace(settingName) || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientSetting clientSetting = db.ClientSettings.FirstOrDefault(c => c.Name.Equals(settingName, StringComparison.InvariantCultureIgnoreCase) && c.ClientId == id);
            if (clientSetting == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", clientSetting.ClientId);
            return View(clientSetting);
        }

        // POST: ClientSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ClientSetting clientSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "Name", clientSetting.ClientId);
            return View(clientSetting);
        }

        // GET: ClientSettings/Delete/5
        public ActionResult Delete(Guid? id, string settingName)
        {
            if (string.IsNullOrWhiteSpace(settingName) || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientSetting clientSetting = db.ClientSettings.FirstOrDefault(c => c.Name.Equals(settingName, StringComparison.InvariantCultureIgnoreCase) && c.ClientId == id);
            if (clientSetting == null)
            {
                return HttpNotFound();
            }
            return View(clientSetting);
        }

        // POST: ClientSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id, string settingName)
        {
            ClientSetting clientSetting = db.ClientSettings.FirstOrDefault(c => c.Name.Equals(settingName, StringComparison.InvariantCultureIgnoreCase) && c.ClientId == id);
            db.ClientSettings.Remove(clientSetting);
            db.SaveChanges();
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
