using System;
using Web.Controllers;

namespace Web.RequestModels
{
    public class CheckToDoItem : UpdateToDoItem
    {
        public DateTime CompletedAt { get; set; }
    }
}