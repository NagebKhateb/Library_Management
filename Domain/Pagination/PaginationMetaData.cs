namespace Domain.Pagination
{
    public class PaginationMetaData
    {
        public int PageSize { get; }
        public int PageNumber { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }

        public PaginationMetaData(int pageSize, int pageNumber, int totalCount)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
