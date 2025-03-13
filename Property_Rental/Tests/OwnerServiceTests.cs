using NUnit.Framework;
using Moq;
using OnlineRentalPropertyManagement.Services;
using OnlineRentalPropertyManagement.Repositories;
using OnlineRentalPropertyManagement.DTOs;
using OnlineRentalPropertyManagement.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OnlineRentalPropertyManagement.Tests
{
    [TestFixture]
    public class OwnerServiceTests
    {
        private Mock<IOwnerRepository> _mockRepo;
        private Mock<ITokenService> _mockTokenService;
        private Mock<ILogger<OwnerService>> _mockLogger;
        private OwnerService _ownerService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IOwnerRepository>();
            _mockTokenService = new Mock<ITokenService>();
            _mockLogger = new Mock<ILogger<OwnerService>>();
            _ownerService = new OwnerService(_mockRepo.Object, _mockTokenService.Object, _mockLogger.Object);
        }

        [Test]
        public async Task RegisterOwnerAsync_ValidInput_ReturnsTrue()
        {
            // Arrange
            var ownerRegistrationDTO = new OwnerRegistrationDTO
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123",
                ContactDetails = "1234567890"
            };

            _mockRepo.Setup(repo => repo.AddOwnerAsync(It.IsAny<Owner>())).ReturnsAsync(true);

            // Act
            var result = await _ownerService.RegisterOwnerAsync(ownerRegistrationDTO);

            // Assert
            Assert.IsTrue(result); // This should now work
        }
    }
}