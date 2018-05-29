using Business.Wrappers;
using Data.Domain.Entities;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DTOs;
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
        private static Dictionary<Guid, VocabularWrapper> currentItem = new Dictionary<Guid, VocabularWrapper>();


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
            if (model != null)
            {
                currentItem[user.UserId] = model;
            }
            //return404
            return View("ItemForm", model);
        }

        [HttpGet]
        [Route("kanji/{name}")]
        public async Task<IActionResult> Kanji([FromRoute]string name)
        {

            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            VocabularWrapper model = await _vocabularRepo.GetWrappedItem(user.UserId, name, Data.Domain.Entities.TemplateItems.VocabularType.Kanji);

            if (model != null)
            {
                currentItem[user.UserId] = model;
            }
            return View("ItemForm", model);
        }

        [HttpGet]
        [Route("word/{name}")]
        public async Task<IActionResult> Word([FromRoute]string name)
        {

            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            VocabularWrapper model = await _vocabularRepo.GetWrappedItem(user.UserId, name, Data.Domain.Entities.TemplateItems.VocabularType.Word);
            if (model != null)
            {
                currentItem[user.UserId] = model;
            }

            return View("ItemForm", model);
        }

        [HttpPost]
        [Route("update/meaningNote")]
        public async Task<bool> UpdateMeaningNote(UpdateNoteDto model)
        {
            VocabularItem item = await _vocabularRepo.FindById(model.VocabularId);
            if (item != null)
            {
                item.Update(model.NewContent, item.ReadingNote, item.Favorite);
                await _vocabularRepo.Edit(item);
            }
            return true;
        }

        [HttpPost]
        [Route("update/readingNote")]
        public async Task<bool> UpdateReadingNote(UpdateNoteDto model)
        {
            VocabularItem item = await _vocabularRepo.FindById(model.VocabularId);
            if (item != null)
            {
                item.Update(item.MeaningNote, model.NewContent, item.Favorite);
                await _vocabularRepo.Edit(item);
            }
            return true;
        }

        [HttpPost]
        [Route("update/Favorite")]
        public async Task<bool> UpdateFavorite(ChangeFavoriteDto model)
        {
            VocabularItem item = await _vocabularRepo.FindById(model.VocabularId);
            if (item != null)
            {
                item.Update(item.MeaningNote, item.ReadingNote, !item.Favorite);
                await _vocabularRepo.Edit(item);
                return item.Favorite;
            }
            return false;
        }
    }
}
