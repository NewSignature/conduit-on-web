using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Data.Entities
{
    public class TodoList
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public virtual IList<TodoListItem> Items { get; set; }

        public virtual User Owner { get; set; }

        [ForeignKey(nameof(Owner))]
        public Guid OwnerId { get; set; }
    }
}
