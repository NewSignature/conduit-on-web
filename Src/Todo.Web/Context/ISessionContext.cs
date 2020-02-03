using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Entities;

namespace Todo.Web.Context
{
    public interface ISessionContext
    {
        User CurrentUser { get; set; }
    }
}
