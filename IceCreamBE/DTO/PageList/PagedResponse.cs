namespace IceCreamBE.DTO.PageList
{
    public class PagedResponse<T> : Response<T>
    {
        public int currentPage { get; set; }
        public int PageSize { get; set; }
        //public Uri? FirstPage { get; set; }
        //public Uri? LastPage { get; set; }
        public int? TotalPages { get; set; }
        public int TotalRecords { get; set; }
        //public Uri? NextPage { get; set; }
        //public Uri? PreviousPage { get; set; }
        //public PagedResponse(T Data, int pageNumber, int pageSize, bool succeeded, string? message, string?[] error)
        //{
        //    this.Succeeded = succeeded;
        //    this.PageNumber = pageNumber;
        //    this.PageSize = pageSize;
        //    this.Data = Data;
        //    this.Message = message;
        //    this.Errors = error;
        //}
    }
}
