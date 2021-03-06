﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;
using Data.Domain.Interfaces.Template;
using System.Linq;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private IUserRepo _userRepo;
        private IVocabularTempRepo _vocabRepo;
        private IFormularTempRepo _formularRepo;
        private IQuestionTempRepo _questRepo;
        private IAnswerTempRepo _ansRepo;
        private ICommonRepo _commonRepo;
        private IWordsElemRelRepo _relationshipsRepo;

        public HomeController(IUserRepo userRepo, IVocabularTempRepo vocabRepo, IFormularTempRepo formularRepo,
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
            var user = HttpContext.User.Claims.Count();
            if(user!=0)
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Dashboard" }));

            return View();
        }
        

    }
}
