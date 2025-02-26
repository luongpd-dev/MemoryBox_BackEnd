using MemoryBox.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.Utils.Pagination
{
    public class PaginatedList<T> : IPaginatedList<T>
    {
        public IReadOnlyCollection<T> Items { get; private set; }
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public int PageSize { get; private set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public PaginatedList(IReadOnlyCollection<T> items, int totalItems, int currentPage, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}
