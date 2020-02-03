using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Todo.Common;
using Todo.Data;
using Todo.Services;
using Todo.Web.Context;
using Todo.Web.ViewModels;
using Todo.Web.ViewModels.Extensions;

namespace Todo.Web.Controllers
{
    [SessionAuthorize]
    public class ListItemController : Controller
    {
        private readonly IContext _context;
        private readonly ISaveListItemService _saveListItemService;
        private readonly IDeleteService _deleteService;
        private readonly ISessionContext _sessionContext;

        public ListItemController(ISaveListItemService saveListItemService, IContext context, IDeleteService deleteService,
            ISessionContext sessionContext)
        {
            _saveListItemService = saveListItemService;
            _context = context;
            _deleteService = deleteService;
            _sessionContext = sessionContext;
        }

        [HttpGet]
        public ActionResult Create(Guid listId)
        {
            return View(new CreateListItemViewModel
            {
                ListId = listId
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create(Guid listId, CreateListItemViewModel createRequest)
        {
            if (!ModelState.IsValid)
                return View(createRequest);

            await _saveListItemService.CreateListItem(listId,
                createRequest, _sessionContext.CurrentUser.Id);

            return RedirectToAction("View", "List", new { id = listId });
        }

        [HttpGet]
        public async Task<ActionResult> View(Guid id)
        {
            var listItem = await _context.ListItems.FirstOrDefaultAsync(x => x.Id == id);
            if (listItem == null)
                return HttpNotFound();

            return View(listItem.AsViewModel());
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            var listItem = await _context.ListItems.FirstOrDefaultAsync(x => x.Id == id);
            if (listItem == null)
                return HttpNotFound();

            return View(listItem.AsViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> ExecDelete(Guid id)
        {
            try
            {
                var listItemDeleted = await _deleteService.DeleteListItem(id, _sessionContext.CurrentUser.Id);
                return RedirectToAction("View", "List", new { id = listItemDeleted.ParentListId });
            }
            catch (ResourceSharingException)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var listItem = await _context.ListItems.FirstOrDefaultAsync(x => x.Id == id);
            if (listItem == null)
                return HttpNotFound();

            return View(listItem.AsEditViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Guid id, EditTodoListItemViewModel editRequest)
        {
            try
            {
                var itemEdited = await _saveListItemService.UpdateListItem(id, editRequest, _sessionContext.CurrentUser.Id);

                return RedirectToAction("View", new { id });
            }
            catch (ResourceSharingException)
            {
                return View("Error");
            }
        }
    }
}