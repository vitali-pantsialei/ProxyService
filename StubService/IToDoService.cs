using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace StubService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IToDoService
    {
        [OperationContract]
        Task<List<DTO.ToDoItem>> GetTodoList(int userId);

        [OperationContract]
        void UpdateToDoItem(DTO.ToDoItem todo);

        [OperationContract]
        void CreateToDoItem(DTO.ToDoItem todo);

        [OperationContract]
        void DeleteToDoItem(int todoItemId);
    }
}
