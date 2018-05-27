using Business.Wrappers;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Filter;

namespace WebApp.Controllers
{
    [DefaultControllerFilter]
    [Authorize]
    [Route("[controller]")]
    public class VocabularController : Controller
    {
        private IUsersRepository _userRepo;
        private IVocabularItemRepository _vocabularRepo;

        public VocabularController(IUsersRepository userRepo, IVocabularItemRepository vocabularRepo)
        {
            _userRepo = userRepo;
            _vocabularRepo = vocabularRepo;
        }


        [HttpGet]
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        [Route("radical/{name}")]
        public async Task<IActionResult> Radical([FromRoute]string name)
        {

            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            VocabularWrapper model = await _vocabularRepo.GetWrappedItem(user.UserId, name, Data.Domain.Entities.TemplateItems.VocabularType.Radical);


            return View("ItemForm", model);
        }

        [HttpGet]
        [Route("kanji/{name}")]
        public async Task<IActionResult> Kanji([FromRoute]string name)
        {

            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            VocabularWrapper model = await _vocabularRepo.GetWrappedItem(user.UserId, name, Data.Domain.Entities.TemplateItems.VocabularType.Kanji);


            return View("ItemForm", model);
        }

        [HttpGet]
        [Route("word/{name}")]
        public async Task<IActionResult> Word([FromRoute]string name)
        {

            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            VocabularWrapper model = await _vocabularRepo.GetWrappedItem(user.UserId, name, Data.Domain.Entities.TemplateItems.VocabularType.Word);


            return View("ItemForm", model);
        }

    }
}
