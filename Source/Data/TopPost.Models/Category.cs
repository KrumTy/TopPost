namespace TopPost.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TopPost.Data.Common.Models;

    public class Category : DeletableEntity
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }    
    }
}