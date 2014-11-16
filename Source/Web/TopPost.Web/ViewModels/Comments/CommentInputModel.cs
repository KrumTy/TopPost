namespace TopPost.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;
    using TopPost.Models;
    using TopPost.Web.Infrastructure.Mapping;

    public class CommentInputModel : IMapFrom<Comment>
    {
        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Text Field")]
        public string Text { get; set; }

        public int PostId { get; set; }

        public int? ParentCommentId { get; set; }
    }
}