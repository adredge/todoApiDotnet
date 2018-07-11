using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.ClientModel;
using Web.Facade;
using Web.RequestModels;

namespace Web.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IToDoListFacade _facade;

        public ApiController(IToDoListFacade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        [Route("/api/auth")]
        public void Authenticate(UserAuth auth)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(7);
            option.SameSite = SameSiteMode.None;
            Response.Cookies.Append("user", auth.UserId, option);  
        }

        [HttpGet]
        [Route("/api/list")]
        public ToDoList GetList()
        {
            var userId = Request.Cookies["user"];
            return _facade.GetList(userId, "");
        }
        
        [HttpPost]
        [Route("/api/createList")]
        public ToDoList CreateList()
        {
            var userId = Request.Cookies["user"];   
            return _facade.CreateEmptyList(userId);
        }
        
        [HttpPut]
        [Route("/api/checkItem")]
        public void CheckItem(CheckToDoItem item)
        { 
            var userId = Request.Cookies["user"];
            _facade.CheckItem(userId, item);
        }
        
        [HttpPut]
        [Route("/api/uncheckItem")]
        public void UncheckItem(UpdateToDoItem item)
        { 
            var userId = Request.Cookies["user"];
            _facade.UncheckItem(userId, item);
        }

        [HttpPost]
        [Route("/api/addItem")]
        public ToDoList AddItem(NewToDoItem item)
        { 
            var userId = Request.Cookies["user"];
            return _facade.AddItem(userId, item);
        }
        
        [HttpDelete]
        [Route("/api/removeItem/{listId}/{itemId}")]
        public void DeleteItem(string listId, string itemId)
        { 
            var userId = Request.Cookies["user"];
            _facade.RemoveItem(userId, listId, itemId);
        }
    }
}