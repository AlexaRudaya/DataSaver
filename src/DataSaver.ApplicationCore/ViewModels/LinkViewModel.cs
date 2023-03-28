namespace DataSaver.ApplicationCore.ViewModels
{
    public sealed class LinkViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? UrlLink { get; set; }

        [Required]
        public string? Description { get; set; }

        public string? PreviewImage { get; set; }

        public string? PreviewTitle { get; set; }

        [Display(Name = "Category")] public int CategoryId { get; set; }

        public Category? Category { get; set; }

        [Display(Name = "Topic")] public int TopicId { get; set; }

        public Topic? Topic { get; set; }
    }
}
