using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Todo.Services;
using Todo.Web.Context;
using Todo.Web.ViewModels;

namespace Todo.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthenticateUserService _authenticateUserService;
        private readonly ISessionContext _sessionContext;

        public AuthController(IAuthenticateUserService authenticateUserService, ISessionContext sessionContext)
        {
            _authenticateUserService = authenticateUserService;
            _sessionContext = sessionContext;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginRequest)
        {
            if (!ModelState.IsValid)
            {
                loginRequest.Password = string.Empty;
                return View(loginRequest);
            }

            var authedUser = await _authenticateUserService.GetUserByUsernameAndPassword(
                loginRequest.Username,
                loginRequest.Password);

            if (authedUser != null)
            {
                _sessionContext.CurrentUser = authedUser;

                return RedirectToAction("Index", "List");
            }
            else
            {
                ModelState.AddModelError("OperationResult", "Username/Password combination not valid");
                loginRequest.Password = string.Empty;

                return View(loginRequest);
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (_sessionContext.CurrentUser == null)
                return RedirectToAction("Index", "Main");

            return View();
        }

        [HttpPost]
        public ActionResult ExecLogout()
        {
            _sessionContext.CurrentUser = null;
            return RedirectToAction("Login");
        }
    }
}