using NgCookingMVC_BackEND.Models;
using NgCookingMVC_BackEND.Models.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace NgCookingMVC_BackEND.Controllers
{
    public class HomeController : Controller
    {
        private NgCookingDbContext _context = new NgCookingDbContext();
        private EFRepository<Recettes> RecettesManager;
        public ActionResult Index()
        {
            RecettesManager = new EFRepository<Recettes>(_context);
            List<Recettes> recList = RecettesManager.GetAll().Include(x=>x.Comments).Include(b => b.RecettesIngredients.Select(x => x.Ingredient)).ToList();
            TempData["Recettes"] = recList;
            ViewBag.RecettesTop4 = recList.Take(4);
            TempData["RctNb"] = recList.Count;
            return View();
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