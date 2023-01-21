using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace BCandSC_CSharp.Pages
{
    public class FormationModel : PageModel
    {
        [BindProperty (SupportsGet = true)]
        public int userId { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPostTest()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
