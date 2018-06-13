using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Filter;
using WebApp.Models;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System;

namespace WebApp.Controllers
{
    [DefaultControllerFilter]
    [AllowAnonymous]
    public class HomeController : Controller
    {

        public HomeController(IUsersRepository userRepo, IVocabularTempRepo vocabRepo, IFormularTempRepo formularRepo,
            IQuestionTempRepo questRepo, IAnswerTempRepo ansRepo)
        {
            try
            {
                PopulateDb.PopulateDb pop = new PopulateDb.PopulateDb(userRepo, vocabRepo, formularRepo, questRepo, ansRepo);
                vocabRepo.CalcTotalNumberLevel();
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return View();
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet]
        [Route("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
