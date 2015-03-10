using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ToDoItem
    {
        public int ToDoId { get; set; }
        public int UserId { get; set; }
        public bool IsCompleted { get; set; }
        public string Name { get; set; }
    }
}
