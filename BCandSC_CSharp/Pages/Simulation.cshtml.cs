using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BCandSC_CSharp.Pages
{
    public class SimulationModel : PageModel
    {

        public Gamelogic gamelogic = new(Enviroment.GetEnviroment().Matchday);
        public BlockchainInterface BlockchainInterface = new();
        private String adminKey = "0xc815fa507181842ea2c85b12d75dc162b633d1c69d87e351bb86406f3db5b72f";


        public IActionResult OnGet()
        {

            return Page();
        }

        public IActionResult OnPost()
        {


            if (Request.Query["method"] == "startgame")
            {
                Console.WriteLine("starting game...");
                BlockchainInterface.StartGame(adminKey);
                Console.WriteLine("started game betting is closed");

            }


            if (Request.Query["method"] == "endgame")
            {
                Console.WriteLine("game is almost over..");
                BlockchainInterface.FinishGame(adminKey);
                Console.WriteLine("finished game");


                Team winnerTeam = gamelogic.GetResultsForMatch();
                BlockchainInterface.DistributePrices(winnerTeam.Name, adminKey);
                Console.WriteLine("winner team: " + winnerTeam.Name + "-> end matchday");
                //matchday +1

            }



            if (Request.Query["method"] == "money")
            {


            }



            return Page();
        }
    }
}
