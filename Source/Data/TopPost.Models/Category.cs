namespace TopPost.Models
{
    using System.Collections.Generic;
    using TopPost.Data.Common.Models;

    public class Category : DeletableEntity
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }    
    }
}