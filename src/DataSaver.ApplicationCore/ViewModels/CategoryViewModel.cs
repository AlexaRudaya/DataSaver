namespace DataSaver.ApplicationCore.ViewModels
{
    public sealed class CategoryViewModel
    {
        public Guid CategoryId { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
