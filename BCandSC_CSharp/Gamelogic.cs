using System.Data.SqlClient;
using System.Data;

namespace BCandSC_CSharp
{
    public class Gamelogic
    {
        private User user1;
        private User user2;
        private int matchDay;
        public Gamelogic(User User1, User User2, int matchDay)
        {
            user1 = User1;
            user2 = User2;
            this.matchDay = matchDay;
        }



        public void GetResultsForMatch()
        {
            Database db = new();
            Dictionary<int, int> MatchDayResults = GetResultsFormApi(matchDay);

            //int pointsForTeamUser1 = GetPointsForEachTeam(user1, MatchDayResults);
            //int pointsForTeamUser2 = GetPointsForEachTeam(user2, MatchDayResults);

            //if (pointsForTeamUser1 > pointsForTeamUser2)
            //{
            //    Console.WriteLine(user1.Name + " wins" + " with Coins");
            //}
            //else
            //{
            //    Console.WriteLine(user2.Name + " wins" + " with Coins");
            //}

        }


        public Dictionary<int, int> GetResultsFormApi(int matchDay)
        {
            Database db = new();
            Dictionary<int, int> MatchDayResults = new Dictionary<int, int>();
            SqlCommand command = new SqlCommand("SELECT * FROM PlayerPoints WHERE matchday =@matchDay");

            SqlParameter param = new SqlParameter
            {
                ParameterName = "@matchDay",
                Value = matchDay,
                SqlDbType = SqlDbType.Int
            };
            command.Parameters.Add(param);

            db.conn.Open();
            command.Connection = db.conn;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read() == true)
            {
                int playerID = reader.GetInt32("player_id");
                int points = reader.GetInt32("points");

                MatchDayResults.Add(playerID, points);

            }
            reader.Close();
            db.conn.Close();
            Console.WriteLine("Got Results From Api");

            return MatchDayResults;
        }

    }

}
