using MyCRM_Online.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.Models
{
    public class PageInfo<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public PageInfo() { }

        public PageInfo(int totalCount, int currentPage, int pageSize, IEnumerable<T> items)
        {
            Items = items;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public bool HasPreviousPage {
            get { return (CurrentPage > 1); }
        }

        public bool HasNextPage {
            get { return (CurrentPage < TotalPages); }
        }
    }
}
