using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Model;

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

        [HttpGet]
        [Route("/api/list")]
        public ToDoList Get()
        {
            var userId = Request.Cookies["user"];
            return _repository.GetList(userId, "");
        }

        [HttpPost]
        [Route("/api/addItem")]
        public void Post(NewToDoItem item)
        {
            //TO DO: Get user cookie
            var userId = Request.Cookies["user"];
            _repository.AddItem(userId, item.ListId, item.NewItemName);
        }
    }

    public class NewToDoItem
    {
        public string ListId { get; set; }
        public string NewItemName { get; set; }
    }
}