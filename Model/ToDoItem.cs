using System;
using System.IO;
using System.Runtime.Serialization.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Web.Model
{
    [BsonIgnoreExtraElements]
    public class ToDoItem
    {
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("name")] public string Name { get; set; } = string.Empty;
        [BsonElement("complete")] public bool Complete { get; set; }
        
        [BsonElement("completedAt")] 
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CompletedAt { get; set; } = null;

        public string ToJson()
        {
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ToDoItem));
            ser.WriteObject(stream1, this);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            return sr.ReadToEnd();
        }
    }
}