namespace DataSaver.ApplicationCore.ViewModels
{
    public sealed class LinkForCreateViewModel
    {
        public LinkViewModel? LinkVM { get; set; } 

        public IEnumerable<SelectListItem>? Categories { get; set; }

        public IEnumerable<SelectListItem>? Topics { get; set; }
    }
}
