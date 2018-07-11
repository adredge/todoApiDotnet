using System.Linq;
using Web.ClientModel;
using Web.DataModel;

namespace Web.Facade
{
    public interface IMapper
    {
        ToDoList MapToDoList(ToDoListDataModel listData);
    }

    public class Mapper : IMapper
    {
        public ToDoList MapToDoList(ToDoListDataModel listData)
        {
            return new ToDoList
            {
                _id = listData.Id.ToString(),
                Items = listData.Items.Select(x => MapToDoItem(x)),
                Name = listData.Name,
                UserId = listData.UserId
            };
        }

        private ToDoItem MapToDoItem(ToDoItemDataModel itemData)
        {
            return new ToDoItem
            {
                _id = itemData.Id.ToString(),
                Name = itemData.Name,
                Complete = itemData.Complete,
                CompletedAt = itemData.CompletedAt
            };
        }
    }
}