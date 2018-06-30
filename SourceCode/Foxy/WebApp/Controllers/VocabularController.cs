using Business.Wrappers;
using Data.Domain.Entities;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Interfaces;
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
    public class VocabularController : Controller
    {
        private IUserRepo _userRepo;
        private IVocabularItemRepo _vocabularRepo;

        public VocabularController(IUserRepo userRepo, IVocabularItemRepo vocabularRepo)
        {
            _userRepo = userRepo;
            _vocabularRepo = vocabularRepo;
        }


        [HttpGet]
        [Route("")]
        [Route("index")]
        public IActionResult Index()
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
            if (model == null)
            {
                return View("NotFound");
            }
            return View("ItemForm", model);
        }

        [HttpGet]
        [Route("kanji/{name}")]
        public async Task<IActionResult> Kanji([FromRoute]string name)
        {

            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            VocabularWrapper model = await _vocabularRepo.GetWrappedItem(user.UserId, name, Data.Domain.Entities.TemplateItems.VocabularType.Kanji);

            if (model == null)
            {
                return View("NotFound");
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
            if (model == null)
            {
                return View("NotFound");
            }

            return View("ItemForm", model);
        }

        [HttpPost]
        [Route("update/meaningNote")]
        public async Task<bool> UpdateMeaningNote(UpdateNoteDto model)
        {
            if (model.ElementId != null)
            {
                Guid itemId = Guid.Parse(model.ElementId);
                VocabularItem item = await _vocabularRepo.FindById(itemId);
                if (item != null)
                {
                    item.Update(model.NewContent, item.ReadingNote, item.Favorite, item.UserSynonyms);
                    await _vocabularRepo.Edit(item);
                }
            }
            return true;
        }

        [HttpPost]
        [Route("update/readingNote")]
        public async Task<bool> UpdateReadingNote(UpdateNoteDto model)
        {
            if (model.ElementId != null)
            {
                Guid itemId = Guid.Parse(model.ElementId);
                VocabularItem item = await _vocabularRepo.FindById(itemId);
                if (item != null)
                {
                    item.Update(item.MeaningNote, model.NewContent, item.Favorite, item.UserSynonyms);
                    await _vocabularRepo.Edit(item);
                }
            }
            return true;
        }

        [HttpPost]
        [Route("update/Favorite")]
        public async Task<bool> UpdateFavorite(ChangeFavoriteDto model)
        {
            if (model.ElementId != null)
            {
                VocabularItem item = await _vocabularRepo.FindById(model.ElementId);
                if (item != null)
                {
                    item.Update(item.MeaningNote, item.ReadingNote, !item.Favorite, item.UserSynonyms);
                    await _vocabularRepo.Edit(item);
                    return item.Favorite;
                }
            }
            return false;
            
        }

        [HttpPost]
        [Route("addSynonym")]
        public async Task<JsonResult> AddSynonym(UpdateNoteDto model)
        {
            if (model.ElementId != null)
            {
                Guid itemId = Guid.Parse(model.ElementId);
                VocabularItem item = await _vocabularRepo.FindById(itemId);
                if (item != null && !model.NewContent.Equals(""))
                {
                    string syn = item.UserSynonyms;
                    if (!syn.Equals("")) syn += ";";
                    syn += model.NewContent;
                    item.Update(item.MeaningNote, item.ReadingNote, !item.Favorite, syn);
                    await _vocabularRepo.Edit(item);
                    return Json(new { synonyms = syn });
                }
            }
            return Json(new { synonyms = "" });
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
        [Route("levels/{levelNr:int}")]
        public async Task<IActionResult> Levels([FromRoute]int levelNr)
        {

            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            VocabLevelsModel model = new VocabLevelsModel();
            model.Levels = await _vocabularRepo.GetGroupedVocabLevels(user.UserId, levelNr, levelNr);
            model.RequestedInfo = InfoRequested.AllInfo;

            return View("Levels", model);
        }

        [HttpGet]
        [Route("lexicon/{type}")]
        public async Task<IActionResult> Lexicon([FromRoute]string type)
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            VocabLevelsModel model = new VocabLevelsModel();
            switch (type.ToLower())
            {
                case "radical" :
                    model.RequestedInfo = InfoRequested.JustRadical;
                    break;
                case "kanji":
                    model.RequestedInfo = InfoRequested.JustKanji;
                    break;
                case "words":
                    model.RequestedInfo = InfoRequested.JustWords;
                    break;
                default:
                    model.RequestedInfo = InfoRequested.AllInfo;
                    break;
            }
            
            model.Levels = await _vocabularRepo.GetGroupedVocabLevels(user.UserId);
            
            return View("Levels", model);
        }

        [HttpGet]
        [Route("grid")]
        public async Task<IActionResult> ShowVocabularGrid()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            VocabLevelsModel model = new VocabLevelsModel();
            model.Levels = await _vocabularRepo.GetGroupedVocabLevels(user.UserId);

            return View("Grid", model);
        }

        [HttpGet]
        [Route("progress")]
        public async Task<IActionResult> ShowVocabularProgress()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            List<VocabularWrapper> totalList = await _vocabularRepo.WrapVocabular((await _vocabularRepo.GetVocabByUser(user.UserId)).ToList());

          

            ProgressVocabModel model = new ProgressVocabModel();
            model.AllVocabular = model.OrderListByProgress(totalList);
            model.Radical = model.OrderListByProgress(totalList.Where(x => x.Template.Type == VocabularType.Radical).ToList());
            model.Kanji = model.OrderListByProgress(totalList.Where(x => x.Template.Type == VocabularType.Kanji).ToList());
            model.Words = model.OrderListByProgress(totalList.Where(x => x.Template.Type == VocabularType.Word).ToList());

            return View("Progress", model);
        }
    }
}
