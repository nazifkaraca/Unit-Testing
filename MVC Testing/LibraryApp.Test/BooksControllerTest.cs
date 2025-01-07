using Library.API.Data.Models;
using Library.API.Data.Services;
using LibraryApp.Controllers;
using LibraryApp.Data.MockData;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApp.Test
{
    public class BooksControllerTest
    {
        [Fact]
        public void IndexUnitTest()
        {
            // arrange
            var mockRepo = new Mock<IBookService>(); // Mock the IBookService dependency
            mockRepo.Setup(n => n.GetAll()).Returns(MockData.GetTestBookItems()); // Set up mock data for GetAll
            var controller = new BooksController(mockRepo.Object); // Pass the mock to the controller

            // act
            var result = controller.Index(); // Call the Index action

            // assert
            var viewResult = Assert.IsType<ViewResult>(result); // Ensure the result is a ViewResult
            var viewResultBooks = Assert.IsAssignableFrom<List<Book>>(viewResult!.ViewData.Model); // Check the model type
            Assert.Equal(5, viewResultBooks!.Count); // Verify the correct number of items
        }

        [Theory]
        [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200", "ab2bd817-98cd-4cf3-a80a-53ea0cd9c211")]
        public void DetailsUnitTest(string guid1, string guid2)
        {
            /* Valid Guid */
            // arrange
            var mockRepo = new Mock<IBookService>(); // Mock the IBookService
            var validItemGuid = new Guid(guid1); // Use a valid Guid
            mockRepo.Setup(n => n.GetById(validItemGuid))
                .Returns(MockData.GetTestBookItems().FirstOrDefault(x => x.Id == validItemGuid)); // Setup mock for valid Guid
            var controller = new BooksController(mockRepo.Object);

            // act
            var result = controller.Details(validItemGuid); // Call the Details action

            // assert
            var viewResult = Assert.IsType<ViewResult>(result); // Check for a ViewResult
            var viewResultValue = Assert.IsAssignableFrom<Book>(viewResult!.ViewData.Model); // Ensure the model is of type Book
            Assert.Equal("Managing Oneself", viewResultValue!.Title); // Validate the correct item is returned

            /* Invalid Guid */
            // arrange
            var invalidItemGuid = new Guid(guid2); // Use an invalid Guid
            mockRepo.Setup(n => n.GetById(invalidItemGuid))
                .Returns(MockData.GetTestBookItems().FirstOrDefault(x => x.Id == invalidItemGuid)); // Mock behavior for invalid Guid

            // act
            var notFoundResult = controller.Details(invalidItemGuid); // Call Details with invalid Guid

            // assert
            Assert.IsType<NotFoundResult>(notFoundResult); // Ensure a NotFoundResult is returned
        }

        [Fact]
        public void CreateTest()
        {
            /* Valid Case */
            // arrange
            var mockRepo = new Mock<IBookService>(); // Create a mock service
            var controller = new BooksController(mockRepo.Object); // Pass mock to the controller
            var newValidItem = new Book
            {
                Author = "Author", // Required field
                Title = "Title", // Required field
                Description = "Description", // Optional field
            };

            // act
            var result = controller.Create(newValidItem); // Call the Create action with valid data

            // assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result); // Check for redirection
            Assert.Equal("Index", redirectToActionResult.ActionName); // Ensure redirection to Index
            Assert.Null(redirectToActionResult.ControllerName); // Controller name should be null

            /* Not Valid Case */
            // arrange
            var newInvalidItem = new Book
            {
                Title = "Title", // Missing Author field (required)
                Description = "Description",
            };
            controller.ModelState.AddModelError("Author", "The author value is required."); // Add a model error for testing invalid case

            // act
            var resultInvalid = controller.Create(newInvalidItem); // Call Create with invalid data

            // assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultInvalid); // Check for a BadRequest result
            Assert.IsType<SerializableError>(badRequestResult.Value); // Ensure the error object is SerializableError
        }

        [Theory]
        [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200")]
        public void DeleteTest(string validGuid)
        {
            // arrange
            var mockRepo = new Mock<IBookService>(); // Mock the IBookService
            mockRepo.Setup(n => n.GetAll()).Returns(MockData.GetTestBookItems()); // Provide mock data
            var controller = new BooksController(mockRepo.Object);
            var itemGuid = new Guid(validGuid); // Use a valid Guid

            // act
            var result = controller.Delete(itemGuid, null); // Call the Delete action

            // assert
            var actionResult = Assert.IsType<RedirectToActionResult>(result); // Check for redirection
            Assert.Equal("Index", actionResult.ActionName); // Ensure redirection to Index
            Assert.Null(actionResult.ControllerName); // Controller name should be null
        }
    }
}
