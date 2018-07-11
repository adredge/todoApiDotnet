using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Web.Model;

namespace Web.Data
{
    public class Repository : IRepository
    {
        private readonly DatabaseConnection _db;

        public Repository() //IOptions<Settings> settings)
        {
            // _db = new DatabaseConnection(settings);
            _db = new DatabaseConnection();
        }

        public ToDoList GetList(string userId, string listId)
        {
            try
            {
                var builder = Builders<ToDoList>.Filter;
                var listFilter = builder.Eq("userId", userId);

                if (listId != null && listId != "")
                    listFilter = listFilter & builder.Eq("_id", new ObjectId(listId));

                var list = _db.Lists.FindSync(listFilter).FirstOrDefault();

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ToDoList AddItem(string userId, string listId, string newItemName)
        {
            var newItem = new ToDoItem
            {
                Id = ObjectId.GenerateNewId(),
                Name = newItemName,
                Complete = false
            };

            var listFilter = Builders<ToDoList>.Filter.Eq("userId", userId);
            var list = _db.Lists.FindSync(listFilter).FirstOrDefault();

            if (list == null) return null;
            if (list.Id != new ObjectId(listId)) throw new Exception("Unable to find the list for that user");

            list.Items.Add(newItem);

            _db.Lists
                .ReplaceOneAsync(n => n.Id.Equals(new ObjectId(listId))
                    , list
                    , new UpdateOptions {IsUpsert = true});

            return list;
        }

        public ToDoList CreateEmptyList(string userId, string listName)
        {
            var newList = new ToDoList
            {
                UserId = userId,
                Name = listName,

                Items = new List<ToDoItem>()
            };
            _db.Lists.InsertOne(newList);
            return newList;
        }

        public void DeleteListsForUser(string userId)
        {
            var listFilter = Builders<ToDoList>.Filter.Eq("userId", userId);
            _db.Lists.DeleteMany(listFilter);
        }

        public List<ToDoList> GetLists(string userId)
        {
            var listFilter = Builders<ToDoList>.Filter.Eq("userId", userId);
            return _db.Lists.FindSync(listFilter).ToList();
        }

        public void CheckItem(string userId, string listId, string itemId, DateTime completedAt)
        {
            var list = GetExistingList(userId, listId);
            var itemToUpdateIndex = GetItemIndexWithId(list, itemId);

            list.Items[itemToUpdateIndex].Complete = true;
            list.Items[itemToUpdateIndex].CompletedAt = completedAt;

            _db.Lists
                .ReplaceOneAsync(n => n.Id.Equals(new ObjectId(listId))
                    , list
                    , new UpdateOptions {IsUpsert = true});
        }

        public void UncheckItem(string userId, string listId, string itemId)
        {
            var list = GetExistingList(userId, listId);
            var itemToUpdateIndex = GetItemIndexWithId(list, itemId);

            list.Items[itemToUpdateIndex].Complete = false;
            list.Items[itemToUpdateIndex].CompletedAt = null;

            _db.Lists
                .ReplaceOneAsync(n => n.Id.Equals(new ObjectId(listId))
                    , list
                    , new UpdateOptions {IsUpsert = true});
        }

        public void RemoveItem(string userId, string listId, string itemId)
        {
            var list = GetExistingList(userId, listId);
            var itemToRemoveIndex = GetItemIndexWithId(list, itemId);

            list.Items.RemoveRange(itemToRemoveIndex, list.Items.Count - itemToRemoveIndex);

            _db.Lists
                .ReplaceOneAsync(n => n.Id.Equals(new ObjectId(listId))
                    , list
                    , new UpdateOptions {IsUpsert = true});
        }

        public void DeleteList(string userId, string listId)
        {
            var builder = Builders<ToDoList>.Filter;
            var listFilter = builder.Eq("userId", userId) & builder.Eq("_id", new ObjectId(listId));
            _db.Lists.DeleteOne(listFilter);
        }

        private ToDoList GetExistingList(string userId, string listId)
        {
            var list = GetList(userId, listId);

            if (list == null) throw new Exception("Unable to find list with id " + listId);
            return list;
        }

        private int GetItemIndexWithId(ToDoList list, string itemId)
        {
            var itemToUpdateIndex = list.Items.FindIndex(x => x.Id == new ObjectId(itemId));

            if (itemToUpdateIndex < 0) throw new Exception("Unable to find item with id " + itemId);
            return itemToUpdateIndex;
        }
    }
}