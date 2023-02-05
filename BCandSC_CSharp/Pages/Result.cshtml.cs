using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BCandSC_CSharp.Pages
{
    public class ResultModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int userId { get; set; }
        public Team team { get; set; } = new();
        public IActionResult OnGet()
        {
            team = team.GetTeam(userId, Enviroment.GetEnviroment().Matchday, true);

            return Page();
        }
    }
}
