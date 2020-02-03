using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Web.ViewModels
{
    public class CreateListItemViewModel
    {
        [Required(ErrorMessage = "Item Name is required")]
        public string ItemName { get; set; }
        public string Description { get; set; }
        public Guid ListId { get; set; }
    }
}
