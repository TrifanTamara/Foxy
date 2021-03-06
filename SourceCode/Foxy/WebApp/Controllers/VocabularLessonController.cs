﻿using Business.Wrappers;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DTOs;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class VocabularLessonController : Controller
    {
        private IUserRepo _userRepo;
        private IVocabularItemRepo _vocabularRepo;
        private IMainService _service;
        private static Dictionary<Guid, LessonModel> currentSeesion = new Dictionary<Guid, LessonModel>();

        public VocabularLessonController(IUserRepo userRepo, IVocabularItemRepo vocabularRepo, IMainService mainServ)
        {
            _userRepo = userRepo;
            _vocabularRepo = vocabularRepo;
            _service = mainServ;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            return RedirectToAction("LessonPage", new RouteValueDictionary(new { controller = "VocabularLesson" }));
        }

        [HttpGet]
        [Route("LessonPage")]
        public async Task<IActionResult> LessonPage([FromRoute]string name)
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            LessonModel model = new LessonModel();
            //set model list and instantiate
            model.LessonList = await _vocabularRepo.GetItemsForLesson(user.UserId);
            if (model.LessonList.Count != 0)
            {
                model.ItemVisited = new List<bool>();
                foreach (var x in model.LessonList) model.ItemVisited.Add(false);
                model.CurrentIndex = 0;
                model.ItemVisited[0] = true;
                model.ReviewActive = false;
                currentSeesion[user.UserId] = model;
                return View("LessonPage", model);
            } else
            {
                return View("EmptyLesson");
            }
        }
        
        [HttpGet]
        [Route("LessonReview")]
        public async Task<IActionResult> LessonReview()
        {
            string email = HttpContext.User.Claims.First().Value;
            User user = await _userRepo.GetByEmail(email);

            LessonModel model = currentSeesion[user.UserId];
            await _service.StartReviewSession(user.UserId, true, model.LessonList);

            return RedirectToAction("ReviewLesson", "VocabularReview");
        }

    }
}
