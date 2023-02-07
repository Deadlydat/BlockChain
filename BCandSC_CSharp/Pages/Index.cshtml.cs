using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipelines;

namespace BCandSC_CSharp.Pages
{
    public class LoginModel : PageModel
    {
        [Required]
        [BindProperty]
        public string Name { get; set; } = "";

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
        User user { get; set; } = new();

        public IActionResult OnGet()
        {
            //List<string> strings = new();
            //int counter = 15;

            //foreach(string line in System.IO.File.ReadAllLines("C:/temp/test.txt"))
            //{
            //    Player player = new Player();
            //    player.Position = (Player.PlayerPosition)Convert.ToInt32(line.Split("\t")[0]);
            //    player.Points = Convert.ToInt32(line.Split("\t")[1]);
            //    player.Name = line.Split("\t")[2];
            //    player.Marktwert = Convert.ToInt32(line.Split("\t")[3]);
            //    player.Nummer = Convert.ToInt32(line.Split("\t")[4]);
            //    player.Einsaetze = Convert.ToInt32(line.Split("\t")[5]);
            //    player.Tore = Convert.ToInt32(line.Split("\t")[6].Replace("-", "0"));
            //    player.Mannschaft = line.Split("\t")[7];

            //    //strings.Add($"INSERT INTO Player ([name],[verein],[position],[marktwert],[einsaetze],[tore]) VALUES ('{player.Name}','{player.Mannschaft}',{(int)player.Position},{player.Marktwert},{player.Einsaetze},{player.Tore});");
            //    strings.Add($"INSERT INTO PlayerPoints ([player_id],[matchday],[points]) VALUES ({counter},1,{player.Points});");
            //    counter++;
            //}
            //System.IO.File.WriteAllLines("C:/temp/sql.txt", strings);
            //Player p = new();
           //return RedirectToPage("/Formation", new { userId = 2 });

             return RedirectToPage("/Matchday", new { userId = 14 });
            //return RedirectToPage("/Simulation");
              return Page();
        }


        public IActionResult OnPostLogin()
        {
            user = user.GetUser(Name, Password);
            if (user.Id > 0)
                return RedirectToPage("/Matchday", new { userId = user.Id });

            return Page();
        }

        public IActionResult OnPostSignUp()
        {
            user = user.SetUser(Name, Password);
            if (user.Id > 0)
                return RedirectToPage("/Matchday", new { userId = user.Id });

            return Page();
        }

    }
}