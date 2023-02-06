using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace BCandSC_CSharp
{
    public class Gamelogic
    {
        private List<User> Users = new List<User>();

        private int matchDay;
        public Gamelogic(int matchDay)
        {

            this.matchDay = matchDay;
        }

        public void AddUser(User user)
        {
            Users.Add(user);
        }



        private Dictionary<User, int> GetTotalPointsForUser()
        {
            Dictionary<User, int> PointsForUsers = new();
            Database db = new();

            SqlCommand command = new SqlCommand("SELECT TotalPoints, user_id FROM UserMatchdayPoints WHERE matchday=@matchday");

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
                int userID = reader.GetInt32("user_id");
                int points = reader.GetInt32("TotalPoints");


                var user = Users.SingleOrDefault(x => x.Id == userID);

                if (user == null)
                    throw new Exception();


                PointsForUsers.Add(user, points);

            }
            reader.Close();
            db.conn.Close();

            return PointsForUsers;
        }


        public void GetResultsForMatch()
        {

            Dictionary<User, int> MatchDayResults = GetTotalPointsForUser();

            foreach (var item in MatchDayResults.OrderByDescending(key => key.Value))
            {
                Console.WriteLine(item.Key.Name + ":" + item.Value);
            }



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
