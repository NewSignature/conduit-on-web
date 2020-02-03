using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Data.Entities
{
    public class TodoListItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string ItemName { get; set; }
        public string Description { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime? CompletedOn { get; set; }

        public virtual TodoList List { get; set; }

        [ForeignKey(nameof(List))]
        [Column("ListId")]
        public Guid ParentListId { get; set; }
    }
}
