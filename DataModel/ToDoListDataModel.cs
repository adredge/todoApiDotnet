using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Web.DataModel
{
    [BsonIgnoreExtraElements]
    public class ToDoListDataModel
    {
        [BsonId] public ObjectId Id { get; set; }

        [BsonElement("name")] public string Name { get; set; } = string.Empty;

        [BsonElement("userId")] public string UserId { get; set; }

        [BsonElement("items")] public List<ToDoItemDataModel> Items { get; set; }
    }
}