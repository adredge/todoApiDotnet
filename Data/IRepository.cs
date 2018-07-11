using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Web.Model;

namespace Web.Data
{
    public interface IRepository
    {
        ToDoList GetList(string userId, string listId);
        ToDoList AddItem(string userId, string listId, string newItemName);
        ToDoList CreateEmptyList(string userId, string listName);
        void DeleteListsForUser(string userId);
        List<ToDoList> GetLists(string userId);
        void CheckItem(string userId, string listId, string itemId, DateTime completedAt);
        void UncheckItem(string userId, string listId, string itemId);
        void RemoveItem(string userId, string listId, string itemId);
        void DeleteList(string userId, string listId);
    }
}