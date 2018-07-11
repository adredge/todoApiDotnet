using System;
using System.Collections.Generic;
using MongoDB.Bson;
using NUnit.Framework;
using Web.ClientModel;
using Web.Data;
using Web.DataModel;
using Web.Facade;
using Web.RequestModels;

namespace UnitTests
{
    public class ToDoListFacadeTests : WithAnAutoMockedSpec<ToDoListFacade>
    {
        private string userId;
        private ObjectId listId;
        private ToDoListDataModel toDoListData;
        
        [SetUp]
        public void Setup()
        {
            userId = Guid.NewGuid().ToString();
            listId = ObjectId.GenerateNewId();

            toDoListData = new ToDoListDataModel
            {
                Id = listId,
                Name = "Test List",
                UserId = userId,
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
        }

        [Test]
        public void WhenGettingAUsersList()
        {
            ToDoList mappedList = new ToDoList();
            
            GetMock<IRepository>().Setup(x => x.GetList(Any<string>(), Any<string>())).Returns(toDoListData);
            GetMock<IMapper>().Setup(x => x.MapToDoList(Any<ToDoListDataModel>())).Returns(mappedList);
           
            var actual = classUnderTest.GetList(userId, listId.ToString());
            
            GetMock<IRepository>().Verify(x => x.GetList(userId, listId.ToString()));
            GetMock<IMapper>().Verify(x => x.MapToDoList(toDoListData));
            Assert.That(actual, Is.SameAs(mappedList));  
        }
        
        [Test]
        public void WhenCreatingAnEmptyList()
        {
            ToDoList mappedList = new ToDoList();
            
            GetMock<IRepository>().Setup(x => x.CreateEmptyList(Any<string>(), Any<string>())).Returns(toDoListData);
            GetMock<IMapper>().Setup(x => x.MapToDoList(Any<ToDoListDataModel>())).Returns(mappedList);
           
            var actual = classUnderTest.CreateEmptyList(userId);
            
            GetMock<IRepository>().Verify(x => x.CreateEmptyList(userId, "Default"));
            GetMock<IMapper>().Verify(x => x.MapToDoList(toDoListData));
            Assert.That(actual, Is.SameAs(mappedList));  
        }

        [Test]
        public void WhenAddingAnItemToAList()
        {
            ToDoList mappedList = new ToDoList();
            string itemName = "Do something awesome";
            NewToDoItem newToDoItemData = new NewToDoItem
            {
                ListId = listId.ToString(),
                NewItemName = itemName
            };
            
            GetMock<IRepository>().Setup(x => x.AddItem(Any<string>(), Any<string>(), Any<string>())).Returns(toDoListData);
            GetMock<IMapper>().Setup(x => x.MapToDoList(Any<ToDoListDataModel>())).Returns(mappedList);

            var actual = classUnderTest.AddItem(userId, newToDoItemData);
            
            GetMock<IRepository>().Verify(x => x.AddItem(userId, listId.ToString(), itemName));
            GetMock<IMapper>().Verify(x => x.MapToDoList(toDoListData));
        
            Assert.That(actual, Is.SameAs(mappedList));  
        }
        
        [Test]
        public void WhenCheckingAnItem()
        {
            var itemId = toDoListData.Items[0].Id.ToString();
            var completedAt = DateTime.Now;
            var itemToCheck = new CheckToDoItem
            {
                ItemId = itemId,
                ListId = listId.ToString(),
                CompletedAt = completedAt
            };
            
            GetMock<IRepository>()
                .Setup(x => x.CheckItem(Any<string>(), Any<string>(), Any<string>(), Any<DateTime>()));

            classUnderTest.CheckItem(userId, itemToCheck);
            
            GetMock<IRepository>().Verify(x => x.CheckItem(userId, listId.ToString(), itemId, completedAt));
        }
        
        [Test]
        public void WhenUncheckingAnItem()
        {
            var itemId = toDoListData.Items[0].Id.ToString();
            var itemToUncheck = new UpdateToDoItem
            {
                ItemId = itemId,
                ListId = listId.ToString()
            };
            
            GetMock<IRepository>()
                .Setup(x => x.UncheckItem(Any<string>(), Any<string>(), Any<string>()));

            classUnderTest.UncheckItem(userId, itemToUncheck);
            
            GetMock<IRepository>().Verify(x => x.UncheckItem(userId, listId.ToString(), itemId));
        }
        
        [Test]
        public void WhenRemovingAnItem()
        {
            var itemId = toDoListData.Items[0].Id.ToString();
            
            GetMock<IRepository>()
                .Setup(x => x.RemoveItem(Any<string>(), Any<string>(), Any<string>()));

            classUnderTest.RemoveItem(userId, listId.ToString(), itemId);
            
            GetMock<IRepository>().Verify(x => x.RemoveItem(userId, listId.ToString(), itemId));
        }
    }
}