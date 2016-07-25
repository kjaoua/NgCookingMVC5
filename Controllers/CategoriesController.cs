using NgCookingMVC_BackEND.Models;
using NgCookingMVC_BackEND.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;

namespace NgCookingMVC_BackEND.Controllers
{
    public class CategoriesController : Controller
    {
        private NgCookingDbContext _context = new NgCookingDbContext();
        private EFRepository<Categories> CategoriesManager;
        // GET: Categories
        public ActionResult Index()
        {
            CategoriesManager = new EFRepository<Categories>(_context);
            List<Categories> catList = CategoriesManager.GetAll().ToList();
            TempData["Categories"] = catList;
            ViewBag.Categories = catList;
            return View();
        }
        [System.Web.Http.HttpPost]

        
        public ActionResult Create(Categories categ)
        {
            CategoriesManager = new EFRepository<Categories>(_context);

            if (ModelState.IsValid)
            {
                CategoriesManager.Add(categ);
                CategoriesManager.Save();
                return RedirectToAction("Index");
            }
            return View(categ);
        }

        public ActionResult PostListCategories([FromBody] List<Categories> categories)
        {
            CategoriesManager = new EFRepository<Categories>(_context);
            try
            {
                CategoriesManager.AddRange(categories);
                CategoriesManager.Save();
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
        public ActionResult PostOneCategories([FromBody] Categories categories)
        {
            try
            {
                CategoriesManager.Add(categories);
                CategoriesManager.Save();
                Response.StatusCode = (int)HttpStatusCode.Created;

                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult GetListCategories()
        {
            CategoriesManager = new EFRepository<Categories>(_context);
            try
            {

                List<Categories> cat = CategoriesManager.GetAll().ToList();
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
        public ActionResult GetCategoriesById(int id)
        {
            CategoriesManager = new EFRepository<Categories>(_context);
            try
            {

                Categories cat = CategoriesManager.GetById(id);
                if (cat != null)
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

    }
}