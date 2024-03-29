﻿namespace DataSaver.ApplicationCore.ViewModels
{
    public sealed class ResponseViewModel
    {
        public int? CategoryId { get; set; }
        public int? TopicId { get; set; }
        public string? SearchTerm { get; set; }
        public int? SortOrderId { get; set; }
        public string? SortOrder { get; set; }
    }
}
