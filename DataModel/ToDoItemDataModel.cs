using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Web.DataModel
{
    [BsonIgnoreExtraElements]
    public class ToDoItemDataModel
    {
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("name")] public string Name { get; set; } = string.Empty;
        [BsonElement("complete")] public bool Complete { get; set; }
        
        [BsonElement("completedAt")] 
        public DateTime? CompletedAt { get; set; }
    }
}