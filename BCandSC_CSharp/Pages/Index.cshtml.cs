using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
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
            //foreach(Player p in GetPlayers())
            //{
            //    //if(p.Punkte > 30)
            //    //{
            //    //    p.Punkte = p.Punkte - 30;
            //    //}

            //    CreatePlayer(p.Id, 5, p.Punkte - 21);
            //}

            

            //return RedirectToPage("/Formation", new { userId = 2 });

            //return RedirectToPage("/Matchday", new { userId = 42 });
            //return RedirectToPage("/Simulation");
            return Page();
        }


        public void CreatePlayer(int playerId, int matchday, int points)
        {
            Database db = new();
            try
            {
                SqlCommand command = new SqlCommand($"INSERT INTO PlayerPoints ([player_id],[matchday],[points]) VALUES ({playerId},{matchday},{points})");

                db.conn.Open();
                command.Connection = db.conn;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                db.conn.Close();
            }
        }

        public List<Player> GetPlayers()
        {
            User u = new();
            List<Player> list = new List<Player>();
            Database db = new();
            SqlCommand command = new SqlCommand("SELECT * FROM PlayerPoints");

            db.conn.Open();
            command.Connection = db.conn;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Player p = new();
                p.Id = reader.GetInt32("player_id");
                p.Punkte = reader.GetInt32("points");
                list.Add(p);
            }
            reader.Close();
            db.conn.Close();

            return list;
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