namespace DataSaver.ApplicationCore.ViewModels
{
    public sealed class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
