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
            Team t = new();
            if(t.GetTeam(userId, Enviroment.GetEnviroment().Matchday).Players.Count == 11)
                return RedirectToPage("/Result", new { userId = userId });

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
