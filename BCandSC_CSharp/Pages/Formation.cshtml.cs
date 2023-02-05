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
        Team t = new();

        public IActionResult OnGet()
        {            
            if(t.GetTeam(userId, Enviroment.GetEnviroment().Matchday).Players.Count == 11)
                return RedirectToPage("/Result", new { userId = userId });

            return Page();
        }

        public IActionResult OnPost()
        {
            t = t.GetTeam(userId, Enviroment.GetEnviroment().Matchday);
            t.SetFormation(t.Id, "f" + Request.Query["formation"]);

            return RedirectToPage("/PlayerSelection", new { userId = userId }); ;
        }
    }
}
