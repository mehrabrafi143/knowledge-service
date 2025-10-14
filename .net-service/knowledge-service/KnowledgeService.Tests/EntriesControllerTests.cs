
using knowledge_service.Controllers;
using knowledge_service.DTO;
using knowledge_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace KnowledgeService.Tests
{
    public class EntriesControllerTests
    {
        private readonly Mock<IKnowledgeService> _mockService;
        private readonly Mock<ILogger<EntriesController>> _mockLogger;
        private readonly EntriesController _controller;

        public EntriesControllerTests()
        {
            _mockService = new Mock<IKnowledgeService>();
            _mockLogger = new Mock<ILogger<EntriesController>>();
            _controller = new EntriesController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllEntries_ReturnsOkResult()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllEntriesAsync())
                .ReturnsAsync(new List<KnowledgeEntryDto>
                {
                    new KnowledgeEntryDto { Id = 1, Title = "Test", Description = "Desc" }
                });

            // Act
            var result = await _controller.GetAllEntries();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var entries = Assert.IsAssignableFrom<IEnumerable<KnowledgeEntryDto>>(okResult.Value);
        }

        [Fact]
        public async Task GetEntry_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetEntryByIdAsync(1)).ReturnsAsync((KnowledgeEntryDto)null);

            var result = await _controller.GetEntry(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateEntry_ReturnsCreatedAtAction()
        {
            var createDto = new CreateKnowledgeEntryDto { Title = "New", Description = "Desc" };
            var entryDto = new KnowledgeEntryDto { Id = 1, Title = "New", Description = "Desc" };

            _mockService.Setup(s => s.CreateEntryAsync(createDto)).ReturnsAsync(entryDto);

            var result = await _controller.CreateEntry(createDto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedEntry = Assert.IsType<KnowledgeEntryDto>(createdResult.Value);
            Assert.Equal("New", returnedEntry.Title);
        }
    }
}
