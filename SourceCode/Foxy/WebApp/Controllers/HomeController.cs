using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Filter;
using WebApp.Models;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;
using Data.Domain.Interfaces.Template;

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
        private ICommonRepo _commonRepo;
        private IWordsElemRelRepo _relationshipsRepo;

        public HomeController(IUsersRepository userRepo, IVocabularTempRepo vocabRepo, IFormularTempRepo formularRepo,
            IQuestionTempRepo questRepo, IAnswerTempRepo ansRepo, ICommonRepo commonRepo, IWordsElemRelRepo relRepo)
        {
            _userRepo = userRepo;
            _vocabRepo = vocabRepo;
            _formularRepo = formularRepo;
            _questRepo = questRepo;
            _ansRepo = ansRepo;
            _commonRepo = commonRepo;
            _relationshipsRepo = relRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                PopulateDb.PopulateDb pop = new PopulateDb.PopulateDb(_userRepo, _vocabRepo, _formularRepo, _questRepo, _ansRepo, _commonRepo, _relationshipsRepo);
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
