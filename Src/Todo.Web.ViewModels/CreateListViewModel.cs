using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Web.ViewModels
{
    public class CreateListViewModel
    {
        [Required(ErrorMessage = "Title is a required field")]
        [MaxLength(100, ErrorMessage = "Title Length cannot exceed 100 characters")]
        public string Title { get; set; }
    }
}
