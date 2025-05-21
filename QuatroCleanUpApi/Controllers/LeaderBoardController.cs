using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuatroCleanUpApi.Controllers
{
    public class LeaderBoardController : Controller
    {
        // GET: LeaderBoardController
        public ActionResult Index()
        {
            return View();
        }

        // GET: LeaderBoardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LeaderBoardController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaderBoardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaderBoardController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaderBoardController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaderBoardController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaderBoardController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
