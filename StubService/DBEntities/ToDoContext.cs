using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StubService.DBEntities
{
    public class ToDoContext : DbContext
    {
        public DbSet<ToDoItemEF> ToDoItems { get; set; }
        public DbSet<DeletingId> DeletingIds { get; set; }
    }
}