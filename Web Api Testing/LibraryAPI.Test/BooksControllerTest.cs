using Library.API.Controllers;
using Library.API.Data.Models;
using Library.API.Data.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryAPI.Test
{
    public class BooksControllerTest
    {
        private readonly BooksController _controller;
        private readonly IBookService _service;

        public BooksControllerTest()
        {
            // Mock service oluþturuluyor
            _service = new BookService();

            // Controller, mock service ile oluþturuluyor
            _controller = new BooksController(_service);
        }

        [Fact]
        public void GetAllTest()
        {
            //arrange

            //act
            var result = _controller.Get();

            //assert
            Assert.IsType<OkObjectResult>(result.Result); // Check if the result 200 type
            var list = result.Result as OkObjectResult;

            Assert.IsType<List<Book>>(list!.Value); // Check if the type of booklist is correct
            var listBooks = list.Value as List<Book>;

            Assert.Equal(5, listBooks!.Count); // Check if are there 5 books in the list
        }

        [Theory]
        [InlineData("d81e0829-55fa-4c37-b62f-f578c692af78", "d81e0829-55fa-4c37-b62f-f578c692a111")]
        public void Get(string guid1, string guid2)
        {
            // arrange
            var validGuid = new Guid(guid1);
            var invalidGuid = new Guid(guid2);

            // act
            var notFoundResult = _controller.Get(invalidGuid); // Invalid guid Http response assignment.

            var okResult = _controller.Get(validGuid); // Valid guid Http response assignment.

            // assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result); // Invalid guid should result NotFound.

            Assert.IsType<OkObjectResult>(okResult.Result); // Valid guid should result Ok.

            var item = okResult.Result as OkObjectResult; // To get the book, we convert from ActionResult to OkObjectResult.
            
            Assert.IsType<Book>(item!.Value); // Check if the item (book) is in the type of Book class.

            var bookItem = item.Value as Book; // 

            Assert.Equal(validGuid, bookItem!.Id);
            Assert.Equal("The Lessons of History", bookItem.Title);
        }

        [Fact]
        public void AddBookTest()
        {
            // arrange
            var completeBook = new Book()
            {
                Author = "Author",
                Title = "Title",
                Description = "Description",
            };

            // act
            var createdResponse = _controller.Post(completeBook);

            // assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);

            var item = createdResponse as CreatedAtActionResult;
            Assert.IsType<Book>(item!.Value);

            var bookItem = item.Value as Book;
            Assert.Equal(completeBook.Author, bookItem!.Author);
            Assert.Equal(completeBook.Title, bookItem!.Title);
            Assert.Equal(completeBook.Description, bookItem!.Description);


            // arrange
            var incompleteBook = new Book()
            {
                Title = "Title",
                Description = "Description",
            };

            // act
            _controller.ModelState.AddModelError("Title", "Title is a required field.");
            var badResponse = _controller.Post(incompleteBook);

            // assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Theory]
        [InlineData("d81e0829-55fa-4c37-b62f-f578c692af78", "d81e0829-55fa-4c37-b62f-f578c692a111")]
        public void RemoveBookIdTest(string guid1, string guid2)
        {
            // arrange
            var validGuid = new Guid(guid1);
            var invalidGuid = new Guid(guid2);

            // act
            var notFoundResult = _controller.Remove(invalidGuid); // Invalid guid Http response assignment.
            // assert
            Assert.IsType<NotFoundResult>(notFoundResult); 
            Assert.Equal(5, _service.GetAll().Count());

            // act
            var okResult = _controller.Remove(validGuid); // Valid guid Http response assignment.
            // assert
            Assert.IsType<OkResult>(okResult);
            Assert.Equal(4, _service.GetAll().Count());
        }

    }
}