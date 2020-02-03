using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Todo.Common;
using Todo.Services;
using Todo.Web.Context;
using Todo.Web.ViewModels;

namespace Todo.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly ICreateUserService _createUserService;
        private readonly ISessionContext _sessionContext;

        public UserController(ICreateUserService createUserService, ISessionContext sessionContext)
        {
            _createUserService = createUserService;
            _sessionContext = sessionContext;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserViewModel createUserRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(createUserRequest);
            }

            try
            {
                // create the user
                var newUser = await _createUserService.CreateUser(createUserRequest);
                _sessionContext.CurrentUser = newUser;

                return RedirectToAction("Index", "List");
            }
            catch (DuplicateUserException)
            {
                ModelState.AddModelError("Username", "Username already in use");
                return View(createUserRequest);
            }
        }
    }
}