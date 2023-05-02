namespace DataSaver.ApplicationCore.ViewModels
{
    public sealed class TopicViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
