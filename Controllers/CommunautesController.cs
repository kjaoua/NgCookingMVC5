using NgCookingMVC_BackEND.Models;
using NgCookingMVC_BackEND.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace NgCookingMVC_BackEND.Controllers
{
    public class CommunautesController : Controller
    {
         

        private NgCookingDbContext _context = new NgCookingDbContext();
        private EFRepository<NgCookingUser> CommunautesManager;
        // GET: Communautes
        public ActionResult Index()
        {
            CommunautesManager = new EFRepository<NgCookingUser>(_context);
            var commList = CommunautesManager.GetAll();
            TempData["Communautes"] = commList;
            TempData["RctNb"] = _context.Recettes.Count();
            ViewBag.Communauties = commList;
            return View(commList);
        }
        [System.Web.Http.HttpPost]
        public ActionResult Create(NgCookingUser communaute)
        {
            CommunautesManager = new EFRepository<NgCookingUser>(_context);

            if (ModelState.IsValid)
            {
                CommunautesManager.Add(communaute);
                CommunautesManager.Save();
                return RedirectToAction("Index");
            }
            return View(communaute);
        }
        public ActionResult Details()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Details(string Id)
        {
            CommunautesManager = new EFRepository<NgCookingUser>(_context);
            
            if(Id == null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    Id = CommunautesManager.GetAll().FirstOrDefault(a => a.UserName == User.Identity.Name).Id;
                }
                else return View();
            }
                NgCookingUser cat = CommunautesManager.GetById(Id);
            ViewBag.user = cat;
            var recettes = _context.Recettes.Where(a => a.CreatorId == Id).ToList();
            ViewBag.Recettes = recettes;
            TempData["RctNb"] = _context.Recettes.Count();
            return View();
        }
        public ActionResult PostListCommunautes( List<NgCookingUser> categories)
        {
            CommunautesManager = new EFRepository<NgCookingUser>(_context);
            try
            {
                CommunautesManager.AddRange(categories);
                CommunautesManager.Save();

                Response.StatusCode = (int)HttpStatusCode.Created;

                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult PostOneCommunautes( NgCookingUser communautes)
        {
            CommunautesManager = new EFRepository<NgCookingUser>(_context);
            try
            {
                CommunautesManager.Add(communautes);
                CommunautesManager.Save();
                Response.StatusCode = (int)HttpStatusCode.Created;

                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult GetListCommunautes()
        {
            CommunautesManager = new EFRepository<NgCookingUser>(_context);
            try
            {

                List<NgCookingUser> cat = CommunautesManager.GetAll().ToList();
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
        public ActionResult GetCommunautesById(int id)
        {
            CommunautesManager = new EFRepository<NgCookingUser>(_context);
            try
            {

                NgCookingUser cat = CommunautesManager.GetById(id);
                if (cat != null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(cat, JsonRequestBehavior.AllowGet);
                }
               
                else{
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