using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.DTO;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _repository;

        public UsersController(IUsersRepository repository)
        {
            _repository = repository;
        }

        [HttpGet, Authorize]
        [Route("All")]
        public async Task<IActionResult> Index()
        {
            return Ok(await _repository.GetAll());
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] CreateUsers usrToCreate)
        {
            User user = Data.Domain.Entities.User.Create(usrToCreate.Name, usrToCreate.Email, usrToCreate.Password, usrToCreate.Description);
            _repository.Add(user);
            return Ok(user);
        }
    }
}
