namespace DataSaver.ApplicationCore.Entities
{
    public sealed class Topic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public DateTime DateCreated { get; set; }

        public Topic()
        {
            DateCreated = DateTime.Now;
        }

        public Topic(string name)
        {
            Name = name;

            DateCreated = DateTime.Now;
        }
    }
}
