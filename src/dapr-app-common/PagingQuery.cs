using System.Reflection.PortableExecutable;
using System.Reflection;

namespace DaprApp.Common
{
    public class PagingQuery<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<T> Results { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PagingQueryMetadata Metadata { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagingQuery(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Results = source.Skip((PageIndex - 1) * PageSize).Take(PageSize);
            Metadata = GeneratePagingQueryMetadata();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private PagingQueryMetadata GeneratePagingQueryMetadata()
        {
            var result = new PagingQueryMetadata();
            //PropertyInfo[] props = typeof(T).GetProperties();
            return result;
        }
    }
}