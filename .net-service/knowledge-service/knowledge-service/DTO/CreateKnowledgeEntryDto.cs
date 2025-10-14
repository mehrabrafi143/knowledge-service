namespace knowledge_service.DTO
{
    public class CreateKnowledgeEntryDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
    }
}
