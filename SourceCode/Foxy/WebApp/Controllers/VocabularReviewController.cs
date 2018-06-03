using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Filter;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [DefaultControllerFilter]
    [Authorize]
    [Route("[controller]")]
    public class VocabularReviewController : Controller
    {
        private IUsersRepository _userRepo;
        private IVocabularItemRepository _vocabularRepo;
        private static Dictionary<Guid, LessonModel> currentSeesion = new Dictionary<Guid, LessonModel>();
        private IMainService _service;

        public VocabularReviewController(IUsersRepository userRepo, IVocabularItemRepository vocabularRepo, IMainService mainSerice)
        {
            _userRepo = userRepo;
            _vocabularRepo = vocabularRepo;
            _service = mainSerice;
        }


    }
}
