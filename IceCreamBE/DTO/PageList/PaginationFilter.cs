namespace IceCreamBE.DTO.PageList
{
    public class PaginationFilter<T>
    {
        public int PageNumber { get; set; } // page can lay
        public int PageSize { get; set; }   // item/page
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = int.MaxValue;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public List<T> GetPageList<T>(List<T> list)
        {
            int firstItemIndex = 0,
                lastItemIndex = 0,
                pageCount = GetPageCount(list.Count, PageSize);

            if (list.Count != 0)
            {
                PageNumber = PageNumber < 1 ? 1 : PageNumber;
                PageNumber = PageNumber > pageCount ? pageCount : PageNumber;

                firstItemIndex = (PageNumber * PageSize) - PageSize;
                lastItemIndex = (PageNumber * PageSize) - 1;

                if ((PageNumber * PageSize) > list.Count)
                {
                    PageSize = PageSize - ((PageNumber * PageSize) - list.Count);
                }

                return list.GetRange(firstItemIndex, PageSize);
            }
            return null;
        }

        public int GetPageCount(int countList, int PageSize)
        {
            return (int)Math.Ceiling((double)countList / (double)PageSize);
        }

    }
}
