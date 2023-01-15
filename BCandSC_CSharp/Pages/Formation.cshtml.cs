using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace BCandSC_CSharp.Pages
{
    public class FormationModel : PageModel
    {
        [BindProperty]
        static public int StrikerCount { get; set; }
        [BindProperty]
        static public int MidfielderCount { get; set; }
        [BindProperty]
        static public int DefenderCount { get; set; }
        [BindProperty]
        static public string FormationString { get; set; }
        [BindProperty]
        static public List<Player> CarouselPlayers { get; set; }
        [BindProperty]
        static public List<Player> Strikers { get; set; }
        [BindProperty]
        static public List<Player> Midfielders { get; set; }
        [BindProperty]
        static public List<Player> Defenders { get; set; }
        [BindProperty]
        static public Player Goalkeeper { get; set; }

        public void OnGet()
        {
            CarouselPlayers = new();
            Strikers = new();
            Midfielders = new();
            Defenders = new();
            Goalkeeper = new();

            //Noch die anderen Formationen einfügen
            if (Request.QueryString.ToString().Contains("352"))
            {
                StrikerCount = 2;
                MidfielderCount = 5;
                DefenderCount = 3;







                //nur für test
                Database db = new();
                Team team1 = db.GetSavedTeam(1);
                Team team2 = db.GetSavedTeam(2);

                User user1 = new("Peter Lustig","plustig",12222);
                User user2 = new("Bob Ross", "Ross", 44444);
                user1.Team= team1;
                user2.Team= team2;


                Gamelogic gamelogic=new Gamelogic(user1, user2,1);

                gamelogic.GetResultsForMatch();





            }
            FormationString = "f" + DefenderCount.ToString() + MidfielderCount.ToString() + StrikerCount.ToString();
        }

        public void OnPostAddStriker()
        {
            Database db = new();
            CarouselPlayers = db.GetPlayers().Where(p => p.Position == Player.PlayerPosition.Striker).ToList();
            foreach(Player p in Strikers)
            {
                CarouselPlayers.Remove(p);
            }
        }

        public void OnPostAddMidfielder()
        {
            Database db = new();
            CarouselPlayers = db.GetPlayers().Where(p => p.Position == Player.PlayerPosition.Midfield).ToList();
            foreach (Player p in Midfielders)
            {
                CarouselPlayers.Remove(p);
            }
        }

        public void OnPostAddDefender()
        {
            Database db = new();
            CarouselPlayers = db.GetPlayers().Where(p => p.Position == Player.PlayerPosition.Defense).ToList();
            foreach (Player p in Defenders)
            {
                CarouselPlayers.Remove(p);
            }
        }

        public void OnPostAddGoalkeeper()
        {
            Database db = new();
            CarouselPlayers = db.GetPlayers().Where(p => p.Position == Player.PlayerPosition.Goal).ToList();
            CarouselPlayers.Remove(Goalkeeper);
        }

        public void OnPostAddCarouselPlayer()
        {
            if (Request.QueryString.ToString().Contains("player="))
            {
                int _playerIndex = Convert.ToInt32(Request.Query["player"]);
                Player _selectedPlayer = CarouselPlayers[_playerIndex];

                //Je nachdem welche Position der gewählte Spieler hat in die Liste der Striker, Defender oder Midfielder adden
                if (_selectedPlayer.Position == Player.PlayerPosition.Goal)
                    Goalkeeper = _selectedPlayer;
                if (_selectedPlayer.Position == Player.PlayerPosition.Defense)
                    Defenders.Add(_selectedPlayer); 
                if (_selectedPlayer.Position == Player.PlayerPosition.Midfield)
                    Midfielders.Add(_selectedPlayer);
                if (_selectedPlayer.Position == Player.PlayerPosition.Striker)
                    Strikers.Add(_selectedPlayer);

                CarouselPlayers.Clear();
            }
        }
    }
}
