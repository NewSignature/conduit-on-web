using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Web.ViewModels
{
    public class TodoListItemViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string AddedOn { get; set; }
        public string CompletedOn { get; set; }
        public Guid ListId { get; set; }
    }
}
