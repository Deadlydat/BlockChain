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
        public int UserId { get; set; } = -1;
        public string FormationString { get; set; } = "";
        public List<Player.PlayerPosition> FormationList { get; set; } = new();
        public List<Player> CarouselPlayers { get; set; } = new();
        public List<Player> team { get; set; } = new();

        public IActionResult OnGet()
        {
            if (Request.Query.ContainsKey("formation"))
                FormationString = Request.Query["formation"];
            if(Request.Query.ContainsKey("user"))
                UserId = Convert.ToInt32(Request.Query["user"].ToString());

            TempData.Clear();
            getValues();
            setFormation();
            setValues();

            return Page();
        }

        public IActionResult OnPost()
        {

            //für test
            //BlockchainInterface abi = new BlockchainInterface();
            PythonScriptGanache.RunPython();

            //BlockchainAPI.GetETHValueFromApi();



            getValues();
            setFormation();

            //Spielerauswahl anzeigen
            if (Request.Query["method"] == "showcarousel")
            {
                Player.PlayerPosition pos = Enum.Parse<Player.PlayerPosition>(Request.Query["addtype"]);
                ShowCarousel(pos);
                if(Request.Query["existingplayer"].ToString() != "Name")
                {
                    TempData["existingplayer"] = Request.Query["existingplayer"].ToString();
                }
            }

            //Ausgewählten Spieler dem Team hinzufügen
            if (Request.Query["method"] == "addplayer")
            {
                AddPlayerToTeam();
            }

            //Team erstellen noch auf Testbasis (Wo kommt matchday her? Teamname Eingabe hinzufügen)
            if (Request.Query["method"] == "done")
            {
                Team t = new();
                t.SaveTeam(UserId, team, "test", 999);
            }

            setValues();
            return Page();
        }

        public void getValues()
        {
            JsonConvert.DeserializeObject<List<Player>>("");

            if (TempData["team"] != null)
                team = JsonConvert.DeserializeObject<List<Player>>(TempData["team"]!.ToString()!)!;
            if (TempData["carousel"] != null)
                CarouselPlayers = JsonConvert.DeserializeObject<List<Player>>(TempData["carousel"]!.ToString()!)!;
            if (TempData["formation"] != null)
                FormationString = (string)TempData["formation"]!;
            if (TempData["userid"] != null)
                UserId = Convert.ToInt32(TempData["userid"])!;
        }

        public void setValues()
        {
            TempData["userid"] = UserId.ToString();
            TempData["team"] = JsonConvert.SerializeObject(team);
            TempData["carousel"] = JsonConvert.SerializeObject(CarouselPlayers); 
            TempData["formation"] = FormationString;
        }

        public void ShowCarousel(Player.PlayerPosition position)
        {
            getValues();

            Player player = new();
            CarouselPlayers = player.GetPlayers(position);

            foreach(Player p in team)
            {
                Player? tmp = CarouselPlayers.Find(pl => pl.Id == p.Id);
                if(tmp != null)
                    CarouselPlayers.Remove(tmp);
            }            

            setValues();
        }

        public void AddPlayerToTeam()
        {
            Player player = CarouselPlayers[Convert.ToInt32(Request.Query["player"])];
            team.Add(player);

            if (TempData["existingplayer"] != null)
                team.Remove(team.Find(p => p.Name == (string)TempData["existingplayer"]!)!);
            CarouselPlayers.Clear();
        }

        public void setFormation()
        {
            for(int i = 0; i < (int)Convert.ToUInt32(FormationString[3].ToString()); i++) { FormationList.Add(Player.PlayerPosition.Striker); }
            for(int i = 0; i < (int)Convert.ToUInt32(FormationString[2].ToString()); i++) { FormationList.Add(Player.PlayerPosition.Midfield); }
            for(int i = 0; i < (int)Convert.ToUInt32(FormationString[1].ToString()); i++) { FormationList.Add(Player.PlayerPosition.Defense); }
            FormationList.Add(Player.PlayerPosition.Goal);
        }
    }
}
