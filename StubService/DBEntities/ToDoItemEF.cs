using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StubService.DBEntities
{
    public class ToDoItemEF
    {
        public int ToDoId { get; set; }

        public int UserId { get; set; }

        public bool IsCompleted { get; set; }

        public string Name { get; set; }

        public bool IsSynchronized { get; set; }
    }
}