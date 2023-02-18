namespace DataSaver.ApplicationCore.ViewModels
{
    public sealed class FilterViewModel
    {
        public IEnumerable<LinkViewModel>? Links { get; set; }

        public IEnumerable<SelectListItem>? Categories { get; set; }

        public IEnumerable<SelectListItem>? Topics { get; set; }

        public int? CategoryId { get; set; }

        public int? TopicId { get; set; }

        public FilterViewModel()
        {

        }
    }
}
