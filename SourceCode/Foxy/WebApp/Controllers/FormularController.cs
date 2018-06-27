using Data.Domain.Entities;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.UserRelated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DTOs;

namespace WebApplication.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class FormularController : Controller
    {
        private IUserRepo _userRepo;
        private readonly IVocabularItemRepo _vocabularRepo;
        private IFormularItemRepo _formularRepo;
        private IQuestionItemRepo _questionRepo;

        public FormularController(IUserRepo userRepo, IVocabularItemRepo vocabularRepo, 
            IFormularItemRepo formularRepo, IQuestionItemRepo questionRepo)
        {
            _userRepo = userRepo;
            _vocabularRepo = vocabularRepo;
            _formularRepo = formularRepo;
            _questionRepo = questionRepo;
        }

        [HttpPost]
        [Route("AnswerQuestions")]
        public async Task AnswerQuest([FromBody]ListQuestionAnsDto model)
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            if (model != null)
            {
                int rightAns = 0;
                int wrongAns = 0;
                foreach (var q in model.ListQ) {
                    Guid qg = Guid.Parse(q.Id);
                    if (qg != null)
                    {
                        QuestionItem quest = await _questionRepo.FindById(qg);
                        if (q.Answer)
                        {
                            rightAns += 1;
                            quest.Update(quest.RightAnswers + 1, quest.WrongAnswers);
                        }
                        else
                        {
                            wrongAns += 1;
                            quest.Update(quest.RightAnswers, quest.WrongAnswers + 1);
                        }
                        await _questionRepo.Edit(quest);
                    }
                }
                Guid fg = Guid.Parse(model.FormularId);
                if (fg != null)
                {
                    FormItem form = await _formularRepo.FindById(fg);
                    int total = form.TimesAnswered + rightAns + wrongAns;
                    float average = (form.AverageScore * form.TimesAnswered + rightAns) / (total*2);
                    form.Update(form.Note, average, total, form.Favorite);
                    await _formularRepo.Edit(form);
                }
            }
        }
    }
}
