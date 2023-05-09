using Microsoft.AspNetCore.Mvc.Filters;

namespace IceCreamBE.DTO.PageList
{
    public class PagedResponseDetail<T>
    {
        public int current_page { get; set; }
        public int Page_pize { get; set; }
        //public Uri? FirstPage { get; set; }
        //public Uri? LastPage { get; set; }
        public int total_pages { get; set; }
        public int total_records { get; set; }
        //public Uri? NextPage { get; set; }
        //public Uri? PreviousPage { get; set; }
        //public PagedResponseDetail(List<T> result, PaginationFilter<T> pageFilter)
        //{
        //    current_page = pageFilter.PageNumber,
        //    Page_pize = pageFilter.PageSize,
        //    total_pages = (int)Math.Ceiling((double)result.Count / (double)filter.PageSize),
        //    total_records = result.Count
        //}
    }
}
