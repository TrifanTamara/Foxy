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

namespace WebApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class GrammarController : Controller
    {
        private IUserRepo _userRepo;
        private readonly IVocabularItemRepo _vocabularRepo;
        private IFormularItemRepo _formularRepo;

        public GrammarController(IUserRepo userRepo, IVocabularItemRepo vocabularRepo, IFormularItemRepo formularRepo)
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

            GrammarModel model = new GrammarModel(await _formularRepo.GetAllFormByUserAndType(user.UserId, Data.Domain.Entities.TemplateItems.FormType.Grammar));
            
            return View("Index", model);
        }

        [HttpGet]
        [Route("quizz/{quizznr}")]
        public async Task<IActionResult> Quizz([FromRoute]int quizznr)
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            FormularWrapper formularW = await _formularRepo.GetByUserAndPvId(user.UserId, quizznr, Data.Domain.Entities.TemplateItems.FormType.Grammar);
            List<QuestionWrapper> formQuestions = await _formularRepo.GetQuestionsByUserAndPvId(user.UserId, quizznr, Data.Domain.Entities.TemplateItems.FormType.Grammar);
            if (null == formQuestions) return View("NotFound");

            int questionsNr = 5;
            int questTempNr = formQuestions.Count();
            int sizeQuest = questionsNr > questTempNr ? questTempNr : questionsNr;

            List<QuestionWrapper> questList = new List<QuestionWrapper>();
            var rnd = new Random();
            questList = formQuestions.OrderBy(item => rnd.Next()).ToList().GetRange(0, sizeQuest);
            QuizzModel model = new QuizzModel(questList, formularW);

            return View("Quizz", model);
        }

        [HttpGet]
        [Route("formular{gramm}")]
        public async Task<IActionResult> FormGrammar([FromRoute]int gramm)
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            FormularWrapper formular = await _formularRepo.GetByUserAndPvId(user.UserId, gramm, Data.Domain.Entities.TemplateItems.FormType.Grammar);
            if (null == formular) return View("NotFound");
            
            return View("FormGram", formular);
        }

        [HttpPost]
        [Route("update/Favorite")]
        public async Task<bool> UpdateFavorite(ChangeFavoriteDto model)
        {
            FormItem item = await _formularRepo.FindById(model.ElementId);
            if (item != null)
            {
                item.Update(item.Note, item.AverageScore, item.TimesAnswered, !item.Favorite);
                await _formularRepo.Edit(item);
                return item.Favorite;
            }
            return false;
        }

        [HttpPost]
        [Route("update/note")]
        public async Task<bool> UpdateNote(UpdateNoteDto model)
        {
            Guid itemId = Guid.Parse(model.ElementId);
            FormItem item = await _formularRepo.FindById(itemId);
            if (item != null)
            {
                item.Update(model.NewContent, item.AverageScore, item.TimesAnswered, !item.Favorite);
                await _formularRepo.Edit(item);
            }
            return true;
        }
    }
}
