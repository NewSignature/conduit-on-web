using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Data.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(100), Required]
        public string Username { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required, MaxLength(200)]
        public string Password { get; set; }

        [MaxLength(200), Required]
        public string FirstName { get; set; }

        [MaxLength(200), Required]
        public string LastName { get; set; }

        public virtual IList<TodoList> Lists { get; set; }
    }
}
