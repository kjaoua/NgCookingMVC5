using NgCookingMVC_BackEND.Models;
using NgCookingMVC_BackEND.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NgCookingMVC_BackEND.Controllers
{
    public class IngredientsController : Controller
    {


        private NgCookingDbContext _context = new NgCookingDbContext();
        private EFRepository<Ingredients> IngredientsManager;
        private EFRepository<Categories> CategoriesManager;
        // GET: Ingredients
        public ActionResult Index()
        {
            IngredientsManager = new EFRepository<Ingredients>(_context);
            CategoriesManager = new EFRepository<Categories>(_context);
            var ingList = IngredientsManager.GetAll().Include(a => a.Category).OrderBy(a => a.Name);
            TempData["RctNb"] = _context.Recettes.Count();
            ViewBag.Categories = CategoriesManager.GetAll().ToList();
            
            if (TempData["DispNbre"] == null)
            {
                TempData["DispNbre"] = 10;
            };

            if (TempData["Ingredients"] == null)
            {
                TempData["Ingredients"] = ingList.Take((int)TempData["DispNbre"]);
            }


            return View();
        }
        public ActionResult Create()
        {
            CategoriesManager = new EFRepository<Categories>(_context);
            ViewBag.categories = CategoriesManager.GetAll().ToList();

            return View();
        }
        public ActionResult Delete()
        {

            return View();
        }
        [HttpPost]
        public ActionResult IngredientViewList(int idcatId, string NameIng, int MinCal, int MaxCal, bool IncrementDisplay)
        {

            IngredientsManager = new EFRepository<Ingredients>(_context);
            List<Ingredients> ingList = IngredientsManager.GetAll().Include(a => a.Category).ToList();

            if (TempData["DispNbre"] == null)
            {
                TempData["DispNbre"] = 10;
            }
            int dispnbr = (int)TempData["DispNbre"];
            if (IncrementDisplay)
            {
                dispnbr += 4;
                TempData["DispNbre"] = dispnbr;
            }
            if (idcatId != 0)
            {

                ingList = ingList.Where(a => a.CategoryId == idcatId).ToList();
            }
            if (NameIng != "")
            {
                ingList = ingList.Where(a => a.Name.ToUpper().Contains(NameIng.ToUpper())).ToList();

            }
            ingList = ingList.Where(a => a.Calories >= MinCal && a.Calories <= MaxCal).OrderBy(a => a.Name).ToList();
            TempData["Ingredients"] = ingList.Take(dispnbr);



            return PartialView("IngredientViewList");
            //return Json(ingList, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult Create(IngredientViewModel ing, HttpPostedFileBase file)
        {
            IngredientsManager = new EFRepository<Ingredients>(_context);
            CategoriesManager = new EFRepository<Categories>(_context);
            Ingredients ingredient = new Ingredients() { Name = ing.Name, Calories = ing.Calories, Category = CategoriesManager.GetById(ing.CategoryId), IsAvailable = ing.IsAvailable };
            if (file != null && file.ContentLength > 0)
            {

                ingredient.Picture = new byte[file.ContentLength];
                file.InputStream.Read(ingredient.Picture, 0, file.ContentLength);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    IngredientsManager.Add(ingredient);
                    IngredientsManager.Save();
                    TempData["Status"] = "Created";
                    return RedirectToAction("Create");
                }
                catch
                {

                    TempData["Status"] = "NotCreated";
                    return RedirectToAction("Create");
                }

            }
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            IngredientsManager = new EFRepository<Ingredients>(_context);
            var ing = IngredientsManager.GetById(Id);
            IngredientsManager.Delete(ing);
            return View();
        }
        public ActionResult PostListIngredients(List<Ingredients> categories)
        {
            IngredientsManager = new EFRepository<Ingredients>(_context);
            try
            {
                IngredientsManager.AddRange(categories);
                IngredientsManager.Save();
                Response.StatusCode = (int)HttpStatusCode.Created;

                return Json(true, JsonRequestBehavior.AllowGet);


            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [System.Web.Http.HttpPost]

        public ActionResult PostOneIngredients(Ingredients Ingredients)
        {
            IngredientsManager = new EFRepository<Ingredients>(_context);
            EFRepository<Categories> CategoriesManager = new EFRepository<Categories>(_context);
            try
            {

                IngredientsManager.Add(Ingredients);
                IngredientsManager.Save();
                Response.StatusCode = (int)HttpStatusCode.Created;

                return Json(true, JsonRequestBehavior.AllowGet);


            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult GetListIngredients()
        {
            IngredientsManager = new EFRepository<Ingredients>(_context);
            try
            {

                List<Ingredients> cat = IngredientsManager.GetAll().Include(a => a.Category).ToList();
                if ((cat != null) && (cat.Count != 0))
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(cat, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return Json("There is no Comunity", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult GetIngredientsById(int id)
        {
            IngredientsManager = new EFRepository<Ingredients>(_context);
            try
            {

                Ingredients ing = IngredientsManager.GetById(id);
                if (ing != null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(ing, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;

                    return Json("There is no Comunity", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public List<Ingredients> GetAllIng(int CategoryId)
        {
            IngredientsManager = new EFRepository<Ingredients>(_context);
            return IngredientsManager.GetAll().Where(a => a.CategoryId == CategoryId).Take(3).ToList();

        }
    }
}