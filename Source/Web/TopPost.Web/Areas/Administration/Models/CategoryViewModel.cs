namespace TopPost.Web.Areas.Administration.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using TopPost.Models;
    using TopPost.Web.Infrastructure.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        //[HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        //public DateTime CreatedOn { get; set; }

        //public DateTime? ModifiedOn { get; set; }


        [Required]
        [UIHint("String")]
        public string Name { get; set; }
    }
}