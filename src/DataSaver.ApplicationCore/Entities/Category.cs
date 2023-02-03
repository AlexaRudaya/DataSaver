namespace DataSaver.ApplicationCore.Entities
{
    public sealed class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public DateTime DateCreated { get; set; }

        public Category()
        {
            DateCreated = DateTime.Now;
        }

        public Category(string name)
        {
            Name = name;

            DateCreated = DateTime.Now;
        }
    }
}
