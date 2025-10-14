using knowledge_service.Data;
using knowledge_service.DTO;
using knowledge_service.Models;
using Microsoft.EntityFrameworkCore;

namespace knowledge_service.Services
{

    public class KnowledgeService : IKnowledgeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<KnowledgeService> _logger;

        public KnowledgeService(ApplicationDbContext context, ILogger<KnowledgeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<KnowledgeEntryDto> CreateEntryAsync(CreateKnowledgeEntryDto createDto)
        {
            _logger.LogInformation("Creating new knowledge entry: {Title}", createDto.Title);

            var entry = new KnowledgeEntry
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Tags = createDto.Tags,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.KnowledgeEntries.Add(entry);
            await _context.SaveChangesAsync();

            return MapToDto(entry);
        }

        public async Task<KnowledgeEntryDto?> GetEntryByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving knowledge entry with ID: {Id}", id);

            var entry = await _context.KnowledgeEntries.FindAsync(id);
            return entry == null ? null : MapToDto(entry);
        }

        public async Task<IEnumerable<KnowledgeEntryDto>> GetAllEntriesAsync()
        {
            _logger.LogInformation("Retrieving all knowledge entries");

            var entries = await _context.KnowledgeEntries
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            return entries.Select(MapToDto);
        }

        public async Task<KnowledgeEntryDto?> UpdateEntryAsync(int id, UpdateKnowledgeEntryDto updateDto)
        {
            _logger.LogInformation("Updating knowledge entry with ID: {Id}", id);

            var entry = await _context.KnowledgeEntries.FindAsync(id);
            if (entry == null) return null;

            entry.Title = updateDto.Title;
            entry.Description = updateDto.Description;
            entry.Tags = updateDto.Tags;
            entry.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDto(entry);
        }

        public async Task<bool> DeleteEntryAsync(int id)
        {
            _logger.LogInformation("Deleting knowledge entry with ID: {Id}", id);

            var entry = await _context.KnowledgeEntries.FindAsync(id);
            if (entry == null) return false;

            _context.KnowledgeEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<KnowledgeEntryDto>> SearchEntriesAsync(string keyword)
        {
            _logger.LogInformation("Searching knowledge entries with keyword: {Keyword}", keyword);

            var entries = await _context.KnowledgeEntries
                .Where(e => e.Title.Contains(keyword) ||
                           e.Description.Contains(keyword) ||
                           e.Tags.Any(tag => tag.Contains(keyword)))
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            return entries.Select(MapToDto);
        }

        private static KnowledgeEntryDto MapToDto(KnowledgeEntry entry) => new()
        {
            Id = entry.Id,
            Title = entry.Title,
            Description = entry.Description,
            Tags = entry.Tags,
            CreatedAt = entry.CreatedAt,
            UpdatedAt = entry.UpdatedAt
        };
    }
}
