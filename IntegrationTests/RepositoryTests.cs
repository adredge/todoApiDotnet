using System;
using System.Net.Mime;
using NUnit.Framework;
using Web.Data;
using Web.Model;

namespace IntegrationTests
{
    public class RepositoryTest
    {
        protected IRepository repository;
        protected string userId;

        [OneTimeSetUp]
        public void SetUp()
        {
            repository = new Repository();
            userId = Guid.NewGuid().ToString();
        }

        [TearDown]
        public void TearDown()
        {
            repository.DeleteListsForUser(userId);
            Assert.That(repository.GetLists(userId).Count, Is.EqualTo(0));
        }
    }

    public class RepositoryTests_SimpleList : RepositoryTest
    {
        [Test]
        public void CreatingAndGettingAnEmptyList_returns_an_empty_list()
        {
            var createdList = repository.CreateEmptyList(userId, "Default");
            var endList = repository.GetList(userId, createdList.Id.ToString());

            Assert.That(createdList.Name, Is.EqualTo("Default"));
            Assert.That(createdList.Items, Is.Empty);
            Assert.That(createdList.Id, Is.Not.Null);

            Assert.That(endList.Id, Is.EqualTo(createdList.Id));
            Assert.That(endList.Name, Is.EqualTo(createdList.Name));
            Assert.That(endList.Items, Is.Empty);
        }
        
        [Test]
        public void CreatingAndGettingAnEmptyListWithoutPassingInId_returns_the_empty_list()
        {
            var createdList = repository.CreateEmptyList(userId, "Default");
            var endList = repository.GetList(userId, "");

            Assert.That(endList.Id, Is.EqualTo(createdList.Id));
            Assert.That(endList.Name, Is.EqualTo(createdList.Name));
            Assert.That(endList.Items, Is.Empty);
        }
        
        [Test]
        public void AddingOneItemToAList_saves_the_incomplete_item()
        {
            var newItemName = "New Item";

            var createdList = repository.CreateEmptyList(userId, "My Test List");
            var listWithItem = repository.AddItem(userId, createdList.Id.ToString(), newItemName);
            var endList = repository.GetList(userId, listWithItem.Id.ToString());

            Assert.That(listWithItem.Id, Is.EqualTo(createdList.Id));
            Assert.That(listWithItem.Items.Count, Is.EqualTo(1));
            Assert.That(listWithItem.Items[0].Name, Is.EqualTo(newItemName));
            Assert.That(listWithItem.Items[0].Id, Is.Not.Null);
            Assert.That(listWithItem.Items[0].Complete, Is.False);
            Assert.That(listWithItem.Items[0].CompletedAt, Is.Null);

            Assert.That(endList.Items.Count, Is.EqualTo(1));
        }
    }
    
