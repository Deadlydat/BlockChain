using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;

namespace BCandSC_CSharp.Pages
{
    public class MatchdayModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public int userId { get; set; }
        public int Matchday { get; set; } = 0;
        Team team { get; set; } = new();
        public List<Team> teams { get; set; } = new();
        public User user { get; set; } = new();
        public double AccountBalanceETH { get; set; } = 0;
        public double AccountBalanceEUR { get; set; } = 0;
        public double AccountBalanceUSD { get; set; } = 0;
        [BindProperty(SupportsGet = true)]
        public bool done { get; set; } = false;

        public IActionResult OnGet()
        {
            Matchday = Enviroment.GetEnviroment().Matchday;
            Team t = team.GetTeam(userId, Matchday);

            if (t.Formation != "" && t.Done == false)
                return RedirectToPage("/PlayerSelection", new { UserId = userId });

            if (t.Id > 0 && t.Done == false)
                return RedirectToPage("/Formation", new { userId = userId });


            teams = t.GetTeamList(userId);
            teams.Reverse();
            user = user.GetUser(userId);


            Gamelogic gamelogic = new Gamelogic(Matchday);

            BlockchainInterface blockchainInterface = new(user);

            if (gamelogic.CheckCurrentMatchday() || done == true)
            {
                decimal balance = blockchainInterface.GetAccountBalance();
                MoneyConversion.DataObject data = MoneyConversion.TurnAccountBalanceInFiatMoney(balance);

                AccountBalanceETH = Decimal.ToDouble(balance);
                AccountBalanceEUR = data.EUR;
                AccountBalanceUSD = data.USD;

                user.SetUserBalance(userId, balance);
            }
            else
            {
                var balance = user.GetUserBalance(userId);
                var data = MoneyConversion.TurnAccountBalanceInFiatMoney(balance);

                AccountBalanceETH = decimal.ToDouble(balance);
                AccountBalanceEUR = data.EUR;
                AccountBalanceUSD = data.USD;
            }

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