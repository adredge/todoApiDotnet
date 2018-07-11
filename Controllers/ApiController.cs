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
            return _repository.GetList("amy-dredge", "");
        }

        [HttpPost]
        [Route("/api/addItem")]
        public void Post(NewToDoItem item)
        {
            //TO DO: Get user cookie
            _repository.AddItem("amy-dredge", item.ListId, item.NewItemName);
        }
    }

    public class NewToDoItem
    {
        public string ListId { get; set; }
        public string NewItemName { get; set; }
    }
}