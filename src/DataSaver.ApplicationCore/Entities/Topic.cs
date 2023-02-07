namespace DataSaver.ApplicationCore.Entities
{
    public sealed class Topic : BaseModel
    {
        [Required]
        public string? Name { get; set; }

        public DateTime DateCreated { get; set; }

        public Topic()
        {
        }

        public Topic(string name)
        {
            Name = name;
        }
    }
}
