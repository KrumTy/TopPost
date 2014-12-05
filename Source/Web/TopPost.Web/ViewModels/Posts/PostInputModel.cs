namespace TopPost.Web.ViewModels.Posts
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;

    public class PostInputModel
    {
        [Required]
        [Display(Name = "Title")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [AllowHtml]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        [DataType("tinymce_full")]
        [UIHint("tinymce_full")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [AllowHtml]
        public string Description { get; set; }

        // TODO: Create custon validation for the tags
        [Required]
        [Display(Name = "Tags (separated by ',')")]
        [AllowHtml]
        public string StringTags { get; set; }
        
        [Required]
        [Display(Name = "Image")]
        public HttpPostedFileBase file { get; set; }
    }
}