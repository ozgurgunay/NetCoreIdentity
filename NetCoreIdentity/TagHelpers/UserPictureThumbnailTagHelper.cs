using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NetCoreIdentity.TagHelpers
{
    //after this class you must add ViewImports.cshtml file in this file!!!
    public class UserPictureThumbnailTagHelper : TagHelper
    {
        public string? PictureUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            
            if(string.IsNullOrEmpty(PictureUrl) )
            {
                output.Attributes.SetAttribute("src", "/userPictures/default_user_picture.jpg");
            }
            else
            {
                output.Attributes.SetAttribute("src", $"/userPictures/{PictureUrl}");
            }
        }
    }
}
