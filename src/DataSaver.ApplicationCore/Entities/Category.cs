namespace DataSaver.ApplicationCore.Entities
{
    public sealed class Category : BaseModel
    {

        [Required]
        public string? Name { get; set; }

        public DateTime DateCreated { get; set; }

        public Category()
        {
        }

        public Category(string name)
        {
            Name = name;
        }
    }
}
