using System.ComponentModel.DataAnnotations;

namespace DataSaver.ApplicationCore.ViewModels
{
    public sealed class TopicViewModel
    {
        public Guid TopicId { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
