namespace knowledge_service.DTO
{
    public class UpdateKnowledgeEntryDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
    }
}
