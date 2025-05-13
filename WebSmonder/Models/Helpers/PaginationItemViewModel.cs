using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace WebSmonder.Models.Helpers
{
    public class PaginationItemViewModel
    {
        public int Page { get; set; } = 1;
        [Display(Name = "Кількість записів на сторінці")]
        [Range(1, 100, ErrorMessage = "Кількість записів на сторінці повинна бути від 1 до 100")]
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
}
