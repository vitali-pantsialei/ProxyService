using StubService.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StubService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ToDoService : IToDoService
    {
        private readonly ToDoContext context;
        private readonly ServiceReference.ToDoManagerClient manager;

        public ToDoService()
        {
            context = new ToDoContext();
            manager = new ServiceReference.ToDoManagerClient();
        }

        public List<DTO.ToDoItem> GetTodoList(int userId)
        {
            List<DTO.ToDoItem> list = manager.GetTodoList(userId).Select(i =>
            {
                return new DTO.ToDoItem()
                {
                    IsCompleted = i.IsCompleted,
                    Name = i.Name,
                    ToDoId = i.ToDoId,
                    UserId = i.UserId
                };
            }).ToList();

            foreach (var item in context.Set<DeletingId>())
            {
                if (list.FirstOrDefault(i => i.ToDoId == item.ToDoId) == null)
                {
                    context.Set<DeletingId>().Remove(item);
                    var it = context.Set<ToDoItemEF>().FirstOrDefault(i => i.ToDoId == item.ToDoId);
                    context.Set<ToDoItemEF>().Remove(it);
                }
            }

            foreach (var item in context.Set<ToDoItemEF>().Where(i => !i.IsSynchronized))
            {
                var res = list.FirstOrDefault(i => i.ToDoId == item.ToDoId);
                if (res != null && res.IsCompleted == item.IsCompleted &&
                        res.Name == item.Name)
                {
                    context.Set<ToDoItemEF>().Remove(item);
                }
            }

            foreach(var item in context.Set<ToDoItemEF>().Where(i => i.IsSynchronized))
            {
                context.Set<ToDoItemEF>().Remove(item);
            }

            list.AddRange(context.Set<ToDoItemEF>().ToList().Select(i =>
            {
                return new DTO.ToDoItem()
                {
                    IsCompleted = i.IsCompleted,
                    Name = i.Name,
                    ToDoId = i.ToDoId,
                    UserId = i.UserId
                };
            }));

            context.SaveChangesAsync();

            return list;
        }

        public void UpdateToDoItem(DTO.ToDoItem todo)
        {
            ToDoItemEF item = context.Set<ToDoItemEF>().FirstOrDefault(t => t.ToDoId == todo.ToDoId);
            if (item != null)
            {
                item.IsCompleted = todo.IsCompleted;
                item.IsSynchronized = false;
                item.Name = todo.Name;
                context.Set<ToDoItemEF>().Attach(item);
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
            CreateToDoItem(todo);

            manager.UpdateToDoItem(GetManagerToDoItem(todo));

            context.SaveChangesAsync();
        }

        public void DeleteToDoItem(int todoItemId)
        {
            DeletingId del = new DeletingId() {
                ToDoId = todoItemId
            };
            context.Set<DeletingId>().Add(del);

            manager.DeleteToDoItem(todoItemId);

            context.SaveChangesAsync();
        }

        public void CreateToDoItem(DTO.ToDoItem todo)
        {
            ToDoItemEF item = new ToDoItemEF()
            {
                Name = todo.Name,
                IsCompleted = todo.IsCompleted,
                ToDoId = todo.ToDoId,
                UserId = todo.UserId,
                IsSynchronized = false
            };

            context.Set<ToDoItemEF>().Add(item);

            manager.CreateToDoItem(GetManagerToDoItem(todo));

            context.SaveChangesAsync();
        }

        public DTO.ToDoItem GetById(int id)
        {
            var item = context.Set<ToDoItemEF>().FirstOrDefault(i => i.ToDoId == id);
            if (item != null)
                return null;
            else
                return new DTO.ToDoItem()
                {
                    IsCompleted = item.IsCompleted,
                    Name = item.Name,
                    ToDoId = item.ToDoId,
                    UserId = item.UserId
                };
        }

        public static ToDoManagerService.Entities.ToDoItem GetManagerToDoItem(DTO.ToDoItem item)
        {
            return new ToDoManagerService.Entities.ToDoItem()
            {
                IsCompleted = item.IsCompleted,
                Name = item.Name,
                ToDoId = item.ToDoId,
                UserId = item.UserId
            };
        }
    }
}