    public class RepositoryTests_AddingOneItem : RepositoryTest
    {
        [Test]
        public void AddingOneItemToAList_saves_the_incomplete_item()
        {
            var newItemName = "New Item";

            var createdList = repository.CreateEmptyList(userId, "My Test List");
            var listWithItem = repository.AddItem(userId, createdList.Id.ToString(), newItemName);
            var endList = repository.GetList(userId, listWithItem.Id.ToString());

            Assert.That(listWithItem.Id, Is.EqualTo(createdList.Id));
            Assert.That(listWithItem.Items.Count, Is.EqualTo(1));
            Assert.That(listWithItem.Items[0].Name, Is.EqualTo(newItemName));
            Assert.That(listWithItem.Items[0].Id, Is.Not.Null);
            Assert.That(listWithItem.Items[0].Complete, Is.False);
            Assert.That(listWithItem.Items[0].CompletedAt, Is.Null);

            Assert.That(endList.Items.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void AddingOneItemToAListThatDoesNOTExist_does_NOT_save_anything()
        {
            var nonexistantListId = Guid.NewGuid().ToString();
            
            var listWithItem = repository.AddItem(userId, nonexistantListId, "New Item");
            var endList = repository.GetList(userId, nonexistantListId);

            Assert.That(listWithItem, Is.Null);
            Assert.That(endList, Is.Null);
        }
    }

    public class RepositoryTests_AddingMultipleItems : RepositoryTest
    {
        private ToDoList createdList;
        private ToDoList listWithThreeItems;
        private ToDoList endList;
        
        private string newItemName1 = "New Item 1";
        private string newItemName2 = "New Item 2";
        private string newItemName3 = "New Item 3";
        
        [OneTimeSetUp]
        public void SaveMultipleItems()
        {
            createdList = repository.CreateEmptyList(userId, "My Test List");
            repository.AddItem(userId, createdList.Id.ToString(), newItemName1);
            repository.AddItem(userId, createdList.Id.ToString(), newItemName2);
            listWithThreeItems = repository.AddItem(userId, createdList.Id.ToString(), newItemName3);
            endList = repository.GetList(userId, listWithThreeItems.Id.ToString());
        }
        
        [Test]
        public void AddingMultipleItemsToAList_saves_all_items()
        {
            Assert.That(listWithThreeItems.Id, Is.EqualTo(createdList.Id));
            Assert.That(listWithThreeItems.Items.Count, Is.EqualTo(3));
           
            Assert.That(endList.Items.Count, Is.EqualTo(3));
        }
        
        [Test]
        public void AddingMultipleItemsToAList_saves_all_items_with_different_ids()
        {   
            Assert.That(endList.Items[0].Id, Is.Not.EqualTo(endList.Items[1].Id));
            Assert.That(endList.Items[1].Id, Is.Not.EqualTo(endList.Items[2].Id));
            Assert.That(endList.Items[0].Id, Is.Not.EqualTo(endList.Items[2].Id));   
        }
        
        [Test]
        public void AddingMultipleItemsToAList_saves_the_items_as_incomplete()
        {
            Assert.That(listWithThreeItems.Items[0].Complete, Is.False);
            Assert.That(listWithThreeItems.Items[1].Complete, Is.False);
            Assert.That(listWithThreeItems.Items[2].Complete, Is.False);
        }
        
        [Test]
        public void AddingMultipleItemsToAList_preserves_the_order_of_the_items()
        {
            Assert.That(endList.Items[0].Name, Is.EqualTo(newItemName1));
            Assert.That(endList.Items[1].Name, Is.EqualTo(newItemName2));
            Assert.That(endList.Items[2].Name, Is.EqualTo(newItemName3));
        }
  
}

    public class RepositoryTests_EdgeCases : RepositoryTest
    {   
        private static readonly string otherUserId = Guid.NewGuid().ToString();
        
        [Test]
        public void AddingItemsToAListTheUserDoesNOTOwn_throw_an_error()
        {
            var createdListForMainUser = repository.CreateEmptyList(userId, "Default");
            repository.CreateEmptyList(otherUserId, "Default");
            var ex = Assert.Throws<Exception>(() => repository.AddItem(otherUserId, createdListForMainUser.Id.ToString(), "My Item"));
            Assert.That(ex.Message, Contains.Substring("Unable to find the list for that user"));
        }

        [TearDown]
        public void DeleteExtraList()
        {
            repository.DeleteListsForUser(otherUserId);
        }
    }
    
    public class RepositoryTests_CheckingAnItem : RepositoryTest
    {
        [Test]
        public void CheckingAnItemAsComplete_should_save_with_date()
        {
            var itemName = "My Item";
            var createdList = repository.CreateEmptyList(userId, "My Test List");
            var listWithItem = repository.AddItem(userId, createdList.Id.ToString(), itemName);
            
            var itemId = listWithItem.Items[0].Id;
            var expectedCompletedAt = DateTime.Now.ToUniversalTime();//.ToLocalTime();
            repository.CheckItem(userId, createdList.Id.ToString(), itemId.ToString(), expectedCompletedAt);
            
            var endList = repository.GetList(userId, listWithItem.Id.ToString());
            
            Assert.That(endList.Items[0].Complete, Is.True);
            Assert.That(endList.Items[0].CompletedAt, Is.EqualTo(expectedCompletedAt).Within(TimeSpan.FromMilliseconds(1)));
        }
        
        [Test]
        public void CheckingAnItemThatDoesNOTExistAsComplete_should_throw_an_error()
        {
            var createdList = repository.CreateEmptyList(userId, "My Test List");
            var nonexistantItemId = Guid.NewGuid().ToString();
            var ex = Assert.Throws<Exception>(() => repository.CheckItem(userId, createdList.Id.ToString(), nonexistantItemId, DateTime.Now));
            Assert.That(ex.Message, Contains.Substring("Unable to find item with id " + nonexistantItemId));    
        }
        
        [Test]
        public void CheckingAnItemForAListThatDoesNOTExist_should_throw_an_error()
        {  
            var nonexistantListId = Guid.NewGuid().ToString();
            var ex = Assert.Throws<Exception>(() => repository.CheckItem(userId, nonexistantListId, Guid.NewGuid().ToString(), DateTime.Now));
            Assert.That(ex.Message, Contains.Substring("Unable to find list with id " + nonexistantListId));    
        }
    }
    
    public class RepositoryTests_UNCheckingAnItem : RepositoryTest
    {
        [Test]
        public void UNCheckingAnItemAsComplete_should_save()
        {
            var createdList = repository.CreateEmptyList(userId, "My Test List");
            var listId = createdList.Id.ToString();
            var listWithItem = repository.AddItem(userId, listId, "My Item");
            
            var itemId = listWithItem.Items[0].Id;
            repository.CheckItem(userId, listId, itemId.ToString(), DateTime.Now.ToUniversalTime());
            repository.UncheckItem(userId, listId, itemId.ToString());
            
            var savedList = repository.GetList(userId, listId);

            Assert.That(savedList.Items[0].Complete, Is.False);
            Assert.That(savedList.Items[0].CompletedAt, Is.Null);
        }
        
        [Test]
        public void UNCheckingAnItemThatDoesNOTExist_should_throw_an_error()
        {      
            var createdList = repository.CreateEmptyList(userId, "My Test List");
            var nonexistantItemId = Guid.NewGuid().ToString();
            var ex = Assert.Throws<Exception>(() => repository.UncheckItem(userId, createdList.Id.ToString(), nonexistantItemId));
            Assert.That(ex.Message, Contains.Substring("Unable to find item with id " + nonexistantItemId));    
        }
        
        [Test]
        public void UNCheckingAnItemForAListThatDoesNOTExist_should_throw_an_error()
        {  
            var nonexistantListId = Guid.NewGuid().ToString();
            var ex = Assert.Throws<Exception>(() => repository.UncheckItem(userId, nonexistantListId, Guid.NewGuid().ToString()));
            Assert.That(ex.Message, Contains.Substring("Unable to find list with id " + nonexistantListId));    
        }
    }
    
    public class RepositoryTests_RemovingAnItem : RepositoryTest
    {
        [Test]
        public void RemovingAnItem_fully_deletes_the_item()
        {
            var createdList = repository.CreateEmptyList(userId, "My Test List");
            var listId = createdList.Id.ToString();
            repository.AddItem(userId, listId, "My Item");
            repository.AddItem(userId, listId, "My Item 2");
            var listWithThreeItems = repository.AddItem(userId, listId, "My Item 3");          
            
            var itemIdToRemove = listWithThreeItems.Items[1].Id;
            repository.RemoveItem(userId, listId, itemIdToRemove.ToString());
            
            var endList = repository.GetList(userId, listId);

            Assert.That(endList.Items.Count, Is.EqualTo(2));
            Assert.That(endList.Items.FindIndex(x => x.Id == itemIdToRemove), Is.EqualTo(-1));
            Assert.That(endList.Items[0].Name, Is.EqualTo("My Item"));
            Assert.That(endList.Items[1].Name, Is.EqualTo("My Item 3"));
        }
        
        [Test]
        public void RemovingAnItemThatDoesNOTExist_should_throw_an_error()
        {      
            var createdList = repository.CreateEmptyList(userId, "My Test List");
            var nonexistantItemId = Guid.NewGuid().ToString();
            var ex = Assert.Throws<Exception>(() => repository.RemoveItem(userId, createdList.Id.ToString(), nonexistantItemId));
            Assert.That(ex.Message, Contains.Substring("Unable to find item with id " + nonexistantItemId));    
        }
        
        [Test]
        public void RemovingAnItemFromAListThatDoesNOTExist_should_throw_an_error()
        {  
            var nonexistantListId = Guid.NewGuid().ToString();
            var ex = Assert.Throws<Exception>(() => repository.RemoveItem(userId, nonexistantListId, Guid.NewGuid().ToString()));
            Assert.That(ex.Message, Contains.Substring("Unable to find list with id " + nonexistantListId));    
        }
    }

    public class RepositoryTests_RemovingAList : RepositoryTest
    {
        [Test]
        public void RemovingAList_fully_deletes_the_list()
        {
            var createdList = repository.CreateEmptyList(userId, "My Test List");
            var listId = createdList.Id.ToString();
            repository.DeleteList(userId, listId);
            var endList = repository.GetList(userId, listId);

            Assert.That(endList, Is.Null);
        }
    }
}
//
//  context('when removing a list', () => {
//    let listId, list
//
//    beforeEach(() => {
//      return toDoListRepository.createEmptyList(userId, "List To Delete")
//        .then(l => listId = l._id)
//        .then(() => toDoListRepository.deleteList(userId, listId))
//        .then(() => toDoListRepository.getList(userId))
//        .then(l => list = l)
//    })
//
//    it('should delete the list', () => {
//      expect(list).to.be.null
//    })
//  })
//})