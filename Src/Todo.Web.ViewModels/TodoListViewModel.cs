using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Web.ViewModels
{
    public class TodoListViewModel
    {
        public Guid ListId { get; set; }
        public string Title { get; set; }
        
        [DisplayName("Created By")]
        public string Owner { get; set; }

        [DisplayName("Created On")]
        public string CreatedDate { get; set; }

        public IList<TodoListItemViewModel> Items { get; set; }
    }
}
