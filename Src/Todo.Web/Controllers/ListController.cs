using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Todo.Data;
using Todo.Web.ViewModels;
using Todo.Services;
using System;
using System.Data.Entity;
using Todo.Web.ViewModels.Extensions;
using Todo.Common;
using Todo.Web.Context;

namespace Todo.Web.Controllers
{
    [SessionAuthorize]
    public class ListController : Controller
    {
        private readonly IContext _context;
        private readonly ISaveListService _saveListService;
        private readonly IDeleteService _deleteListService;
        private readonly ISessionContext _sessionContext;

        public ListController(IContext context, ISaveListService saveListService, IDeleteService deleteListService,
            ISessionContext sessionContext)
        {
            _context = context;
            _saveListService = saveListService;
            _deleteListService = deleteListService;
            _sessionContext = sessionContext;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new ListIndexViewModel
            {
                Lists = _context.Lists
                    .Include(x => x.Owner)
                    .Where(x => x.Owner.Id == _sessionContext.CurrentUser.Id)
                    .ToList()
            });
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> View(Guid id)
        {
            var list = await _context.Lists
                .Include(x => x.Owner)
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (list == null)
                return HttpNotFound();

            return View(list.AsViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateListViewModel createRequest)
        {
            if (!ModelState.IsValid)
                return View(createRequest);

            var newList = await _saveListService.CreateTodoList(createRequest.Title, _sessionContext.CurrentUser.Id);
            return RedirectToAction("View", new { id = newList.Id });
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var list = await _context.Lists.FirstOrDefaultAsync(x => x.Id == id);
            if (list == null)
                return HttpNotFound();

            return View(new TodoListViewModel
            {
                Title = list.Title,
                ListId = list.Id
            });
        }

        [HttpPost]
        public async Task<ActionResult> ExecDelete(Guid id)
        {
            try
            {
                await _deleteListService.DeleteList(id, _sessionContext.CurrentUser.Id);
                return RedirectToAction("Index");
            }
            catch (ResourceSharingException)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var list = await _context.Lists.FirstOrDefaultAsync(x => x.Id == id);
            if (list == null)
                return HttpNotFound();

            return View(list.AsViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Guid id, TodoListViewModel editRequest)
        {
            if (!ModelState.IsValid)
                return View(editRequest);

            try
            {
                await _saveListService.UpdateTodoList(id, editRequest, _sessionContext.CurrentUser.Id);
                return RedirectToAction("View", new { id });
            }
            catch (ResourceSharingException)
            {
                return View("Error");
            }
        }
    }
}