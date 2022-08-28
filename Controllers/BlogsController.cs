using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoBlogTest.Models;
using Newtonsoft.Json;
using System.Net.Http;
using PagedList;
using System.Configuration;
using System.Web.Configuration;

namespace DemoBlogTest.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Blogs
        public async Task<ActionResult> Index(int page=1)
        {
            var user = User.Identity.Name;
            int recordsPerPage = 20;
           var sysadmin=  WebConfigurationManager.AppSettings["SysAdmin"];
            if (user == sysadmin)
            {
                using (var https = new HttpClient())
                {
                    using (var response = await https.GetAsync("https://mocki.io/v1/d33691f7-1eb5-45aa-9642-8d538f6c5ebd"))
                    {
                        var jsonvaries = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<APICall>(jsonvaries);
                        if (result.data.Count > 0)
                        {
                            var createdlocally = db.Blogs.ToList();
                            createdlocally.AddRange((result.data));
                            var m = createdlocally.ToPagedList(page, recordsPerPage);
                            return View(m);
                        }

                    }
                }
            }
            var normaluser = db.Blogs.Where(x => x.User_Id == user).ToList();
            var paged = normaluser.ToPagedList(page, recordsPerPage);
           return View(paged);
        }

        // GET: Blogs/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = await db.Blogs.FindAsync(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Blogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BlogId,Title,Description,Publication_date,User_Id")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                var check = db.Blogs.Where(x => x.Title == blog.Title).ToList();
                if (check.Count<1)
                {
                    blog.BlogId = Guid.NewGuid();
                    blog.User_Id = User.Identity.Name;
                    blog.Publication_date = DateTime.Now;
                    db.Blogs.Add(blog);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }               
            }

            return View(blog);
        }

        // GET: Blogs/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = await db.Blogs.FindAsync(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BlogId,Title,Description,Publication_date,User_Id")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blog).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = await db.Blogs.FindAsync(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Blog blog = await db.Blogs.FindAsync(id);
            db.Blogs.Remove(blog);
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
