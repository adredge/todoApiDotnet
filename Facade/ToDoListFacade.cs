using Web.ClientModel;
using Web.Data;
using Web.RequestModels;

namespace Web.Facade
{
    public interface IToDoListFacade
    {
        ToDoList GetList(string userId, string listId = "");
        ToDoList AddItem(string userId, NewToDoItem item);
        ToDoList CreateEmptyList(string userId);
        void CheckItem(string userId, CheckToDoItem itemToCheck);
        void UncheckItem(string userId, UpdateToDoItem itemToUncheck);
        void RemoveItem(string userId, string listId, string itemId);
    }

    public class ToDoListFacade : IToDoListFacade
    {
        private IRepository _repository;
        private IMapper _mapper;

        public ToDoListFacade(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ToDoList GetList(string userId, string listId = "")
        {
            var listData = _repository.GetList(userId, listId);
            return _mapper.MapToDoList(listData);
        }
        
        public ToDoList CreateEmptyList(string userId)
        {
            var listData = _repository.CreateEmptyList(userId, "Default");
            return _mapper.MapToDoList(listData);
        }

        public void CheckItem(string userId, CheckToDoItem itemToCheck)
        {
            _repository.CheckItem(userId, itemToCheck.ListId, itemToCheck.ItemId, itemToCheck.CompletedAt);
        }

        public ToDoList AddItem(string userId, NewToDoItem item)
        {
            var listData = _repository.AddItem(userId, item.ListId, item.NewItemName);
            return _mapper.MapToDoList(listData);
        }

        public void UncheckItem(string userId, UpdateToDoItem itemToUncheck)
        {
            _repository.UncheckItem(userId, itemToUncheck.ListId, itemToUncheck.ItemId);
        }

        public void RemoveItem(string userId, string listId, string itemId)
        {
            _repository.RemoveItem(userId, listId, itemId);
        }
    }
}