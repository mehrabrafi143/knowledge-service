using knowledge_service.DTO;

namespace knowledge_service.Services
{
    public interface IKnowledgeService
    {
        Task<KnowledgeEntryDto> CreateEntryAsync(CreateKnowledgeEntryDto createDto);
        Task<KnowledgeEntryDto?> GetEntryByIdAsync(int id);
        Task<IEnumerable<KnowledgeEntryDto>> GetAllEntriesAsync();
        Task<KnowledgeEntryDto?> UpdateEntryAsync(int id, UpdateKnowledgeEntryDto updateDto);
        Task<bool> DeleteEntryAsync(int id);
        Task<IEnumerable<KnowledgeEntryDto>> SearchEntriesAsync(string keyword);
    }
}
