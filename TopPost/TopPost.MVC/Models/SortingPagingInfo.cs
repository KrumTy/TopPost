using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
namespace TopPost.MVC.Models
{
    public class SortingPagingInfo
    {
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public string TitleFilter { get; set; }
        public string FilterBy { get; set; }
        public string Filter { get; set; }

        public static SortingPagingInfo Default()
        {
            return new SortingPagingInfo()
            {
                CurrentPageIndex = 0,
                PageSize = 50,
                SortDirection = "Ascending",
                SortField = "Title",
                FilterBy = "Title"
            };
        }

        public static List<SelectListItem> ShowPagesList(string exclude)
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "10",
                    Value = "10"
                },
                new SelectListItem
                {
                    Text = "20",
                    Value = "20"
                },
                new SelectListItem
                {
                    Text = "30",
                    Value = "30"
                },
                new SelectListItem
                {
                    Text = "40",
                    Value = "40"
                },
                new SelectListItem
                {
                    Text = "50",
                    Value = "50"
                },
                new SelectListItem
                {
                    Text = "100",
                    Value = "100"
                }
            };

            return list.Where(x => x.Text != exclude).ToList();
        }

        public static List<SelectListItem> SortByList(string exclude)
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "Title",
                    Value = "Title"
                },
                new SelectListItem
                {
                    Text = "Description",
                    Value = "Description"
                },
                new SelectListItem
                {
                    Text = "Created",
                    Value = "Created"
                },
                new SelectListItem
                {
                    Text = "Comments",
                    Value = "Comments"
                },
                new SelectListItem
                {
                    Text = "Favorites",
                    Value = "Favorites"
                },
                new SelectListItem
                {
                    Text = "Likes",
                    Value = "Likes"
                }
            };

            return list.Where(x => x.Text != exclude).ToList();
        }

        public static List<SelectListItem> OrderByList(string exclude)
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "Ascending",
                    Value = "Ascending"
                },
                new SelectListItem
                {
                    Text = "Descending",
                    Value = "Descending"
                }
            };

            return list.Where(x => x.Text != exclude).ToList();
        }

        public static List<SelectListItem> FilterByList(string exclude)
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "Title",
                    Value = "Title"
                },
                new SelectListItem
                {
                    Text = "Tag",
                    Value = "Tag"
                }
            };

            return list.Where(x => x.Text != exclude).ToList();
        }
    }
}