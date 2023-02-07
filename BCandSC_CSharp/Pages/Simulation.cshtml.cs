using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BCandSC_CSharp.Pages
{
    public class SimulationModel : PageModel
    {

        Gamelogic gamelogic;
        BlockchainInterface BlockchainInterface;
        private String adminKey = "";
        

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

            }

            if (Request.Query["method"] == "startgame")
            {
                gamelogic.UsersId = gamelogic.GetUsersForMatchDay();
                BlockchainInterface.StartGame(adminKey);
                //Start Methode im Smartcontract, um Wetten und Teamcreation freizuschalten
            }
            if (Request.Query["method"] == "endgame")
            {
                BlockchainInterface.FinishGame(adminKey);

                //Preise verteilen
            }

            if (Request.Query["method"] == "endmatchday")
            {
                gamelogic.GetResultsForMatch();
                //BlockchainInterface.DistributePrices()

                    //matchday +1

                //Start Methode im Smartcontract, um Wetten und Teamcreation freizuschalten
            }


            return Page();
        }
    }
}
