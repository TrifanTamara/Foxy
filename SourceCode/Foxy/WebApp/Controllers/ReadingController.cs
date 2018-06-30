using Data.Domain.Entities;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.UserRelated;
using Data.Domain.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DTOs;
using WebApp.Models;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ReadingController : Controller
    {
        private IUserRepo _userRepo;
        private readonly IVocabularItemRepo _vocabularRepo;
        private IFormularItemRepo _formularRepo;

        public ReadingController(IUserRepo userRepo, IVocabularItemRepo vocabularRepo, IFormularItemRepo formularRepo)
        {
            _userRepo = userRepo;
            _vocabularRepo = vocabularRepo;
            _formularRepo = formularRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            List<FormularWrapper> readL = await _formularRepo.GetAllFormByUserAndType(user.UserId, Data.Domain.Entities.TemplateItems.FormType.Reading);
            List<FormularWrapper> listL = await _formularRepo.GetAllFormByUserAndType(user.UserId, Data.Domain.Entities.TemplateItems.FormType.Listening);

            ReadListModel model = new ReadListModel(readL, listL);

            return View("Index", model);
        }

        [HttpGet]
        [Route("listeningform{quizz_nr}")]
        public async Task<IActionResult> QuizzListening([FromRoute]int quizz_nr)
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            FormularWrapper formularW = await _formularRepo.GetByUserAndPvId(user.UserId, quizz_nr, Data.Domain.Entities.TemplateItems.FormType.Listening);
            List<QuestionWrapper> formQuestions = await _formularRepo.GetQuestionsByUserAndPvId(user.UserId, quizz_nr, Data.Domain.Entities.TemplateItems.FormType.Listening);
            if (null == formQuestions) return View("NotFound");

            QuizzModel model = new QuizzModel(formQuestions, formularW);
            
            return View("QuizzRL", model);
        }

        [HttpGet]
        [Route("readingform{quizz_nr}")]
        public async Task<IActionResult> QuizzReading([FromRoute]int quizz_nr)
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);
            FormularWrapper formularW = await _formularRepo.GetByUserAndPvId(user.UserId, quizz_nr, Data.Domain.Entities.TemplateItems.FormType.Reading);
            List<QuestionWrapper> formQuestions = await _formularRepo.GetQuestionsByUserAndPvId(user.UserId, quizz_nr, Data.Domain.Entities.TemplateItems.FormType.Reading);
            if (null == formQuestions) return View("NotFound");

            QuizzModel model = new QuizzModel(formQuestions, formularW);

            return View("QuizzRL", model);
        }
    }
}
