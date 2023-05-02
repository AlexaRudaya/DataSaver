namespace DataSaver.ApplicationCore.ViewModels
{
    public sealed class PageViewModel
    {
        public int PageSize { get; set; }

        public int PageNumber { get; private set; }

        public int TotalPages { get; private set; }

        public PageViewModel()
        {
            
        }

        public PageViewModel(int count = 1, int pageNumber = 1)
        {
            PageSize = 3;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}
