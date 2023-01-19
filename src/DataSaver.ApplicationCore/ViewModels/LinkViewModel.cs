using DataSaver.ApplicationCore.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataSaver.ApplicationCore.ViewModels
{
    public sealed class LinkViewModel
    {
        public Guid LinkId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? UrlLink { get; set; }

        [Required]
        public string? Description { get; set; }

        [Display(Name = "Category")] public int CategoryId { get; set; }

        public Category? Category { get; set; }

        [Display(Name = "Topic")] public int TopicId { get; set; }

        public Topic? Topic { get; set; }
    }
}
