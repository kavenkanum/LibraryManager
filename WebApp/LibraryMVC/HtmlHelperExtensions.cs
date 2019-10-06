using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryMVC
{
    public static class HtmlHelperExtensions
    {
        public static string ToBase64Image(this IHtmlHelper htmlHelper, string image)
        {
            return $"data:image/png;base64, {image}";
        }
    }
}
