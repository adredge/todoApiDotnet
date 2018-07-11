using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Web.DataModel;

namespace Web.Data
{
    public interface IRepository
    {
        ToDoListDataModel GetList(string userId, string listId);
        ToDoListDataModel AddItem(string userId, string listId, string newItemName);
        ToDoListDataModel CreateEmptyList(string userId, string listName);
        void DeleteListsForUser(string userId);
        List<ToDoListDataModel> GetLists(string userId);
        void CheckItem(string userId, string listId, string itemId, DateTime completedAt);
        void UncheckItem(string userId, string listId, string itemId);
        void RemoveItem(string userId, string listId, string itemId);
        void DeleteList(string userId, string listId);
    }
}