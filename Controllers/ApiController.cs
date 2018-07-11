using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Model;
using Web.RequestModels;

namespace Web.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IRepository _repository;

        public ApiController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("/api/auth")]
        public void Authenticate(UserAuth auth)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(7);
            //option.Domain = "http://localhost:5000";
            option.SameSite = SameSiteMode.None;
            Response.Cookies.Append("user", auth.UserId, option);  
        }

        [HttpGet]
        [Route("/api/list")]
        public ToDoList Get()
        {
            var userId = Request.Cookies["user"];
            return _repository.GetList(userId, "");
        }
        
        [HttpPost]
        [Route("/api/createList")]
        public ToDoList CreateList()
        {
            var userId = Request.Cookies["user"];
            return _repository.CreateEmptyList(userId, "Default");
        }
        
        [HttpPut]
        [Route("/api/checkItem")]
        public void CheckItem(CheckToDoItem item)
        { 
            var userId = Request.Cookies["user"];
            _repository.CheckItem(userId, item.ListId, item.ItemId, item.CompletedAt);
        }
        
        [HttpPut]
        [Route("/api/uncheckItem")]
        public void UncheckItem(UpdateToDoItem item)
        { 
            var userId = Request.Cookies["user"];
            _repository.UncheckItem(userId, item.ListId, item.ItemId);
        }
        
        [HttpDelete]
        [Route("/api/removeItem/{listId}/{itemId}")]
        public void DeleteItem(string listId, string itemId)
        { 
            var userId = Request.Cookies["user"];
            _repository.RemoveItem(userId, listId, itemId);
        }

        [HttpPost]
        [Route("/api/addItem")]
        public void AddItem(NewToDoItem item)
        { 
            var userId = Request.Cookies["user"];
            _repository.AddItem(userId, item.ListId, item.NewItemName);
        }
    }
}