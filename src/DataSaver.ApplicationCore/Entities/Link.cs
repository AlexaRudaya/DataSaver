﻿namespace DataSaver.ApplicationCore.Entities
{
    public sealed class Link : BaseModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        public string? UrlLink { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public int? TopicId { get; set; }

        [ForeignKey("TopicId")]
        public Topic? Topic { get; set; }

        public string? PreviewImage { get; set; }

        public string? PreviewTitle { get; set; }

        public Link()
        {
        }

        public Link(int categoryId, int topicId, string? name,
            string? previewImage, string? previewTitle,
            string? description, string? urlLink)
        {
            CategoryId = categoryId;
            TopicId = topicId;
            Name = name;
            PreviewImage = previewImage;
            PreviewTitle = previewTitle;
            Description = description;
            UrlLink = urlLink;
        }
    }
}
