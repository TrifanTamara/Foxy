using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.UserRelated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Filter;
using WebApp.Models;

namespace WebApp.Controllers
{
    [DefaultControllerFilter]
    [Authorize]
    public class GrammarController : Controller
    {
        private IUsersRepository _userRepo;
        private IVocabularItemRepo _vocabularRepo;
        private IFormularItemRepo _formularRepo;

        public GrammarController(IUsersRepository userRepo, IVocabularItemRepo vocabularRepo, IFormularItemRepo formularRepo)
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

            GrammarModel model = new GrammarModel();
            model.Formulars = await _formularRepo.GetAllFormByUserAndType(user.UserId, Data.Domain.Entities.TemplateItems.FormularType.Grammar);

            return View("Index", model);
        }
    }
}
