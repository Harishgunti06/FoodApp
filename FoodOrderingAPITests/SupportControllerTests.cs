using FoodOrderingDataAccessLayer;
using FoodOrderingDataAccessLayer.Models;
using FoodOrderingWebServices.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.Models;
using UserService.Services;

namespace SupportTesting
{
    public class SupportTesting
    {
        private Mock<IFoodRepository> mockRepository;
        private SupportController controller;

        public SupportTesting()
        {
            mockRepository = new Mock<IFoodRepository>();
            controller = new SupportController(mockRepository.Object);
        }

        [Fact]
        public void GetIssues_Success()
        {
            // Arrange
            var issues = new List<Issue>
            {
                new Issue
                {
                    IssueId = 1,
                    OrderItemId = 101,
                    Email = "user1@example.com",
                    IssueDescription = "Item was cold",
                    IssueStatus = "Open"
                },
                new Issue
                {
                    IssueId = 2,
                    OrderItemId = 102,
                    Email = "user2@example.com",
                    IssueDescription = "Wrong item delivered",
                    IssueStatus = "Open"
                }
            };

            mockRepository
                .Setup(repo => repo.GetAllIssues())
                .Returns(issues);

            // Act
            var result = controller.GetIssues();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnIssues = Assert.IsType<List<Issue>>(okResult.Value);
            Assert.Equal(2, returnIssues.Count);
        }

        [Fact]
        public void GetIssues_NotFound()
        {
            // Arrange
            mockRepository
                .Setup(repo => repo.GetAllIssues())
                .Returns(new List<Issue>());

            // Act
            var result = controller.GetIssues();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No issues found.", notFoundResult.Value);
        }

        [Fact]
        public void GetIssues_Exception()
        {
            // Arrange
            mockRepository.Setup(repo => repo.GetAllIssues()).Throws(new System.Exception("DB error"));

            // Act
            var result = controller.GetIssues();

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public void GetAllIssues_ByEmail_Success()
        {
            // Arrange
            string email = "user@example.com";
            var issues = new List<Issue> {
            new Issue { IssueId = 1, IssueDescription = "Payment failed", Email = email, OrderItemId = 1 }
        };
            mockRepository.Setup(r => r.GetAllIssuesByEmail(email)).Returns(issues);

            // Act
            var result = controller.GetAllIssues(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Issue>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public void GetAllIssues_ByEmail_ReturnsNotFound()
        {
            // Arrange
            string email = "noone@example.com";
            mockRepository.Setup(r => r.GetAllIssuesByEmail(email)).Returns(new List<Issue>());

            // Act
            var result = controller.GetAllIssues(email);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No issues found.", notFound.Value);
        }

        [Fact]
        public void GetAllIssues_Exception()
        {
            // Arrange
            mockRepository.Setup(r => r.GetAllIssues()).Throws(new Exception("DB Error"));

            // Act
            var result = controller.GetIssues();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public void UpdateIssueStatus_Success()
        {
            // Arrange
            var issueModel = new FoodOrderingWebServices.Models.Issue
            {
                IssueId = 1,
                IssueStatus = "Resolved"
            };

            mockRepository
                .Setup(r => r.UpdateIssueStatus(issueModel.IssueId, issueModel.IssueStatus))
                .Returns(true);

            // Act
            var result = controller.UpdateIssueStatus(issueModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public void UpdateIssueStatus_ReturnsNotFound()
        {
            // Arrange
            var issueModel = new FoodOrderingWebServices.Models.Issue
            {
                IssueId = 99,
                IssueStatus = "Closed"
            };

            mockRepository
                .Setup(r => r.UpdateIssueStatus(issueModel.IssueId, issueModel.IssueStatus))
                .Returns(false);

            // Act
            var result = controller.UpdateIssueStatus(issueModel);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Issue not found.", notFound.Value);
        }

        [Fact]
        public void UpdateIssueStatus_ReturnsBadRequest()
        {
            // Act
            var result = controller.UpdateIssueStatus(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid issue data.", badRequest.Value);
        }

        [Fact]
        public void UpdateIssueStatus_Exception()
        {
            // Arrange
            var issueModel = new FoodOrderingWebServices.Models.Issue
            {
                IssueId = 1,
                IssueStatus = "Open"
            };

            mockRepository
                .Setup(r => r.UpdateIssueStatus(issueModel.IssueId, issueModel.IssueStatus))
                .Throws(new Exception("Database failure"));

            // Act
            var result = controller.UpdateIssueStatus(issueModel);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("An error occurred while updating issue status", badRequest.Value.ToString());
        }

        //[Fact]
        //public void TestEmail_Success()
        //{
        //    // Arrange
        //    string email = "test@example.com";
        //    string status = "Resolved";

        //    mockEmailService
        //        .Setup(service => service.SendEmail(It.IsAny<Message>()))
        //        .Verifiable();

        //    // Act
        //    var result = controller.TestEmail(email, status);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    Assert.Equal("Test email sent successfully.", okResult.Value);
        //    mockEmailService.Verify();
        //}

        //[Fact]
        //public void TestEmail_Exception()
        //{
        //    // Arrange
        //    string email = "fail@example.com";
        //    string status = "Closed";

        //    mockEmailService
        //        .Setup(service => service.SendEmail(It.IsAny<Message>()))
        //        .Throws(new Exception("SMTP failure"));

        //    // Act
        //    var result = controller.TestEmail(email, status);

        //    // Assert
        //    var serverError = Assert.IsType<ObjectResult>(result);
        //    Assert.Equal(500, serverError.StatusCode);
        //    Assert.Contains("Email sending failed", serverError.Value.ToString());
        //}
    }
}