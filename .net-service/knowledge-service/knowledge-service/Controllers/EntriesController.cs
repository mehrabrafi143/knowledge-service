using knowledge_service.DTO;
using knowledge_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace knowledge_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntriesController : ControllerBase
    {
        private readonly IKnowledgeService _knowledgeService;
        private readonly ILogger<EntriesController> _logger;

        public EntriesController(IKnowledgeService knowledgeService, ILogger<EntriesController> logger)
        {
            _knowledgeService = knowledgeService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<KnowledgeEntryDto>> CreateEntry(CreateKnowledgeEntryDto createDto)
        {
            try
            {
                var entry = await _knowledgeService.CreateEntryAsync(createDto);
                return CreatedAtAction(nameof(GetEntry), new { id = entry.Id }, entry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating knowledge entry");
                return StatusCode(500, "An error occurred while creating the entry");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KnowledgeEntryDto>> GetEntry(int id)
        {
            try
            {
                var entry = await _knowledgeService.GetEntryByIdAsync(id);
                return entry == null ? NotFound() : Ok(entry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving knowledge entry with ID: {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the entry");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KnowledgeEntryDto>>> GetAllEntries()
        {
            try
            {
                var entries = await _knowledgeService.GetAllEntriesAsync();
                return Ok(entries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all knowledge entries");
                return StatusCode(500, "An error occurred while retrieving entries");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<KnowledgeEntryDto>> UpdateEntry(int id, UpdateKnowledgeEntryDto updateDto)
        {
            try
            {
                var entry = await _knowledgeService.UpdateEntryAsync(id, updateDto);
                return entry == null ? NotFound() : Ok(entry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating knowledge entry with ID: {Id}", id);
                return StatusCode(500, "An error occurred while updating the entry");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEntry(int id)
        {
            try
            {
                var deleted = await _knowledgeService.DeleteEntryAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting knowledge entry with ID: {Id}", id);
                return StatusCode(500, "An error occurred while deleting the entry");
            }
        }
    }
}
