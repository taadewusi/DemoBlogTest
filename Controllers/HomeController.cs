using DemoBlogTest.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DemoBlogTest.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public async Task<ActionResult> Index(int page =1)
        {
            var user = User.Identity.Name;
            int recordsPerPage = 20;
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
            return View(db.Blogs.Where(x => x.User_Id == user).ToPagedList(page, recordsPerPage));
          
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}