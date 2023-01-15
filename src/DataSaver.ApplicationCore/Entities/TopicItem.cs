using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSaver.ApplicationCore.Entities
{
    public sealed class Link
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UrlLink { get; set; }
        public int TopicId { get; set; }
        public Topic? Topic { get; set; }
    }
}
