using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Todo.Data;
using Todo.Web.ViewModels;

namespace Todo.Web.Controllers
{
    [SessionAuthorize]
    public class ListController : Controller
    {
        public readonly IContext _context;

        public ListController(IContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new ListIndexViewModel
            {
                Lists = _context.Lists.ToList()
            });
        }
    }
}