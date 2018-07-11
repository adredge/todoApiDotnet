using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Web.Model
{
    [BsonIgnoreExtraElements]
    public class ToDoList
    {
        [BsonId] public ObjectId Id { get; set; }

        [BsonElement("name")] public string Name { get; set; } = string.Empty;

        [BsonElement("userId")] public string UserId { get; set; }

        //[BsonElement("items")] public List<ObjectId> ItemIds { get; set; }

        [BsonElement("items")] public List<ToDoItem> Items { get; set; }

        public string ToJson()
        {
            var stream1 = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(ToDoList));
            ser.WriteObject(stream1, this);
            stream1.Position = 0;
            var sr = new StreamReader(stream1);
            return sr.ReadToEnd();
        }
    }
}