using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Todo.Common;
using Todo.Services;
using Todo.Web.ViewModels;

namespace Todo.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly CreateUserService _createUserService;

        public UserController(CreateUserService createUserService)
        {
            _createUserService = createUserService;
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
                SessionContext.Current.CurrentUser = newUser;

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