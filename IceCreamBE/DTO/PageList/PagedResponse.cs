namespace IceCreamBE.DTO.PageList
{
    public class PagedResponse<T>: Response<T>
    {
        public PagedResponseDetail<T> Pagination { get; set; }
    }
}
