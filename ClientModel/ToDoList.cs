using System;
using System.Collections.Generic;

namespace Web.ClientModel
{
    public class ToDoList
    {
        public string _id { get; set; }
        public IEnumerable<ToDoItem> Items { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
    }

    public class ToDoItem
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public bool Complete { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}   