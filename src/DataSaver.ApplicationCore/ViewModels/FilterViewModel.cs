namespace DataSaver.ApplicationCore.ViewModels
{
    public sealed class FilterViewModel
    {
        public IEnumerable<LinkViewModel>? Links { get; set; }

        public IEnumerable<SelectListItem>? CategoriesList { get; set; }

        public IEnumerable<SelectListItem>? TopicsList { get; set; }

        public PageViewModel? PageViewModel { get; set; }

        public ResponseViewModel? ResponseViewModel { get; set; }

        public FilterViewModel()
        {
            ResponseViewModel = new();
        }
    }
}
