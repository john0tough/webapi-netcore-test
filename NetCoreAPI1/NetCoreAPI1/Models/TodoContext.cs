using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreAPI1.Models
{
    /// <summary>
    /// Todo context
    /// </summary>
    public class TodoContext: DbContext
    {
       /// <inheritdoc />
       public TodoContext(DbContextOptions<TodoContext> options): base(options)
        {

        }
        /// <summary>
        /// Todo model item
        /// </summary>
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
