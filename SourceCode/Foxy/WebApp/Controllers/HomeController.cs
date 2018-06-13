using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Filter;
using WebApp.Models;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [DefaultControllerFilter]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private IUsersRepository _userRepo;
        private IVocabularTempRepo _vocabRepo;
        private IFormularTempRepo _formularRepo;
        private IQuestionTempRepo _questRepo;
        private IAnswerTempRepo _ansRepo;

        public HomeController(IUsersRepository userRepo, IVocabularTempRepo vocabRepo, IFormularTempRepo formularRepo,
            IQuestionTempRepo questRepo, IAnswerTempRepo ansRepo)
        {
            _userRepo = userRepo;
            _vocabRepo = vocabRepo;
            _formularRepo = formularRepo;
            _questRepo = questRepo;
            _ansRepo = ansRepo;


        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                PopulateDb.PopulateDb pop = new PopulateDb.PopulateDb(_userRepo, _vocabRepo, _formularRepo, _questRepo, _ansRepo);
                await pop.Populate();
                await _vocabRepo.CalcTotalNumberLevel();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

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
