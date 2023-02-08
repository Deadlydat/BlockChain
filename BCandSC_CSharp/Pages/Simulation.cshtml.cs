using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BCandSC_CSharp.Pages
{
    public class SimulationModel : PageModel
    {
        public Gamelogic gamelogic = new(Enviroment.GetEnviroment().Matchday);
        public BlockchainInterface BlockchainInterface = new BlockchainInterface();


        public IActionResult OnGet()
        {


            return Page();
        }

        public IActionResult OnPost()
        {
            if (Request.Query["method"] == "startgame")
            {
                Console.WriteLine("starting game...");
                BlockchainInterface.StartGame();
                Console.WriteLine("started game. bets are closed");
                Console.WriteLine("");
            }


            if (Request.Query["method"] == "endgame")
            {
                Console.WriteLine("game is almost over..");
                BlockchainInterface.FinishGame();
                Console.WriteLine("finished game");
                Console.WriteLine("");
            }

            if (Request.Query["method"] == "prices")
            {
                Console.WriteLine("getting results from game..");
                List<string> winnerTeam = gamelogic.GetResultsForMatch();
                BlockchainInterface.DistributePrices(winnerTeam);
                Console.WriteLine("winner team: " + winnerTeam[0] + "-> end matchday");

                MoneyConversion.CalculateTransactionForParticipants(Enviroment.GetEnviroment().Matchday);
                Console.WriteLine("new matchday!");
                Enviroment.SetMatchday();
                Console.WriteLine("");

            }



            return Page();
        }
    }
}