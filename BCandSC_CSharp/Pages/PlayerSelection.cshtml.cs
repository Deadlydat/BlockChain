using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Web;

namespace BCandSC_CSharp.Pages
{
    public class PlayerSelectionModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; } = -1;
        public Enviroment enviroment = new();
        public Team team { get; set; } = new();
        public Player player { get; set; } = new();
        public List<Player.PlayerPosition> FormationList { get; set; } = new();
        public List<Player> CarouselPlayers { get; set; } = new();
        [BindProperty] 
        public int BetAmount { get; set; } = 0;


        public IActionResult OnGet()
        {
            TempData.Clear();

            if (Request.Query.ContainsKey("user"))
                UserId = Convert.ToInt32(Request.Query["user"].ToString());

            getValues();
            setValues();

            return Page();
        }

        public IActionResult OnPost()
        {

            //fï¿½r test
            //BlockchainInterface abi = new BlockchainInterface();



            //BlockchainAPI.GetETHValueFromApi();



            getValues();

            if (Request.Query["method"] == "test")
                test();

            //Spielerauswahl anzeigen
            if (Request.Query["method"] == "showcarousel")
                ShowCarousel(Enum.Parse<Player.PlayerPosition>(Request.Query["addtype"]));

            //Ausgewï¿½hlten Spieler dem Team hinzufï¿½gen
            if (Request.Query["method"] == "addplayer")
                AddPlayerToTeam();

            //Ausgewählten Spieler aus dem Team löschen
            if (Request.Query["method"] == "removeplayer")
                RemovePlayerFromTeam();

            //Team erstellen noch auf Testbasis (Wo kommt matchday her? Teamname Eingabe hinzufï¿½gen)
            if (Request.Query["method"] == "done")
            {
                //Gamelogic.SetUserToUserMatchDayList(UserId, enviroment.Matchday);

                User user = new User();
                user = user.GetUser(UserId);

                MoneyConversion.BetCertainAmount(team.Name, BetAmount, user);
                team.SetTeamDone(true, team.Id);

           
                return RedirectToPage("/Matchday", new { userId = UserId });
            }

            setValues();

            return Page();
        }

        public void getValues()
        {
            enviroment = Enviroment.GetEnviroment();

            if (TempData["userid"] != null)
                UserId = Convert.ToInt32(TempData["userid"]);

            team = team.GetTeam(UserId, enviroment.Matchday, true);
            enviroment.Coins -= team.Players.Sum(p => p.Marktwert);
            setFormation();
        }

        public void setValues()
        {
            enviroment = Enviroment.GetEnviroment();
            TempData["userid"] = UserId.ToString();
            team = team.GetTeam(UserId, enviroment.Matchday, true);
            enviroment.Coins -= team.Players.Sum(p => p.Marktwert);
        }

        public void ShowCarousel(Player.PlayerPosition position)
        {
            CarouselPlayers = player.GetPlayers(position).Where(p => p.Marktwert < enviroment.Coins).ToList();
            foreach (Player p in team.Players)
                if (CarouselPlayers.Find(x => x.Id == p.Id) != null) { CarouselPlayers.Remove(CarouselPlayers.Find(x => x.Id == p.Id)!); }
        }

        public void RemovePlayerFromTeam()
        {
            player.RemovePlayerFromTeam(Convert.ToInt32(Request.Query["player"]), team.Id);
        }

        public void test()
        {
            player.SetPlayerToTeam(50, team.Id);
            player.SetPlayerToTeam(16, team.Id);
            player.SetPlayerToTeam(18, team.Id);
            player.SetPlayerToTeam(33, team.Id);
            player.SetPlayerToTeam(36, team.Id);
            player.SetPlayerToTeam(38, team.Id);
            player.SetPlayerToTeam(41, team.Id);
            player.SetPlayerToTeam(44, team.Id);
            player.SetPlayerToTeam(45, team.Id);
            player.SetPlayerToTeam(64, team.Id);
            player.SetPlayerToTeam(66, team.Id);
        }

        public void AddPlayerToTeam()
        {
            player.SetPlayerToTeam(Convert.ToInt32(Request.Query["player"]), team.Id);
        }

        public void setFormation()
        {
            for (int i = 0; i < (int)Convert.ToUInt32(team.Formation[3].ToString()); i++) { FormationList.Add(Player.PlayerPosition.Striker); }
            for (int i = 0; i < (int)Convert.ToUInt32(team.Formation[2].ToString()); i++) { FormationList.Add(Player.PlayerPosition.Midfield); }
            for (int i = 0; i < (int)Convert.ToUInt32(team.Formation[1].ToString()); i++) { FormationList.Add(Player.PlayerPosition.Defense); }
            FormationList.Add(Player.PlayerPosition.Goal);
        }
    }
}
