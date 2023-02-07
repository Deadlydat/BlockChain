using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BCandSC_CSharp.Pages
{
    public class SimulationModel : PageModel
    {

        Gamelogic gamelogic;
        BlockchainInterface BlockchainInterface;
        private String adminKey = "0xc815fa507181842ea2c85b12d75dc162b633d1c69d87e351bb86406f3db5b72f";


        public IActionResult OnGet()
        {

            return Page();
        }

        public IActionResult OnPost()
        {

            if (Request.Query["method"] == "startmatchday")
            {
                gamelogic = new Gamelogic(Enviroment.GetEnviroment().Matchday);
                BlockchainInterface = new BlockchainInterface();
                Console.WriteLine("started Matchday now you can bet");

            }

            if (Request.Query["method"] == "startgame")
            {
                //gamelogic.UsersId = gamelogic.GetUsersForMatchDay();
                BlockchainInterface.StartGame(adminKey);
                Console.WriteLine("started game betting is closed");

            }
            if (Request.Query["method"] == "endgame")
            {
                BlockchainInterface.FinishGame(adminKey);
                Console.WriteLine("finished game");


            }

            if (Request.Query["method"] == "endmatchday")
            {
                Team winnerTeam = gamelogic.GetResultsForMatch();
                BlockchainInterface.DistributePrices(winnerTeam.Name, adminKey);
                Console.WriteLine("winner team "+winnerTeam.Name+" end machtday");

                //matchday +1


            }


            return Page();
        }
    }
}
