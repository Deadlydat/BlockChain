using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BCandSC_CSharp.Pages
{
    public class SimulationModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (Request.Query["method"] == "startgame")
            {
                //Start Methode im Smartcontract, um Wetten und Teamcreation freizuschalten
            }
            if (Request.Query["method"] == "endgame")
            {
                //Preise verteilen
            }
            return Page();
        }
    }
}
