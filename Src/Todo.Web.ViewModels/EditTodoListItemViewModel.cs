using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Web.ViewModels
{
    public class EditTodoListItemViewModel : TodoListItemViewModel
    {
        public EditTodoListItemViewModel()
        {
            // parameterless constructor
        }

        internal EditTodoListItemViewModel(TodoListItemViewModel viewModel)
        {
            this.AddedOn = viewModel.AddedOn;
            this.CompletedOn = viewModel.CompletedOn;
            this.Description = viewModel.Description;
            this.Id = viewModel.Id;
            this.ListId = viewModel.ListId;
            this.Name = viewModel.Name;
        }

        public bool IsComplete { get; set; }
    }
}
