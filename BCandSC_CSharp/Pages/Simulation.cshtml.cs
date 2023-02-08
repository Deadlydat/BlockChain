using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BCandSC_CSharp.Pages
{
    public class SimulationModel : PageModel
    {

        public Gamelogic gamelogic = new(Enviroment.GetEnviroment().Matchday);
        public BlockchainInterface BlockchainInterface = new();
        private String adminKey = "0x2a0635d67d97ae1abeaab336f7c409acbc4330cbd30eab78934cab02e147d0af";


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
                Console.WriteLine("");

            }


            if (Request.Query["method"] == "endgame")
            {
                Console.WriteLine("game is almost over..");
                BlockchainInterface.FinishGame(adminKey);
                Console.WriteLine("finished game");
                Console.WriteLine("");



            }

            if (Request.Query["method"] == "prices")
            {
                Console.WriteLine("getting results from game..");
                Team winnerTeam = gamelogic.GetResultsForMatch();
                BlockchainInterface.DistributePrices(winnerTeam.Name, adminKey);
                Console.WriteLine("winner team: " + winnerTeam.Name + "-> end matchday");
             


            }




            if (Request.Query["method"] == "money")
            {

                MoneyConversion.CalculateTransactionForParticipants(Enviroment.GetEnviroment().Matchday);
                Console.WriteLine("new matchday!");
                Enviroment.SetMatchday();
                Console.WriteLine("");
                //MoneyConversion.DataObject data = MoneyConversion.GetETHValueFromApi().Result;
                //Console.WriteLine(data.USD + " " + data.EUR);

                //User user = new User();
                //user = user.GetUser(45);

                //MoneyConversion.DataObject data2 = MoneyConversion.GetAccountBalanceInFiatMoney(user);
                //Console.WriteLine(data2.USD + " " + data2.EUR);

                //Console.WriteLine("money: " + BlockchainInterface.GetMoneyPool());
                //Console.WriteLine("players: " + BlockchainInterface.GetPlayerCount());

                //User user = new();

                //decimal balance = BlockchainInterface.GetAccountBalance("0x563ADff6863de0853B305B61E2DdfCd91bD8c448");
                //user.SetUserBalance(45, balance);



                Console.WriteLine("");


            }



            return Page();
        }
    }
}
