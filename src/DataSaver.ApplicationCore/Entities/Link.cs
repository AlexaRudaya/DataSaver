using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataSaver.ApplicationCore.Entities
{
    public sealed class Link
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        public string? UrlLink { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public int TopicId { get; set; }

        [ForeignKey("TopicId")]
        public Topic? Topic { get; set; }

        public Link()
        {
            DateCreated = DateTime.Now;
        }

        public Link(int categoryId, int topicId, string? name, 
            string? description, string? urlLink)
        {
            DateCreated = DateTime.Now;
            Name = name;
            Description = description;
            UrlLink = urlLink;
            CategoryId = categoryId;
            TopicId = topicId;
        }
    }
}
