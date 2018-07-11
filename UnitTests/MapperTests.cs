using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using NUnit.Framework;
using Web.DataModel;
using Web.Facade;

namespace UnitTests
{
    public class MapperTests
    {  
        private string _userId;
        private ObjectId _listId;
        private ToDoListDataModel _toDoListData;
        private Mapper _mapper;
        
        [SetUp]
        public void Setup()
        {
            _userId = Guid.NewGuid().ToString();
            _listId = ObjectId.GenerateNewId();

            _toDoListData = new ToDoListDataModel
            {
                Id = _listId,
                Name = "Test List",
                UserId = _userId,
                Items = new List<ToDoItemDataModel>
                {
                    new ToDoItemDataModel
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = "Item 1",
                        Complete = false,
                        CompletedAt = null
                    },

                    new ToDoItemDataModel
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = "Item 2",
                        Complete = true,
                        CompletedAt = DateTime.Now
                    }
                }
            };

            _mapper = new Mapper();
        }
         
        [Test]
        public void WhenMappingToDoListDataModelToClientModel()
        {
            var actual = _mapper.MapToDoList(_toDoListData);
            
            Assert.That(actual._id, Is.EqualTo(_listId.ToString()));
            Assert.That(actual.Name, Is.EqualTo(_toDoListData.Name));
            Assert.That(actual.UserId, Is.EqualTo(_toDoListData.UserId));
            
            Assert.That(actual.Items.ElementAt(0)._id, Is.EqualTo(_toDoListData.Items[0].Id.ToString()));
            Assert.That(actual.Items.ElementAt(0).Name, Is.EqualTo(_toDoListData.Items[0].Name));
            Assert.That(actual.Items.ElementAt(0).Complete, Is.EqualTo(_toDoListData.Items[0].Complete));
            Assert.That(actual.Items.ElementAt(0).CompletedAt, Is.EqualTo(_toDoListData.Items[0].CompletedAt));
            
            Assert.That(actual.Items.ElementAt(1)._id, Is.EqualTo(_toDoListData.Items[1].Id.ToString()));
            Assert.That(actual.Items.ElementAt(1).Name, Is.EqualTo(_toDoListData.Items[1].Name));
            Assert.That(actual.Items.ElementAt(1).Complete, Is.EqualTo(_toDoListData.Items[1].Complete));
            Assert.That(actual.Items.ElementAt(1).CompletedAt, Is.EqualTo(_toDoListData.Items[1].CompletedAt));
            
        }
    }
}