using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSaver.ApplicationCore.ViewModels
{
    public sealed class CategoryViewModel
    {
        public Guid CategoryId { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
