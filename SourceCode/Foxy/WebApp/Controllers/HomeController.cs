using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Filter;
using WebApp.Models;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [DefaultControllerFilter]
    [AllowAnonymous]
    public class HomeController : Controller
    {

        public HomeController(IUsersRepository userRepo, IVocabularTempRepository vocabRepo)
        {
            PopulateDb.PopulateDb pop = new PopulateDb.PopulateDb(userRepo, vocabRepo);
            
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.LoggedIn = false;

            return View();
        }

        [HttpGet]
        [Route("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
