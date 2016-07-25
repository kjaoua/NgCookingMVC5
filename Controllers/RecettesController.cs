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
    public class RecettesController : Controller
    {
        private NgCookingDbContext _context = new NgCookingDbContext();
        private EFRepository<Recettes> RecettesManager;
        private EFRepository<Comments> CommentsManager;
        private EFRepository<NgCookingUser> UsersManager;
        private EFRepository<Categories> CategoriesManager;
        private EFRepository<Ingredients> IngredientsManager;
        // GET: Recettes
        public ActionResult Index()
        {
            RecettesManager = new EFRepository<Recettes>(_context);

           
            
            List<Recettes> recList = RecettesManager.GetAll().Include(a => a.Creator).Include(a=>a.Comments).OrderBy(a => a.Name).ToList();
            TempData["RctNb"] = recList.Count();
            TempData["TopRecettes"] = recList.OrderByDescending(a => a.Comments.Average(x => x.Mark)).Take(4).ToList();
            TempData["NewRecettes"] = recList.OrderByDescending(a => a.Id).Take(4).ToList();
            if (TempData["DispNbre"] == null)
            {
                TempData["DispNbre"] = 4;
            };

            if (TempData["Recettes"] == null)
            {
                //ViewBag.Recettes = recList.Take((int)TempData["DispNbre"]);
                TempData["Recettes"] = recList.Take((int)TempData["DispNbre"]);
            }
            
            
            return View();
        }
        // GET: /Account/Register

        public ActionResult Create()
        {
            CategoriesManager = new EFRepository<Categories>(_context);
            List<Categories> catList = CategoriesManager.GetAll().ToList();
            //ViewBag.Categories = CategoriesManager.GetAll().Select(c => c.Name).ToList();
            ViewBag.Categories = catList.ToList();
            TempData["SelectedIng"] = new HashSet<Ingredients>();
            
            ViewBag.SelectedIng = new HashSet<Ingredients>();
            return View();
        }
        public ActionResult UpdateIngredientList(string operation, int id)
        {

            IngredientsManager = new EFRepository<Ingredients>(_context);
            HashSet<Ingredients> x = new HashSet<Ingredients>();
            x = (HashSet<Ingredients>)TempData["SelectedIng"];
            if (operation == "add")
            {
                if (x == null || x.FirstOrDefault(a => a.Id == id) == null)
                {
                    x.Add(IngredientsManager.GetById(id));
                }

            }
            else
            {

                x.RemoveWhere(a => a.Id == id);
            }

            TempData["SelectedIng"] = x;
            ViewBag.selectedIng = x;
            // Return HTML to update the view
            return View();
        }
        [HttpPost]
        public ActionResult Create(RecettesViewModel postedRct, HttpPostedFileBase file)
        {
            RecettesManager = new EFRepository<Recettes>(_context);
            CommentsManager = new EFRepository<Comments>(_context);
            UsersManager = new EFRepository<NgCookingUser>(_context);
            Recettes Rct = new Recettes() { Name = postedRct.Name, NameToDisplay = postedRct.NameToDisplay, Preparation = postedRct.Preparation, IsAvailable = postedRct.IsAvailable, Creator = UsersManager.GetAll().FirstOrDefault(a => a.Email == User.Identity.Name) };
            HashSet<Ingredients> ingList = (HashSet<Ingredients>)TempData["SelectedIng"];
            if (ingList != null)
            {
                foreach (var ing in ingList)
                {
                    Rct.RecettesIngredients.Add(new RecettesIngredients() { IngredientId = ing.Id, RecetteId = Rct.Id });
                }
            }

            if (file != null && file.ContentLength > 0)
            {
                Rct.Picture = new byte[file.ContentLength];
                file.InputStream.Read(Rct.Picture, 0, file.ContentLength);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    RecettesManager.Add(Rct);
                    CommentsManager.AddRange(Rct.Comments);
                    CommentsManager.Save();
                    RecettesManager.Save();
                    TempData["Status"] = "Created";

                    return RedirectToAction("create");
                }
                catch (Exception e)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    TempData["Status"] = "NotCreated";
                    return RedirectToAction("Create");
                }
            }
            return View();

        }
        [HttpPost]

        public ActionResult AddComment(int RctId,int note, string title, string comment)
        {
            RecettesManager = new EFRepository<Recettes>(_context);
            var rct = RecettesManager.GetAll().Include(x => x.Comments).FirstOrDefault(a => a.Id == RctId);

            try
            {
                Comments comnt = new Comments() { Comment = comment, Mark = note, Title = title, UserId = _context.Users.FirstOrDefault(a => a.UserName == User.Identity.Name).Id };
                rct.Comments.Add(comnt);
                RecettesManager.Update(rct);
                RecettesManager.Save();
                TempData["CommentStatus"] = "Created";
            }
            catch
            {
                TempData["CommentStatus"] = "NotCreated";
                return RedirectToAction("Details");

            }

            return  Json(rct.Comments, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Details(int Id)
        {
            RecettesManager = new EFRepository<Recettes>(_context);
            UsersManager = new EFRepository<NgCookingUser>(_context);

            Recettes rct = RecettesManager.GetAll().Include(x=>x.RecettesIngredients.Select(a=>a.Ingredient)).Include(a=>a.Comments).FirstOrDefault(a=>a.Id == Id);
            ViewBag.Recette = rct;
            TempData["RctIdRecette"] = rct;
            ViewBag.Comments = rct.Comments.ToList();
            List<Recettes> recList = RecettesManager.GetAll().Include(a => a.Creator).Include(a => a.Comments).OrderBy(a => a.Name).ToList();

            TempData["TopRecettes"] = recList.OrderByDescending(a => a.Comments.Average(x => x.Mark)).Take(4).ToList();
            TempData["NewRecettes"] = recList.OrderByDescending(a => a.Id).Take(4).ToList();
            TempData["RctNb"] = recList.Count();
            
            TempData["users"] = UsersManager.GetAll().ToList();
            return View();
        }
        public ActionResult CommentView()
        {
            Recettes rct = (Recettes)TempData["RctIdRecette"];
            TempData["users"] = _context.Users.ToList();
            ViewBag.Comments = rct.Comments.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult RecetteViewList(string IngList, string NameRct, int MinCal,int MaxCal,bool IncrementDisplay, string Order)
        {
            RecettesManager = new EFRepository<Recettes>(_context);
            List<Recettes> filteredListe = RecettesManager.GetAll().Include(a=>a.Comments).Include(a=>a.RecettesIngredients.Select(x=>x.Ingredient)).ToList();
            if (TempData["DispNbre"] == null)
            {
                TempData["DispNbre"] = 4;
            }
            int dispnbr = (int)TempData["DispNbre"];
            if (!IncrementDisplay)
            {
                dispnbr += 4;
                TempData["DispNbre"] = dispnbr;
            }
            var ingredientList = new List<string>();
            if (NameRct != "")
            {
                filteredListe = filteredListe.Where(a => a.Name.ToUpper().Contains(NameRct.ToUpper())).ToList(); ;
            }
            if (IngList != "")
            {

                ingredientList = IngList.Replace(" ", "").Split(';').ToList();
                List<Recettes> a = new List<Recettes>();
                foreach (var rct in filteredListe)
                {
                    List<string> rctIngList = rct.RecettesIngredients.Select(x => x.Ingredient.Name).ToList();
                    rctIngList =rctIngList.ConvertAll(y => y.ToLower());
                    ingredientList = ingredientList.ConvertAll(y => y.ToLower());
                    if (!ingredientList.Except(rctIngList).Any())
                    {
                        a.Add(rct);
                    }

                }
                filteredListe = a;
            }
            switch (Order)
            {
                case "az":
                    filteredListe = filteredListe.OrderBy(a => a.Name).ToList();
                    break;
                case "za":
                    filteredListe = filteredListe.OrderByDescending(a => a.Name).ToList();
                    break;
                case "new":
                    break;
                case "old":
                    filteredListe = filteredListe.OrderByDescending(a=>a.Id).ToList();
                    break;
                case "best":
                    break;
                case "worst":
                    break;
                case "cal":
                    break;
                case "cal_desc":
                    break;
                default:
                    break;
                    

            }
            TempData["Recettes"] = filteredListe.Take(dispnbr);
            return PartialView("RecetteViewList");
        }

        [HttpGet]
        public ActionResult GetIngByCategory(int id)
        {
            IngredientsManager = new EFRepository<Ingredients>(_context);
            try
            {


                Response.StatusCode = (int)HttpStatusCode.Created;
                //List<Ingredients> ingList = IngredientsManager.GetAll().Where(a => a.CategoryId == id).ToList();
                List<Ingredients> ingList = _context.Ingredients.Where(a => a.CategoryId == id).ToList();
                return Json(ingList, JsonRequestBehavior.AllowGet);


            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
        
     
        public ActionResult PostListRecettes(List<Recettes> newRecettes)
        {
            RecettesManager = new EFRepository<Recettes>(_context);
            CommentsManager = new EFRepository<Comments>(_context);
            try
            {
                foreach (var recette in newRecettes)
                {
                    RecettesManager.Add(recette);
                    CommentsManager.AddRange(recette.Comments);

                    CommentsManager.Save();

                }
                RecettesManager.Save();
                Response.StatusCode = (int)HttpStatusCode.Created;
                return Json(true);
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]

       
        public ActionResult GetRecettesById(int id)
        {
            RecettesManager = new EFRepository<Recettes>(_context);
            try
            {

                Recettes rec = RecettesManager.GetById(id);
                if (rec != null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(rec, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("There is no recettes", JsonRequestBehavior.AllowGet);
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