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
        private IVocabularItemRepo _vocabularRepo;
        private static Dictionary<Guid, VocabularWrapper> currentItem = new Dictionary<Guid, VocabularWrapper>();


        public VocabularController(IUsersRepository userRepo, IVocabularItemRepo vocabularRepo)
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
                item.Update(model.NewContent, item.ReadingNote, item.Favorite, item.UserSynonyms);
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
                item.Update(item.MeaningNote, model.NewContent, item.Favorite, item.UserSynonyms);
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
                item.Update(item.MeaningNote, item.ReadingNote, !item.Favorite, item.UserSynonyms);
                await _vocabularRepo.Edit(item);
                return item.Favorite;
            }
            return false;
        }

        [HttpPost]
        [Route("addSynonym")]
        public async Task<JsonResult> AddSynonim(UpdateNoteDto model)
        {
            VocabularItem item = await _vocabularRepo.FindById(model.VocabularId);
            if (item != null && !model.NewContent.Equals(""))
            {
                string syn = item.UserSynonyms;
                if (!syn.Equals("")) syn += ";";
                syn += model.NewContent;
                item.Update(item.MeaningNote, item.ReadingNote, !item.Favorite, syn);
                await _vocabularRepo.Edit(item);
                return Json(new { synonyms = syn });
            }
            return Json("");
        }

        [HttpPost]
        [Route("removeSynonym")]
        public async Task<JsonResult> RemoveSynonim(RemoveSynVocDto model)
        {
            VocabularItem item = await _vocabularRepo.FindById(model.VocabularId);
            if (item != null && model.Index<5)
            {
                string syn = item.UserSynonyms;
                string result="";
                List<string> listSyn = new List<string>(syn.Split(";"));
                listSyn.Remove(listSyn[model.Index]);
                if (listSyn.Count() > 0) result = listSyn[0];
                for(int i=1; i<listSyn.Count();++i) result+=";"+listSyn[i];
                
                item.Update(item.MeaningNote, item.ReadingNote, !item.Favorite, result);
                await _vocabularRepo.Edit(item);
                return Json(new { synonyms = result });
            }
            return Json("");
        }

        [HttpGet]
        [Route("levels/level")]
        public async Task<JsonResult> RemoveSynonim(RemoveSynVocDto model)
        {
            VocabularItem item = await _vocabularRepo.FindById(model.VocabularId);
            if (item != null && model.Index < 5)
            {
                string syn = item.UserSynonyms;
                string result = "";
                List<string> listSyn = new List<string>(syn.Split(";"));
                listSyn.Remove(listSyn[model.Index]);
                if (listSyn.Count() > 0) result = listSyn[0];
                for (int i = 1; i < listSyn.Count(); ++i) result += ";" + listSyn[i];

                item.Update(item.MeaningNote, item.ReadingNote, !item.Favorite, result);
                await _vocabularRepo.Edit(item);
                return Json(new { synonyms = result });
            }
            return Json("");
        }

        [HttpGet]
        [Route("levels/l{level_nr}")]
        public async Task<IActionResult> Word([FromRoute]int level_nr)
        {

            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            

            return View("ItemForm", model);
        }
    }
}
