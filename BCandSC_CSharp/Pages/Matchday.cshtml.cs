using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;

namespace BCandSC_CSharp.Pages
{
    public class MatchdayModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int userId { get; set; }
        public int Matchday { get; set; } = 0;
        Team team { get; set; } = new();
        public List<Team> teams { get; set; } = new();
        public User user { get; set; } = new();


        public IActionResult OnGet()
        {
            Matchday = Enviroment.GetEnviroment().Matchday;
            Team t = team.GetTeam(userId, Matchday);

            //if (t.Players.Count == 11)
            //    return RedirectToPage("/Result", new { userId = userId });

            //if (t.Formation != "")
            //    return RedirectToPage("/PlayerSelection", new { UserId = userId });

            //if (t.Id > 0)
            //    return RedirectToPage("/Formation", new { userId = userId });


            teams = t.GetTeamList(userId);
            user = user.GetUser(userId);

            return Page();
        }

        public IActionResult OnPost()
        {
            User u = new();
            u = u.GetUser(userId);

            team.CreateTeam(userId, $"Team {u.Name} für Spieltag {Enviroment.GetEnviroment().Matchday}", Enviroment.GetEnviroment().Matchday);


            return RedirectToPage("/Formation", new { userId = userId });
        }
    }
}
